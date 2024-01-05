using CeciDapper.Domain.DTO.Auth;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Service.Validators.Login;
using CeciDapper.Test.Fakers.Auth;
using CeciDapper.Test.Fakers.User;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace CeciDapper.Test.Validators.Login
{
    public class ForgotPasswordValidatorTest
    {
        private readonly ForgotPasswordValidator _validator;
        private readonly Moq.Mock<IUnitOfWork> _mockUnitOfWork;

        public ForgotPasswordValidatorTest()
        {
            _mockUnitOfWork = new Moq.Mock<IUnitOfWork>();
            _validator = new ForgotPasswordValidator(_mockUnitOfWork.Object);
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new ForgotPasswordDTO();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.User.Email)} = '{It.IsAny<string>()}'"))
                .ReturnsAsync(value: null);

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(user => user.Email);
        }

        [Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = AuthFaker.ForgotPasswordDTO().Generate();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.User.Email)} = '{model.Email}'"))
                .ReturnsAsync(UserFaker.UserEntity().Generate());

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldNotHaveValidationErrorFor(user => user.Email);
        }
    }
}
