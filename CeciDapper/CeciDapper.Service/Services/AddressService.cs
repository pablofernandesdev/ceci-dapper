using AutoMapper;
using CeciDapper.Domain.DTO.Address;
using CeciDapper.Domain.DTO.Commons;
using CeciDapper.Domain.Entities;
using CeciDapper.Domain.Interfaces.Repository;
using CeciDapper.Domain.Interfaces.Service;
using CeciDapper.Domain.Interfaces.Service.External;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CeciDapper.Service.Services
{
    /// <summary>
    /// Service class for managing addresses.
    /// </summary>
    public class AddressService : IAddressService
    {
        private readonly IViaCepService _viaCepService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddressService"/> class.
        /// </summary>
        /// <param name="uow">The unit of work.</param>
        /// <param name="viaCepService">The ViaCep service.</param>
        /// <param name="mapper">The AutoMapper instance.</param>
        public AddressService(IUnitOfWork uow, IViaCepService viaCepService, IMapper mapper)
        {
            _uow = uow;
            _viaCepService = viaCepService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves an address by zip code asynchronously.
        /// </summary>
        /// <param name="obj">The address zip code DTO.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the address response.</returns>
        public async Task<ResultResponse<AddressResultDTO>> GetAddressByZipCodeAsync(AddressZipCodeDTO obj)
        {
            var response = new ResultResponse<AddressResultDTO>();

            try
            {
                var addressRequest = await _viaCepService.GetAddressByZipCodeAsync(obj.ZipCode);

                response.StatusCode = addressRequest.StatusCode;

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    response.Message = "Unable to get address. Check that the zip code was entered correctly.";
                    return response;
                }

                response.Data = _mapper.Map<AddressResultDTO>(addressRequest.Data);
            }
            catch (Exception ex)
            {
                response.Message = "Could not get address.";
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Adds an address asynchronously.
        /// </summary>
        /// <param name="obj">The address add DTO.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the add operation response.</returns>
        public async Task<ResultResponse> AddAsync(AddressAddDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var address = await _uow.Address.AddAsync(_mapper.Map<Address>(obj));

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
        /// Updates an address asynchronously.
        /// </summary>
        /// <param name="obj">The address update DTO.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the update operation response.</returns>
        public async Task<ResultResponse> UpdateAsync(AddressUpdateDTO obj)
        {
            var response = new ResultResponse();

            try
            {
                var address = await _uow.Address.GetFirstOrDefaultAsync($"{nameof(Address.Id)} = {obj.AddressId}");

                address = _mapper.Map(obj, address);

                var updateAddress = await _uow.Address.UpdateAsync(address);

                if (!updateAddress)
                {
                    response.Message = "Could not updated address.";
                    response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    return response;
                }

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
        /// Deletes an address asynchronously.
        /// </summary>
        /// <param name="id">The address ID.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the delete operation response.</returns>
        public async Task<ResultResponse> DeleteAsync(int id)
        {
            var response = new ResultResponse();

            try
            {
                var address = await _uow.Address.GetFirstOrDefaultAsync($"{nameof(Address.Id)} = {id}");

                if (address is null)
                {
                    response.Message = "Address not found.";
                    response.StatusCode = HttpStatusCode.NotFound;
                    return response;
                }

                await _uow.Address.DeleteAsync(id);

                response.Message = "Address successfully deleted.";

            }
            catch (Exception ex)
            {
                response.Message = "Could not deleted address.";
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Retrieves a list of addresses asynchronously based on filter criteria.
        /// </summary>
        /// <param name="filter">The address filter DTO.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the address list response.</returns>
        public async Task<ResultDataResponse<IEnumerable<AddressResultDTO>>> GetAsync(AddressFilterDTO filter)
        {
            var response = new ResultDataResponse<IEnumerable<AddressResultDTO>>();

            try
            {
                response.Data = _mapper.Map<IEnumerable<AddressResultDTO>>(await _uow.Address.GetByFilterAsync(filter));
                response.TotalItems = await _uow.Address.GetTotalByFilterAsync(filter);
                response.TotalPages = (int)Math.Ceiling((double)response.TotalItems / filter.PerPage);
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }

            return response;
        }

        /// <summary>
        /// Retrieves an address by ID asynchronously.
        /// </summary>
        /// <param name="id">The address ID.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the address response.</returns>
        public async Task<ResultResponse<AddressResultDTO>> GetByIdAsync(int id)
        {
            var response = new ResultResponse<AddressResultDTO>();

            try
            {
                response.Data = _mapper.Map<AddressResultDTO>(await _uow.Address.GetFirstOrDefaultAsync($"{nameof(Address.Id)} = {id}"));

                if (response.Data is null)
                {
                    response.Message = "Address not found.";
                    response.StatusCode = HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                response.Exception = ex;
            }

            return response;
        }
    }
}
