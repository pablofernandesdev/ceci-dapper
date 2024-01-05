using AutoMapper;
using CeciDapper.Domain.DTO.User;
using CeciDapper.Domain.Entities;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Domain.Interfaces.Service;
using CeciDapper.Domain.Mapping;
using CeciDapper.Infra.CrossCutting.Extensions;
using CeciDapper.Service.Services;
using CeciDapper.Test.Fakers.Auth;
using CeciDapper.Test.Fakers.RefreshToken;
using CeciDapper.Test.Fakers.User;
using Hangfire;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CeciDapper.Test.Services
{
    public class AuthServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly IMapper _mapper;
        private readonly Mock<IBackgroundJobClient> _mockBackgroundJobClient;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<ILogger<AuthService>> _mockLogger;

        public AuthServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockTokenService = new Mock<ITokenService>();
            _mockBackgroundJobClient = new Mock<IBackgroundJobClient>();
            _mockEmailService = new Mock<IEmailService>();
            _mockLogger = new Mock<ILogger<AuthService>>();

            //Auto mapper configuration
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Authenticate_successfully()
        {
            //Arrange
            var userEntityFaker = UserFaker.UserEntity().Generate();
            var loginDTOFaker = AuthFaker.LoginDTO().Generate();
            var userValidFaker = new CeciDapper.Domain.Entities.User
            {
                Id = userEntityFaker.Id,
                Name = userEntityFaker.Name,
                Email = userEntityFaker.Email,
                Password = userEntityFaker.Password,
                RoleId = userEntityFaker.RoleId,
                Role = userEntityFaker.Role
            };
            var refreshTokenFaker = RefreshTokenFaker.RefreshTokenEntity().Generate();
            //var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

            userEntityFaker.Password = PasswordExtension.EncryptPassword(loginDTOFaker.Password);

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync($"{nameof(User.Email)} = '{loginDTOFaker.Username}'"))
                .ReturnsAsync(userEntityFaker);

            _mockUnitOfWork.Setup(x => x.User.GetUserByIdAsync(userEntityFaker.Id))
                .ReturnsAsync(userValidFaker);

            //validar porque o valor esta sendo retornado nulo
            _mockTokenService.Setup(x => x.GenerateToken(_mapper.Map<UserResultDTO>(userValidFaker)))
                .Returns(It.IsAny<string>());

            _mockUnitOfWork.Setup(x => x.RefreshToken.AddAsync(refreshTokenFaker))
                .ReturnsAsync(refreshTokenFaker);

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.AuthenticateAsync(loginDTOFaker, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Authenticate_unauthorized_password_incorret()
        {
            //Arrange
            var userEntityFaker = UserFaker.UserEntity().Generate();
            var loginDTOFaker = AuthFaker.LoginDTO().Generate();

            userEntityFaker.Password = PasswordExtension.EncryptPassword("bm92b3Rlc3Rl");

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync($"{nameof(User.Email)} = '{loginDTOFaker.Username}'"))
                .ReturnsAsync(userEntityFaker);

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.AuthenticateAsync(loginDTOFaker, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.Unauthorized));
        }

        [Fact]
        public async Task Authenticate_user_not_found()
        {
            //Arrange
            var loginDTOFaker = AuthFaker.LoginDTO().Generate();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync($"{nameof(User.Email)} = '{loginDTOFaker.Username}'"))
                .ReturnsAsync(value: null);

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.AuthenticateAsync(loginDTOFaker, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.Unauthorized));
        }

        [Fact]
        public async Task Authenticate_exception()
        {
            //Arrange
            var loginDTOFaker = AuthFaker.LoginDTO().Generate();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync($"{nameof(User.Email)} = '{loginDTOFaker.Username}'"))
                .ThrowsAsync(new Exception());

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.AuthenticateAsync(loginDTOFaker, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.InternalServerError));
        }

        [Fact]
        public async Task Refresh_token_successfully()
        {
            //Arrange
            var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            var refreshTokenFaker = RefreshTokenFaker.RefreshTokenEntity().Generate();

            _mockUnitOfWork.Setup(x => x.RefreshToken.GetFirstOrDefaultAsync($"{nameof(RefreshToken.Token)} = '{jwtToken}'"))
                .ReturnsAsync(refreshTokenFaker);

            //validar porque o valor esta sendo retornado nulo
            _mockTokenService.Setup(x => x.GenerateToken(_mapper.Map<UserResultDTO>(refreshTokenFaker.User)))
                .Returns(It.IsAny<string>());

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.RefreshTokenAsync(jwtToken, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Refresh_token_expired()
        {
            //Arrange
            var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            var refreshTokenFaker = RefreshTokenFaker.RefreshTokenExpiredEntity().Generate();

            _mockUnitOfWork.Setup(x => x.RefreshToken.GetFirstOrDefaultAsync($"{nameof(RefreshToken.Token)} = '{jwtToken}'"))
                .ReturnsAsync(refreshTokenFaker);

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.RefreshTokenAsync(jwtToken, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.Unauthorized));
        }

        [Fact]
        public async Task Refresh_token_exception()
        {
            //Arrange
            var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            _mockUnitOfWork.Setup(x => x.RefreshToken.GetFirstOrDefaultAsync($"{nameof(RefreshToken.Token)} = '{jwtToken}'"))
                .ThrowsAsync(new Exception());

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.RefreshTokenAsync(jwtToken, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.InternalServerError));
        }

        [Fact]
        public async Task Revoke_token_successfully()
        {
            //Arrange
            var refreshTokenFaker = RefreshTokenFaker.RefreshTokenEntity().Generate();
            var refreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";

            _mockUnitOfWork.Setup(x => x.RefreshToken.GetFirstOrDefaultAsync($"{nameof(RefreshToken.Token)} = '{refreshToken}'" +
                $" AND {nameof(RefreshToken.IsActive)} = 1"))
                .ReturnsAsync(refreshTokenFaker);

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.RevokeTokenAsync(refreshToken, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Revoke_token_null_token()
        {
            //Arrange
            var refreshTokenFaker = RefreshTokenFaker.RefreshTokenEntity().Generate();
            var refreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";

            _mockUnitOfWork.Setup(x => x.RefreshToken.GetFirstOrDefaultAsync($"{nameof(RefreshToken.Token)} = '{refreshToken}'" +
                $" AND {nameof(RefreshToken.IsActive)} = 1"))
                .ReturnsAsync(value: null);

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.RevokeTokenAsync(refreshToken, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.Unauthorized));
        }

        [Fact]
        public async Task Revoke_token_exception()
        {
            //Arrange
            var refreshTokenFaker = RefreshTokenFaker.RefreshTokenEntity().Generate();
            var refreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";

            _mockUnitOfWork.Setup(x => x.RefreshToken.GetFirstOrDefaultAsync($"{nameof(RefreshToken.Token)} = '{refreshToken}'" +
                $" AND {nameof(RefreshToken.IsActive)} = 1"))
                 .ThrowsAsync(new Exception());

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.RevokeTokenAsync(refreshToken, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.InternalServerError));
        }

        [Fact]
        public async Task Forgot_password_successfully()
        {
            //Arrange
            var userEntityFaker = UserFaker.UserEntity().Generate();
            var forgotPasswordDtoFaker = AuthFaker.ForgotPasswordDTO().Generate();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync($"{nameof(User.Email)} = '{forgotPasswordDtoFaker.Email}'"))
                .ReturnsAsync(userEntityFaker);

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.ForgotPasswordAsync(forgotPasswordDtoFaker);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Forgot_password_exception()
        {
            //Arrange
            var forgotPasswordDtoFaker = AuthFaker.ForgotPasswordDTO().Generate();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync($"{nameof(User.Email)} = '{forgotPasswordDtoFaker.Email}'"))
            .ThrowsAsync(new Exception());

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.ForgotPasswordAsync(forgotPasswordDtoFaker);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.InternalServerError));
        }

        private AuthService AuthServiceConstrutor()
        {
            return new AuthService(_mockTokenService.Object,
                _mockEmailService.Object,
                _mockUnitOfWork.Object,
                _mapper,
                _mockBackgroundJobClient.Object,
                _mockLogger.Object);
        }
    }
}
