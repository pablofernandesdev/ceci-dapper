using CeciDapper.Domain.Entities;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Domain.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CeciDapper.Domain.DTO.Commons;
using AutoMapper;
using CeciDapper.Domain.DTO.User;
using CeciDapper.Infra.CrossCutting.Extensions;
using CeciDapper.Domain.DTO.Email;
using Hangfire;

namespace CeciDapper.Service.Services
{
    /// <summary>
    /// Service for managing users.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IBackgroundJobClient _jobClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="uow">The unit of work for database operations.</param>
        /// <param name="mapper">The object-to-object mapper.</param>
        /// <param name="emailService">The email service for sending emails.</param>
        /// <param name="jobClient">The Hangfire background job client.</param>
        public UserService(IUnitOfWork uow,
            IMapper mapper,
            IEmailService emailService,
            IBackgroundJobClient jobClient)
        {
            _uow = uow;
            _mapper = mapper;
            _emailService = emailService;
            _jobClient = jobClient;
        }

        /// <summary>
        /// Retrieves a list of users based on the specified filter.
        /// </summary>
        /// <param name="filter">The filter criteria for retrieving users.</param>
        /// <returns>A task that represents the asynchronous operation and contains a <see cref="ResultDataResponse{T}"/> object containing the retrieved users.</returns>
        public async Task<ResultDataResponse<IEnumerable<UserResultDTO>>> GetAsync(UserFilterDTO filter)
        {
            var response = new ResultDataResponse<IEnumerable<UserResultDTO>>();

            try
            {
                response.Data = _mapper.Map<IEnumerable<UserResultDTO>>(await _uow.User.GetByFilterAsync(filter));
                response.TotalItems = await _uow.User.GetTotalByFilterAsync(filter);
                response.TotalPages = (int)Math.Ceiling((double)response.TotalItems / filter.PerPage);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Adds a new user to the system.
        /// </summary>
        /// <param name="obj">The user data to be added.</param>
        /// <returns>A task that represents the asynchronous operation and contains a <see cref="ResultResponse"/> object indicating the success or failure of the operation.</returns>
        public async Task<ResultResponse> AddAsync(UserAddDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                obj.Password = PasswordExtension.EncryptPassword(obj.Password);
                var user = await _uow.User.AddAsync(_mapper.Map<User>(obj));

                if (user is null)
                {
                    response.Message = "Could not add user.";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return response;
                }

                response.Message = "User successfully added.";

                _jobClient.Enqueue(() => _emailService.SendEmailAsync(new EmailRequestDTO
                {
                    Body = "User successfully added.",
                    Subject = obj.Name,
                    ToEmail = obj.Email
                }));
            }
            catch (Exception ex)
            {
                response.Message = "Could not add user.";
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Deletes a user from the system.
        /// </summary>
        /// <param name="obj">The user data to be deleted.</param>
        /// <returns>A task that represents the asynchronous operation and contains a <see cref="ResultResponse"/> object indicating the success or failure of the operation.</returns>
        public async Task<ResultResponse> DeleteAsync(UserDeleteDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var user = await _uow.User.GetFirstOrDefaultAsync($"{nameof(User.Id)} = {obj.UserId}");

                if (user != null)
                {
                    await _uow.User.DeleteAsync(user.Id);
                }

                response.Message = "User successfully deleted.";
            }
            catch (Exception ex)
            {
                response.Message = "Could not deleted user.";
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Updates an existing user in the system.
        /// </summary>
        /// <param name="obj">The updated user data.</param>
        /// <returns>A task that represents the asynchronous operation and contains a <see cref="ResultResponse"/> object indicating the success or failure of the operation.</returns>
        public async Task<ResultResponse> UpdateAsync(UserUpdateDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var emailRegistered = await _uow.User
                    .GetFirstOrDefaultAsync($"{nameof(User.Email)} = '{obj.Email}'" +
                    $" AND {nameof(User.Id)} != {obj.UserId}");

                if (emailRegistered != null)
                {
                    return new ResultResponse
                    {
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        Message = "E-mail already registered"
                    };
                }

                var user = await _uow.User.GetFirstOrDefaultAsync($"{nameof(User.Id)} = {obj.UserId}");

                user = _mapper.Map(obj, user);

                await _uow.User.UpdateAsync(user);

                response.Message = "User successfully updated.";
            }
            catch (Exception ex)
            {
                response.Message = "Could not updated user.";
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Updates the role of a user in the system.
        /// </summary>
        /// <param name="obj">The user data containing the updated role information.</param>
        /// <returns>A task that represents the asynchronous operation and contains a <see cref="ResultResponse"/> object indicating the success or failure of the operation.</returns>
        public async Task<ResultResponse> UpdateRoleAsync(UserUpdateRoleDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var user = await _uow.User.GetFirstOrDefaultAsync($"{nameof(User.Id)} = {obj.UserId}");

                user = _mapper.Map(obj, user);

                await _uow.User.UpdateAsync(user);

                response.Message = "User role updated successfully.";
            }
            catch (Exception ex)
            {
                response.Message = "Could not updated user role.";
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Retrieves a user by its ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation and contains a <see cref="ResultResponse{T}"/> object containing the retrieved user.</returns>
        public async Task<ResultResponse<UserResultDTO>> GetByIdAsync(int id)
        {
            var response = new ResultResponse<UserResultDTO>();

            try
            {
                response.Data = _mapper.Map<UserResultDTO>(await _uow.User.GetUserByIdAsync(id));
            }
            catch (Exception ex)
            {
                response.Message = "It was not possible to search the user.";
                response.Exception = ex;
            }

            return response;
        }
    }
}
