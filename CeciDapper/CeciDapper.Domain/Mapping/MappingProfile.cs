using AutoMapper;
using CeciDapper.Domain.DTO.Address;
using CeciDapper.Domain.DTO.Auth;
using CeciDapper.Domain.DTO.Register;
using CeciDapper.Domain.DTO.Role;
using CeciDapper.Domain.DTO.User;
using CeciDapper.Domain.DTO.ViaCep;
using CeciDapper.Domain.Entities;

namespace CeciDapper.Domain.Mapping
{
    /// <summary>
    /// Represents the AutoMapper profile for mapping between DTOs and entities.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// Configures AutoMapper mappings between various DTOs and entity classes.
        /// </summary>
        public MappingProfile()
        {
            #region User
            CreateMap<User, UserAddDTO>().ReverseMap();

            CreateMap<User, UserSelfRegistrationDTO>().ReverseMap();

            CreateMap<User, UserImportDTO>().ReverseMap();

            CreateMap<User, UserResultDTO>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Email))
                .ReverseMap();

            CreateMap<User, UserUpdateDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<User, UserUpdateRoleDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<User, UserLoggedUpdateDTO>().ReverseMap();
            #endregion

            #region Auth
            CreateMap<User, AuthResultDTO>()
              .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name))
              .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Email))
              .ReverseMap();

            CreateMap<RefreshToken, AuthResultDTO>()
              .ForMember(dest => dest.RefreshToken, opt => opt.MapFrom(src => src.Token))
              .ReverseMap();
            #endregion

            #region Role
            CreateMap<Role, RoleAddDTO>().ReverseMap();

            CreateMap<Role, RoleResultDTO>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();

            CreateMap<Role, RoleUpdateDTO>()
                 .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id))
                 .ReverseMap();

            CreateMap<Role, RoleUpdateDTO>().ReverseMap();
            #endregion

            #region Address
            CreateMap<ViaCepAddressResponseDTO, AddressResultDTO>()
                .ForMember(dest => dest.ZipCode, opt => opt.MapFrom(src => src.Cep))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Logradouro))
                .ForMember(dest => dest.District, opt => opt.MapFrom(src => src.Bairro))
                .ForMember(dest => dest.Locality, opt => opt.MapFrom(src => src.Localidade))
                .ForMember(dest => dest.Complement, opt => opt.MapFrom(src => src.Complemento))
            .ReverseMap();

            CreateMap<Address, AddressLoggedUserAddDTO>().ReverseMap();

            CreateMap<Address, AddressAddDTO>().ReverseMap();

            CreateMap<Address, AddressUpdateDTO>().ReverseMap();

            CreateMap<Address, AddressResultDTO>().ReverseMap();

            CreateMap<Address, AddressLoggedUserUpdateDTO>()
                .ForMember(dest => dest.AddressId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
            #endregion
        }
    }
}
