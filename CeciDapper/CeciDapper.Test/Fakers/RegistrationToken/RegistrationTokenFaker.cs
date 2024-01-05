using Bogus;
using CeciDapper.Test.Fakers.User;

namespace CeciDapper.Test.Fakers.RegistrationToken
{
    public class RegistrationTokenFaker
    {
        public static Faker<CeciDapper.Domain.Entities.RegistrationToken> RegistrationTokenEntity()
        {
            return new Faker<CeciDapper.Domain.Entities.RegistrationToken>()
                .CustomInstantiator(p => new CeciDapper.Domain.Entities.RegistrationToken
                {
                    Active = true,
                    UserId = p.Random.Int(),
                    Id = p.Random.Int(),
                    RegistrationDate = p.Date.Recent(),
                    Token = p.Random.String2(30),
                    User = UserFaker.UserEntity().Generate()
                });
        }
    }
}
