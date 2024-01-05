using CeciDapper.Domain.DTO.User;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Service.Validators.User;
using CeciDapper.Test.Fakers.User;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace CeciDapper.Test.Validators.User
{
    public class UserDeleteValidatorTest
    {
        private readonly UserDeleteValidator _validator;
        private readonly Moq.Mock<IUnitOfWork> _mockUnitOfWork;

        public UserDeleteValidatorTest()
        {
            _mockUnitOfWork = new Moq.Mock<IUnitOfWork>();
            _validator = new UserDeleteValidator(_mockUnitOfWork.Object);
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new UserDeleteDTO();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.User.Id)} = {It.IsAny<int>()}"))
                .ReturnsAsync(value: null);

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(user => user.UserId);
        }

        [Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = new UserDeleteDTO { 
                UserId = 1};

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.User.Id)} = {model.UserId}"))
                .ReturnsAsync(UserFaker.UserEntity().Generate());

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldNotHaveValidationErrorFor(user => user.UserId);
        }
    }
}
