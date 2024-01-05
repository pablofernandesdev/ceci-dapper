using CeciDapper.Domain.DTO.Register;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Service.Validators.Registration;
using CeciDapper.Test.Fakers.User;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace CeciDapper.Test.Validators.Registration
{
    public class UserSelfRegistrationValidatorTest
    {
        private readonly UserSelfRegistrationValidator _validator;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public UserSelfRegistrationValidatorTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _validator = new UserSelfRegistrationValidator(_mockUnitOfWork.Object);
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new UserSelfRegistrationDTO();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.User.Email)} = '{model.Email}'"))
                .ReturnsAsync(UserFaker.UserEntity().Generate());

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(user => user.Email);
            result.ShouldHaveValidationErrorFor(user => user.Name);
            result.ShouldHaveValidationErrorFor(user => user.Password);
        }

        [Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = UserFaker.UserSelfRegistrationDTO().Generate();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.User.Email)} = '{model.Email}'"))
                .ReturnsAsync(value: null);

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldNotHaveValidationErrorFor(user => user.Email);
            result.ShouldNotHaveValidationErrorFor(user => user.Name);
            result.ShouldNotHaveValidationErrorFor(user => user.Password);
        }
    }
}
