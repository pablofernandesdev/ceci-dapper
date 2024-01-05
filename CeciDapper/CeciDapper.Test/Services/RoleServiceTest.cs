using AutoMapper;
using CeciDapper.Domain.DTO.Role;
using CeciDapper.Domain.Entities;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Domain.Mapping;
using CeciDapper.Service.Services;
using CeciDapper.Test.Fakers.Role;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CeciDapper.Test.Services
{
    public class RoleServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<RoleService>> _mockLogger;

        public RoleServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<RoleService>>();

            //Auto mapper configuration
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Add_role_successfully()
        {
            //Arrange
            var roleEntityFaker = RoleFaker.RoleEntity().Generate();
            var userAddDTO = _mapper.Map<RoleAddDTO>(roleEntityFaker);

            _mockUnitOfWork.Setup(x => x.Role.AddAsync(It.IsAny<Role>()))
                .ReturnsAsync(roleEntityFaker);

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.AddAsync(userAddDTO);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Add_role_exception()
        {
            //Arrange
            var roleEntityFaker = RoleFaker.RoleEntity().Generate();
            var roleAddDTO = _mapper.Map<RoleAddDTO>(roleEntityFaker);

            _mockUnitOfWork.Setup(x => x.Role.AddAsync(roleEntityFaker))
                .ReturnsAsync(value: null);

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.AddAsync(roleAddDTO);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Delete_role_successfully()
        {
            //Arrange
            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync($"{nameof(Role.Id)} = {1}"))
                .ReturnsAsync(RoleFaker.RoleEntity().Generate());

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.DeleteAsync(1);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Delete_role_not_found()
        {
            //Arrange
            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync($"{nameof(Role.Id)} = {1}"))
                .ReturnsAsync(value: null);

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.DeleteAsync(1);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.BadRequest));
        }

        [Fact]
        public async Task Delete_role_exception()
        {
            //Arrange
            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync($"{nameof(Role.Id)} = {1}"))
                .ThrowsAsync(new Exception());

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.DeleteAsync(1);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Update_role_successfully()
        {
            //Arrange
            var roleUpdateDTOFaker = RoleFaker.RoleUpdateDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync($"{nameof(Role.Id)} = {roleUpdateDTOFaker.RoleId}"))
                .ReturnsAsync(_mapper.Map<Role>(roleUpdateDTOFaker));

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.UpdateAsync(roleUpdateDTOFaker);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Update_role_not_found()
        {
            //Arrange
            var roleUpdateDTOFaker = RoleFaker.RoleUpdateDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync($"{nameof(Role.Id)} = {roleUpdateDTOFaker.RoleId}"))
                .ReturnsAsync(value: null);

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.UpdateAsync(roleUpdateDTOFaker);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.BadRequest));
        }

        [Fact]
        public async Task Update_role_exception()
        {
            //Arrange
            var roleUpdateDTOFaker = RoleFaker.RoleUpdateDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync($"{nameof(Role.Id)} = {roleUpdateDTOFaker.RoleId}"))
                .ThrowsAsync(new Exception());

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.UpdateAsync(roleUpdateDTOFaker);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Get_role_by_id()
        {
            //Arrange
            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync($"{nameof(Role.Id)} = {1}"))
                .ReturnsAsync(RoleFaker.RoleEntity().Generate());

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.GetByIdAsync(1);

            //Assert
            Assert.NotNull(result.Data);
        }

        [Fact]
        public async Task Get_role_by_id_exception()
        {
            //Arrange
            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync($"{nameof(Role.Id)} = {1}"))
                .ThrowsAsync(new Exception());

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.GetByIdAsync(1);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Get_all_users()
        {
            //Arrange
            _mockUnitOfWork.Setup(x => x.Role.GetAllAsync())
                .ReturnsAsync(RoleFaker.RoleEntity().Generate(2));

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.GetAsync();

            //Assert
            Assert.True(result.Data.Any() && result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Get_all_users_exception()
        {
            //Arrange
            _mockUnitOfWork.Setup(x => x.Role.GetAllAsync())
                .ThrowsAsync(new Exception());

            var roleService = RoleServiceConstrutor();

            //Act
            var result = await roleService.GetAsync();

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        private RoleService RoleServiceConstrutor()
        {
            return new RoleService(_mockUnitOfWork.Object,
                _mapper,
                _mockLogger.Object);
        }
    }
}
