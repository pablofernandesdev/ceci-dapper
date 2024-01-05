﻿using Bogus;
using System;

namespace CeciDapper.Test.Fakers.RefreshToken
{
    public static class RefreshTokenFaker
    {
        public static Faker<CeciDapper.Domain.Entities.RefreshToken> RefreshTokenEntity()
        {
            return new Faker<CeciDapper.Domain.Entities.RefreshToken>()
                .CustomInstantiator(p => new CeciDapper.Domain.Entities.RefreshToken
                {
                    Token = p.Random.String2(100),
                    Expires = DateTime.UtcNow.AddDays(7),
                    RegistrationDate = DateTime.UtcNow,
                    CreatedByIp = "127.0.0.1",
                    UserId = p.Random.Int(),
                    User = new Domain.Entities.User{
                        Id = p.Random.Int(1, 99),
                        Name = p.Person.FullName,
                        Email = p.Person.Email,
                        Password = p.Random.Word(),
                        RoleId = p.Random.Int(1, 2)
                    }
                });
        }

        public static Faker<CeciDapper.Domain.Entities.RefreshToken> RefreshTokenExpiredEntity()
        {
            return new Faker<CeciDapper.Domain.Entities.RefreshToken>()
                .CustomInstantiator(p => new CeciDapper.Domain.Entities.RefreshToken
                {
                    Token = p.Random.String2(100),
                    Expires = DateTime.UtcNow.AddDays(-1),
                    RegistrationDate = DateTime.UtcNow,
                    CreatedByIp = "127.0.0.1",
                    UserId = p.Random.Int()
                });
        }
    }
}