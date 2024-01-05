using CeciDapper.Domain.DTO.Notification;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Service.Validators.Notification;
using CeciDapper.Test.Fakers.Notification;
using CeciDapper.Test.Fakers.User;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace CeciDapper.Test.Validators.Notification
{
    public class NotificationSendValidatorTest
    {
        private readonly NotificationSendValidator _validator;
        private readonly Moq.Mock<IUnitOfWork> _mockUnitOfWork;

        public NotificationSendValidatorTest()
        {
            _mockUnitOfWork = new Moq.Mock<IUnitOfWork>();
            _validator = new NotificationSendValidator(_mockUnitOfWork.Object);
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new NotificationSendDTO();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.User.Id)} = {model.IdUser}"))
                .ReturnsAsync(value: null);

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(user => user.IdUser);
            result.ShouldHaveValidationErrorFor(user => user.Title);
            result.ShouldHaveValidationErrorFor(user => user.Body);
        }

        [Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = NotificationFaker.NotificationSendDTO().Generate();
            
            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.User.Id)} = {model.IdUser}"))
                .ReturnsAsync(UserFaker.UserEntity().Generate());

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldNotHaveValidationErrorFor(user => user.IdUser);
            result.ShouldNotHaveValidationErrorFor(user => user.Title);
            result.ShouldNotHaveValidationErrorFor(user => user.Body);
        }
    }
}
