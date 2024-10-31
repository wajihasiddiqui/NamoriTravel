using System.Text.RegularExpressions;
using System.Reflection.Metadata;
using DomainLayer.Entities;
using AutoMapper;
using NamoriTravel.Models;
using ModelsDTO;

namespace NamoriTravel.MappingProfile
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            #region //----------* Entity to DTO *--------------//
            CreateMap<Page, PageDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Groups, GroupDTO>().ReverseMap();
            CreateMap<Department, DepartmentDTO>().ReverseMap();
            CreateMap<Permission, PermissionDTO>().ReverseMap();
            CreateMap<FieldDefinition, FieldDefinitionDTO>().ReverseMap();
            CreateMap<PagePermission, PagePermissionDTO>().ReverseMap();
            CreateMap<PagePermissionsObj, PagePermissionsObjDTO>().ReverseMap();
            CreateMap<DotwRequest, DotwRequestDTO>().ReverseMap();
            CreateMap<XmlRequest, XmlRequestDTO>().ReverseMap();
            CreateMap<Country, CountryDTO>().ReverseMap();
            CreateMap<City, CityDTO>().ReverseMap();
            CreateMap<DomainLayer.Entities.Amenities, AmenitiesDTO>().ReverseMap();
            CreateMap<RateBasis, RateBasisDTO>().ReverseMap();
            CreateMap<Currency, CurrencyDTO>().ReverseMap();
            CreateMap<DomainLayer.Entities.Busines, BusinessDTO>().ReverseMap();

            #endregion

            #region//----------* DTO to ENTITY *--------------//
            CreateMap<PageDTO, Page>().ReverseMap();
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<RoleDTO, Role>().ReverseMap();
            CreateMap<GroupDTO, Group>().ReverseMap();
            CreateMap<DepartmentDTO, Department>().ReverseMap();
            CreateMap<PermissionDTO, Permission>().ReverseMap();
            CreateMap<FieldDefinitionDTO, FieldDefinition>().ReverseMap();
            CreateMap<PagePermissionDTO, PagePermission>().ReverseMap();
            CreateMap<PagePermissionsObjDTO, PagePermissionsObj>().ReverseMap();
            CreateMap<DotwRequestDTO, DotwRequest>().ReverseMap();
            CreateMap<XmlRequestDTO, XmlRequest>().ReverseMap();
            CreateMap<CountryDTO, Country>().ReverseMap();
            CreateMap<CityDTO, City>().ReverseMap();
            CreateMap<AmenitiesDTO, DomainLayer.Entities.Amenities>().ReverseMap();
            CreateMap<RateBasisDTO, RateBasis>().ReverseMap();
            CreateMap<CurrencyDTO, Currency>().ReverseMap();
            CreateMap<BusinessDTO, Busines>().ReverseMap();
            #endregion

            #region//----------* Model to DTO *--------------//
            CreateMap<PageViewModel, PageDTO>().ReverseMap();
            CreateMap<UserViewModel, UserDTO>().ReverseMap();
            CreateMap<RoleViewModel, RoleDTO>().ReverseMap();
            CreateMap<GroupViewModel, GroupDTO>().ReverseMap();
            CreateMap<DepartmentViewModel, DepartmentDTO>().ReverseMap();
            CreateMap<PermissionViewModel, PermissionDTO>().ReverseMap();
            CreateMap<PagePermissionViewModel, PagePermissionDTO>().ReverseMap();
            CreateMap<PagePermissionsObjViewModel, PagePermissionsObjDTO>().ReverseMap();
            #endregion

            #region//----------* DTO to Model *--------------//
            CreateMap<PageDTO, PageViewModel>().ReverseMap();
            CreateMap<UserDTO, UserViewModel>().ReverseMap();
            CreateMap<RoleDTO, RoleViewModel>().ReverseMap();
            CreateMap<GroupDTO, GroupViewModel>().ReverseMap();
            CreateMap<DepartmentDTO, DepartmentViewModel>().ReverseMap();
            CreateMap<PermissionDTO, PermissionViewModel>().ReverseMap();
            CreateMap<PagePermissionDTO, PagePermissionViewModel>().ReverseMap();
            CreateMap<PagePermissionsObjDTO, PagePermissionsObjViewModel>().ReverseMap();
            #endregion

        }
    }
}
