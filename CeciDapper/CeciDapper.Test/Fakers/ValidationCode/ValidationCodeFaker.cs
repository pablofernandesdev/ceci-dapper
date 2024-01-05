using Bogus;
using CeciDapper.Domain.DTO.ValidationCode;
using CeciDapper.Infra.CrossCutting.Extensions;
using CeciDapper.Test.Fakers.User;

namespace CeciDapper.Test.Fakers.ValidationCode
{
    public class ValidationCodeFaker
    {
        public static Faker<CeciDapper.Domain.Entities.ValidationCode> ValidationCodeEntity()
        {
            return new Faker<CeciDapper.Domain.Entities.ValidationCode>()
                .CustomInstantiator(p => new CeciDapper.Domain.Entities.ValidationCode
                {
                    UserId = p.Random.Int(),
                    Active = true,
                    Code = PasswordExtension.EncryptPassword(p.Random.Word()),
                    Expires = p.Date.Future(),
                    Id = p.Random.Int(),
                    RegistrationDate = p.Date.Recent(),
                    User = UserFaker.UserEntity().Generate()               
                });
        }

        public static Faker<ValidationCodeValidateDTO> ValidationCodeValidateDTO()
        {
            return new Faker<ValidationCodeValidateDTO>()
                .CustomInstantiator(p => new ValidationCodeValidateDTO
                {
                    Code = p.Random.Word()
                });
        }
    }
}
