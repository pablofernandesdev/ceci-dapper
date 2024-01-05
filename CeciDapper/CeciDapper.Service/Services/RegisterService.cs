using AutoMapper;
using CeciDapper.Domain.DTO.Address;
using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.DTO.Email;
using CeciDapper.Domain.DTO.Register;
using CeciDapper.Domain.DTO.User;
using CeciDapper.Domain.Entities;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Domain.Interfaces.Service;
using CeciDapper.Infra.CrossCutting.Extensions;
using Hangfire;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CeciDapper.Service.Services
{
    /// <summary>
    /// Service for managing registrations and users.
    /// </summary>
    public class RegisterService : IRegisterService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBackgroundJobClient _jobClient;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterService"/> class.
        /// </summary>
        /// <param name="uow">The unit of work for database operations.</param>
        /// <param name="mapper">The AutoMapper instance for object mapping.</param>
        /// <param name="httpContextAccessor">The HttpContextAccessor to access the current HttpContext.</param>
        /// <param name="jobClient">The Hangfire IBackgroundJobClient for background job scheduling.</param>
        /// <param name="emailService">The email service for sending emails.</param>
        public RegisterService(IUnitOfWork uow,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IBackgroundJobClient jobClient,
            IEmailService emailService)
        {
            _uow = uow;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _jobClient = jobClient;
            _emailService = emailService;
        }

        /// <summary>
        /// Get the logged-in user.
        /// </summary>
        /// <returns>Response with the logged-in user.</returns>
        public async Task<ResultResponse<UserResultDTO>> GetLoggedInUserAsync()
        {
            var response = new ResultResponse<UserResultDTO>();

            try
            {
                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                var user = await _uow.User.GetUserByIdAsync(Convert.ToInt32(userId));

                if (user is null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "User not found.";
                    return response;
                }

                response.Data = _mapper.Map<UserResultDTO>(user);
            }
            catch (Exception ex)
            {
                response.Message = "Error retrieving logged-in user.";
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Perform self-registration for a new user.
        /// </summary>
        /// <param name="obj">User registration data.</param>
        /// <returns>Response indicating the result of the registration.</returns>
        public async Task<ResultResponse> SelfRegistrationAsync(UserSelfRegistrationDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var basicProfile = await _uow.Role.GetBasicProfile();

                obj.Password = PasswordExtension.EncryptPassword(obj.Password);

                var newUser = _mapper.Map<User>(obj);
                newUser.RoleId = basicProfile.Id;

                _uow.BeginTransaction();

                await _uow.User.AddAsync(newUser);

                _uow.Commit();

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
        /// Update the logged-in user's data.
        /// </summary>
        /// <param name="obj">Updated user data.</param>
        /// <returns>Response indicating the result of the update.</returns>
        public async Task<ResultResponse> UpdateLoggedUserAsync(UserLoggedUpdateDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                var emailRegistered = await _uow.User
                    .GetFirstOrDefaultAsync($"{nameof(User.Email)} = {obj.Email} AND {nameof(User.Id)} != {Convert.ToInt32(userId)}");

                if (emailRegistered != null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "E-mail already registered";
                    return response;
                }

                var user = await _uow.User.GetFirstOrDefaultAsync($"{nameof(User.Id)} = {Convert.ToInt32(userId)}");

                if (user is null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "User not found.";
                    return response;
                }

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
        /// Redefines the password for the logged-in user.
        /// </summary>
        /// <param name="obj">Object containing the current and new passwords.</param>
        /// <returns>Response indicating the result of the password redefinition.</returns>
        public async Task<ResultResponse> RedefinePasswordAsync(UserRedefinePasswordDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                var user = await _uow.User
                    .GetFirstOrDefaultAsync($"{nameof(User.Id)} = {Convert.ToInt32(userId)}");

                if (user is null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "User not found.";
                    return response;
                }

                if (!PasswordExtension.DecryptPassword(user.Password).Equals(obj.CurrentPassword))
                {
                    response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    response.Message = "Password incorret.";
                    return response;
                };

                user.Password = PasswordExtension.EncryptPassword(obj.NewPassword);

                await _uow.User.UpdateAsync(user);

                response.Message = "Password user successfully updated.";
            }
            catch (Exception ex)
            {
                response.Message = "Could not updated password user.";
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Adds a new address for the logged-in user.
        /// </summary>
        /// <param name="obj">Object containing the address data.</param>
        /// <returns>Response indicating the result of the address addition.</returns>
        public async Task<ResultResponse> AddLoggedUserAddressAsync(AddressLoggedUserAddDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                var addressEntity = _mapper.Map<Address>(obj);
                addressEntity.UserId = Convert.ToInt32(userId);

                var address = await _uow.Address.AddAsync(addressEntity);

                if (address is null)
                {
                    response.Message = "Could not add address.";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return response;
                }

                response.Message = "Address successfully added.";

            }
            catch (Exception ex)
            {
                response.Message = "Could not add address.";
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Updates an address for the logged-in user.
        /// </summary>
        /// <param name="obj">Object containing the updated address data.</param>
        /// <returns>Response indicating the result of the address update.</returns>
        public async Task<ResultResponse> UpdateLoggedUserAddressAsync(AddressLoggedUserUpdateDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                var address = await _uow.Address.GetFirstOrDefaultAsync($"{nameof(Address.UserId)} = {Convert.ToInt32(userId)}" +
                    $" AND {nameof(Address.Id)} = {obj.AddressId}");

                if (address is null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Address not found.";
                    return response;
                }

                address = _mapper.Map(obj, address);

                await _uow.Address.UpdateAsync(address);              

                response.Message = "Address successfully updated.";

            }
            catch (Exception ex)
            {
                response.Message = "Could not updated address.";
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Inactivates an address for the logged-in user.
        /// </summary>
        /// <param name="obj">Object containing the address identifier.</param>
        /// <returns>Response indicating the result of the address deactivation.</returns>
        public async Task<ResultResponse> InactivateLoggedUserAddressAsync(AddressDeleteDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                var address = await _uow.Address.GetFirstOrDefaultAsync($"{nameof(Address.UserId)} = {Convert.ToInt32(userId)}" +
                    $" AND {nameof(Address.Id)} = {obj.AddressId}");

                if (address is null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Address not found.";
                    return response;
                }

                address.Active = false;

                await _uow.Address.UpdateAsync(address);               

                response.Message = "Address successfully deactivated.";

            }
            catch (Exception ex)
            {
                response.Message = "Could not deactivated address.";
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Retrieves the addresses for the logged-in user.
        /// </summary>
        /// <param name="filter">Object containing the filter criteria for the addresses.</param>
        /// <returns>Response containing the addresses and additional information.</returns>
        public async Task<ResultDataResponse<IEnumerable<AddressResultDTO>>> GetLoggedUserAddressesAsync(AddressFilterDTO filter)
        {
            var response = new ResultDataResponse<IEnumerable<AddressResultDTO>>();

            try
            {
                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                response.Data = _mapper.Map<IEnumerable<AddressResultDTO>>(await _uow.Address.GetLoggedUserAddressesAsync(Convert.ToInt32(userId), filter));
                response.TotalItems = await _uow.Address.GetTotalLoggedUserAddressesAsync(Convert.ToInt32(userId), filter);
                response.TotalPages = (int)Math.Ceiling((double)response.TotalItems / filter.PerPage);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Retrieves a specific address for the logged-in user.
        /// </summary>
        /// <param name="obj">Object containing the address identifier.</param>
        /// <returns>Response containing the address data.</returns>
        public async Task<ResultResponse<AddressResultDTO>> GetLoggedUserAddressAsync(AddressIdentifierDTO obj)
        {
            var response = new ResultResponse<AddressResultDTO>();

            try
            {
                var userId = _httpContextAccessor.HttpContext.User.GetLoggedInUserId();

                var address = await _uow.Address.GetFirstOrDefaultAsync($"{nameof(Address.UserId)} = {Convert.ToInt32(userId)}" +
                    $" AND {nameof(Address.Id)} = {obj.AddressId}");

                if (address is null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Address not found.";
                    return response;
                }

                response.Data = _mapper.Map<AddressResultDTO>(address);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }

            return response;
        }
    }
}
