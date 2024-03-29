﻿using CeciDapper.Domain.DTO.Address;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Service.Validators.Address;
using CeciDapper.Test.Fakers.Address;
using CeciDapper.Test.Fakers.User;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace CeciDapper.Test.Validators.Address
{
    public class AddressUpdateValidatorTest
    {
        private readonly AddressUpdateValidator _validator;
        private readonly Moq.Mock<IUnitOfWork> _mockUnitOfWork;

        public AddressUpdateValidatorTest()
        {
            _mockUnitOfWork = new Moq.Mock<IUnitOfWork>();
            _validator = new AddressUpdateValidator(_mockUnitOfWork.Object);
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new AddressUpdateDTO();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.User.Id)} = {It.IsAny<int>()}"))
                .ReturnsAsync(value: null);

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.Address.Id)} = {It.IsAny<int>()}"))
                .ReturnsAsync(value: null);

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(user => user.AddressId);
            result.ShouldHaveValidationErrorFor(user => user.UserId);
            result.ShouldHaveValidationErrorFor(user => user.ZipCode);
            result.ShouldHaveValidationErrorFor(user => user.Street);
            result.ShouldHaveValidationErrorFor(user => user.District);
            result.ShouldHaveValidationErrorFor(user => user.Locality);
            result.ShouldHaveValidationErrorFor(user => user.Number);
            result.ShouldHaveValidationErrorFor(user => user.Uf);
        }

        [Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = AddressFaker.AddressUpdateDTO().Generate();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.User.Id)} = {model.UserId}"))
                .ReturnsAsync(UserFaker.UserEntity().Generate());

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultAsync($"{nameof(CeciDapper.Domain.Entities.Address.Id)} = {model.AddressId}"))
                .ReturnsAsync(AddressFaker.AddressEntity().Generate());

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldNotHaveValidationErrorFor(user => user.UserId);
            result.ShouldNotHaveValidationErrorFor(user => user.AddressId);
            result.ShouldNotHaveValidationErrorFor(user => user.ZipCode);
            result.ShouldNotHaveValidationErrorFor(user => user.Street);
            result.ShouldNotHaveValidationErrorFor(user => user.District);
            result.ShouldNotHaveValidationErrorFor(user => user.Locality);
            result.ShouldNotHaveValidationErrorFor(user => user.Number);
            result.ShouldNotHaveValidationErrorFor(user => user.Uf);
        }
    }
}
