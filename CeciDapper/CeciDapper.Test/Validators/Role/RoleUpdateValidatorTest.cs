using CeciDapper.Domain.DTO.Role;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Service.Validators.Role;
using CeciDapper.Test.Fakers.Role;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace CeciDapper.Test.Validators.Role
{
    public class RoleUpdateValidatorTest
    {
        private readonly RoleUpdateValidator _validator;
        private readonly Moq.Mock<IUnitOfWork> _mockUnitOfWork;

        public RoleUpdateValidatorTest()
        {
            _mockUnitOfWork = new Moq.Mock<IUnitOfWork>();
            _validator = new RoleUpdateValidator(_mockUnitOfWork.Object);
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new RoleUpdateDTO();

            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.Role.Id)} = {It.IsAny<int>()}"))
                .ReturnsAsync(value: null);

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(role => role.RoleId);
            result.ShouldHaveValidationErrorFor(role => role.Name);
        }

        [Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = RoleFaker.RoleUpdateDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.Role.Id)} = {model.RoleId}"))
                .ReturnsAsync(RoleFaker.RoleEntity().Generate());

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldNotHaveValidationErrorFor(role => role.RoleId);
            result.ShouldNotHaveValidationErrorFor(role => role.Name);
        }
    }
}
