namespace ModelsDTO
{
    // UserDTO.cs
    public class UserDTO : BaseEntityDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int? GroupId { get; set; }
        public string uRoleName { get; set; }
        public string uGroupName { get; set; }
        public string uDepartmentName { get; set; }
        public int? RoleId { get; set; }
        public int DepartmentId { get; set; }

        public static implicit operator List<object>(UserDTO v)
        {
            throw new NotImplementedException();
        }
    }

    // RoleDTO.cs
    public class RoleDTO : BaseEntityDTO
    {
        public string RoleName { get; set; }
    }

    // GroupDTO.cs
    public class GroupDTO : BaseEntityDTO
    {
        public string GroupName { get; set; }
        public int? SubGroupId { get; set; }
    }

    // DepartmentDTO.cs
    public class DepartmentDTO : BaseEntityDTO
    {
        public string DepartmentName { get; set; }
        public int? SubDepartmentId { get; set; }
    }

    // PermissionDTO.cs
    public class PermissionDTO : BaseEntityDTO
    {
        public string PermissionName { get; set; }
    }

    // PageDTO.cs
    public class PageDTO : BaseEntityDTO
    {
        public string PageName { get; set; }
        public string PageURL { get; set; }
        public int? ParentPageId { get; set; }
        public int? PagePosition { get; set; }
        public string? IconCss { get; set; } = "";
        public string? Attr_CSS { get; set; } = "";
        public string? Attr_CSS1 { get; set; } = "";
        public string? Attr_CSS2 { get; set; } = "";
        public string? Attr_CSS3 { get; set; } = "";
        public string? Attr_CSS4 { get; set; } = "";
    }
    public class PagePermissionDTO : BaseEntityDTO
    {
        public int PageId { get; set; }
        public int GroupID { get; set; }
        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }
        public PageDTO Page { get; set; }
        public GroupDTO Groups { get; set; }
        public PermissionDTO Permission { get; set; }
    }

   public class PagePermissionsObjDTO
    {
        public int PageId { get; set; }
        public string PageName { get; set; }
        public string PageUrl { get; set; }
        public string? PermissionName { get; set; }
        public int PermissionId { get; set; }

    }

}
