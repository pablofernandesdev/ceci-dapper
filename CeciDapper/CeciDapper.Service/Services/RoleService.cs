using AutoMapper;
using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.Role;
using CeciDapper.Domain.Entities;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Domain.Interfaces.Service;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CeciDapper.Service.Services
{
    /// <summary>
    /// Service class for managing roles.
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleService"/> class.
        /// </summary>
        /// <param name="uow">The unit of work.</param>
        /// <param name="mapper">The mapper.</param>
        /// <param name="logger">The logger.</param>
        public RoleService(IUnitOfWork uow,
            IMapper mapper, 
            ILogger<RoleService> logger)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all roles.
        /// </summary>
        /// <returns>A response containing the collection of roles.</returns>
        public async Task<ResultDataResponse<IEnumerable<RoleResultDTO>>> GetAsync()
        {
            var response = new ResultDataResponse<IEnumerable<RoleResultDTO>>();

            try
            {
                response.Data = _mapper.Map<IEnumerable<RoleResultDTO>>(await _uow.Role.GetAllAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving roles");
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Adds a new role.
        /// </summary>
        /// <param name="obj">The role object to add.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        public async Task<ResultResponse> AddAsync(RoleAddDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var role = await _uow.Role.AddAsync(_mapper.Map<Role>(obj));

                if (role is null)
                {
                    response.Message = "Could not add role.";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return response;
                }

                response.Message = "Role successfully added.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding role");
                response.Message = "Error adding role.";
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Deletes a role by ID.
        /// </summary>
        /// <param name="id">The ID of the role to delete.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        public async Task<ResultResponse> DeleteAsync(int id)
        {
            var response = new ResultResponse();

            try
            {
                var role = await _uow.Role.GetFirstOrDefaultAsync($"{nameof(Role.Id)} = {id}");

                if (role is null)
                {
                    response.Message = "Role not found";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return response;
                }

                await _uow.Role.DeleteAsync(id);

                response.Message = "Role successfully delete.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting role");
                response.Message = "Error deleting role.";
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Updates a role.
        /// </summary>
        /// <param name="obj">The updated role object.</param>
        /// <returns>A response indicating the result of the operation.</returns>
        public async Task<ResultResponse> UpdateAsync(RoleUpdateDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var role = await _uow.Role.GetFirstOrDefaultAsync($"{nameof(Role.Id)} = {obj.RoleId}");

                if (role is null)
                {
                    response.Message = "Role not found";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return response;
                }

                role = _mapper.Map(obj, role);

                await _uow.Role.UpdateAsync(role);

                response.Message = "Role successfully update.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not update role.");
                response.Message = "Could not update role.";
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Retrieves a role by ID.
        /// </summary>
        /// <param name="id">The ID of the role to retrieve.</param>
        /// <returns>A response containing the role.</returns>
        public async Task<ResultResponse<RoleResultDTO>> GetByIdAsync(int id)
        {
            var response = new ResultResponse<RoleResultDTO>();

            try
            {
                var role = await _uow.Role.GetFirstOrDefaultAsync($"{nameof(Role.Id)} = {id}");

                if (role is null)
                {
                    response.Message = "Função não encontrada.";
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                response.Data = _mapper.Map<RoleResultDTO>(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving role");
                response.Message = "It was not possible to search the role.";
                response.Exception = ex;
            }

            return response;
        }
    }
}
