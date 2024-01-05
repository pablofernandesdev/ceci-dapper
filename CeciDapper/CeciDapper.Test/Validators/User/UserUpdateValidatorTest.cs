using CeciDapper.Domain.DTO.User;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Service.Validators.User;
using CeciDapper.Test.Fakers.Role;
using CeciDapper.Test.Fakers.User;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace CeciDapper.Test.Validators.User
{
    public class UserUpdateValidatorTest
    {
        private UserUpdateValidator validator;
        private readonly Moq.Mock<IUnitOfWork> _mockUnitOfWork;

        public UserUpdateValidatorTest()
        {
            _mockUnitOfWork = new Moq.Mock<IUnitOfWork>();
            validator = new UserUpdateValidator(_mockUnitOfWork.Object);
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new UserUpdateDTO { Email = "asdf" }; 

            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.Role.Id)} = {model.RoleId}"))
                .ReturnsAsync(value: null);

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.User.Id)} = {model.UserId}"))
                .ReturnsAsync(value: null);

            //act
            var result = validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(user => user.UserId);
            result.ShouldHaveValidationErrorFor(user => user.RoleId);
            result.ShouldHaveValidationErrorFor(user => user.Name);
            result.ShouldHaveValidationErrorFor(user => user.Email);
            result.ShouldHaveValidationErrorFor(user => user.Password);
        }

        [Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = UserFaker.UserUpdateDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.Role.Id)} = {model.RoleId}"))
                .ReturnsAsync(RoleFaker.RoleEntity().Generate());

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.User.Id)} = {model.UserId}"))
                .ReturnsAsync(UserFaker.UserEntity().Generate());

            //act
            var result = validator.TestValidate(model);

            //assert
            result.ShouldNotHaveValidationErrorFor(user => user.UserId);
            result.ShouldNotHaveValidationErrorFor(user => user.RoleId);
            result.ShouldNotHaveValidationErrorFor(user => user.Name);
            result.ShouldNotHaveValidationErrorFor(user => user.Email);
            result.ShouldNotHaveValidationErrorFor(user => user.Password);
        }
    }
}
