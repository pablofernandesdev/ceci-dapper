using CeciDapper.Domain.DTO.Role;
using CeciDapper.Service.Validators.Role;
using FluentValidation.TestHelper;
using Xunit;

namespace CeciDapper.Test.Validators.Role
{
    public class RoleIdentifierValidatorTest
    {
        private readonly RoleIdentifierValidator _validator;

        public RoleIdentifierValidatorTest()
        {
            _validator = new RoleIdentifierValidator();
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new IdentifierRoleDTO();

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(role => role.RoleId);
        }

        [Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = new IdentifierRoleDTO{ 
                RoleId = 1};

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldNotHaveValidationErrorFor(role => role.RoleId);
        }
    }
}
