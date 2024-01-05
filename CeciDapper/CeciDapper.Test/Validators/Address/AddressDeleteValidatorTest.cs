using CeciDapper.Domain.DTO.Address;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Service.Validators.Address;
using CeciDapper.Test.Fakers.Address;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace CeciDapper.Test.Validators.Address
{
    public class AddressDeleteValidatorTest
    {
        private readonly AddressDeleteValidator _validator;
        private readonly Moq.Mock<IUnitOfWork> _mockUnitOfWork;

        public AddressDeleteValidatorTest()
        {
            _mockUnitOfWork = new Moq.Mock<IUnitOfWork>();
            _validator = new AddressDeleteValidator(_mockUnitOfWork.Object);
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new AddressDeleteDTO();

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.Address.Id)} = {It.IsAny<int>()}"))
                .ReturnsAsync(value: null);

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(user => user.AddressId);
        }

        [Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = new AddressDeleteDTO
            {
                AddressId = 1
            };

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultAsync($"{nameof(Domain.Entities.Address.Id)} = {model.AddressId}"))
                .ReturnsAsync(AddressFaker.AddressEntity().Generate());

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldNotHaveValidationErrorFor(user => user.AddressId);
        }
    }
}
