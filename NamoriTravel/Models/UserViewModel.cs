using ModelsDTO;
namespace NamoriTravel.Models
{
    // UserViewModel.cs
    public class UserViewModel : BaseEntityModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public int? GroupId { get; set; }
        public int? RoleId { get; set; }
        public int DepartmentId { get; set; }
    }

    // RoleViewModel.cs
    public class RoleViewModel: BaseEntityModel
    {
        public string RoleName { get; set; }
    }

    // GroupViewModel.cs
    public class GroupViewModel: BaseEntityModel
    {
        public string GroupName { get; set; }
    }

    // DepartmentViewModel.cs
    public class DepartmentViewModel: BaseEntityModel
    {
        public string DepartmentName { get; set; }
    }

    // PermissionViewModel.cs
    public class PermissionViewModel: BaseEntityModel
    {
        public string PermissionName { get; set; }
        public bool IsAssigned { get; set; }
    }

    // PageViewModel.cs
    public class PageViewModel: BaseEntityModel
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

    public class PagePermissionViewModel : BaseEntityModel
    {
        public int PageId { get; set; }
        public int GroupID { get; set; }
        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }
        public PageViewModel Page { get; set; }
        public PermissionViewModel Permission { get; set; }
    }
    public class PagePermissionsObjViewModel
    {
        public int PageId { get; set; }
        public string PageName { get; set; }
        public string PageUrl { get; set; }
        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }

    }

    public class UpdatePagePermissionsRequest
    {
        public int GroupId { get; set; }
        public List<PagePermissionDTO> Permissions { get; set; }
    }
}
