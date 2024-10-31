
namespace DomainLayer.Entities
{
    #region Entities

    // User entity representing system users
    public class User : BaseEntity
    {
        public string Username { get; set; } = "";
        public string Email { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public int? GroupId { get; set; }
        public int? RoleId { get; set; }
        public int? DepartmentId { get; set; }

        // Navigation properties
        public Department Department { get; set; } = null;
        public Groups Group { get; set; } = null;
        public Role Role { get; set; } = null;
    }

    // Role entity representing user roles
    public class Role : BaseEntity
    {
        public string RoleName { get; set; } = "";
        public ICollection<RolePermission> RolePermissions { get; set; } = null;
    }

    // Groups entity representing user groups
    public class Groups : BaseEntity
    {
        public string GroupName { get; set; } = "";
        public ICollection<GroupPermission> GroupPermissions { get; set; } = null;
        public ICollection<GroupDepartment> GroupDepartments { get; set; } = null;
        public ICollection<PagePermission> PagePermissions { get; set; } = null;
    }

    // Department entity representing departments
    public class Department : BaseEntity
    {
        public string DepartmentName { get; set; } = "";
        public ICollection<User> Users { get; set; } = null;
        public ICollection<GroupDepartment> GroupDepartments { get; set; } = null;
    }

    // GroupDepartment entity representing many-to-many relationship between Group and Department
    public class GroupDepartment : BaseEntity
    {
        public int DepartmentId { get; set; }
        public Groups Group { get; set; } = null;
        public Department Department { get; set; } = null;
    }

    // Permission entity representing permissions
    public class Permission : BaseEntity
    {
        public string PermissionName { get; set; } = "";
        public ICollection<RolePermission> RolePermissions { get; set; } = null;
        public ICollection<GroupPermission> GroupPermissions { get; set; } = null;
        public ICollection<PagePermission> PagePermissions { get; set; } = null;
    }

    // Page entity representing web pages
    public class Page : BaseEntity
    {
        public int? ParentPageId { get; set; }
        public int? PagePosition { get; set; }
        public string PageName { get; set; } = "";
        public string PageURL { get; set; } = "";
        public string? IconCss { get; set; } = "";
        public string? Attr_CSS { get; set; } = "";
        public string? Attr_CSS1 { get; set; } = "";
        public string? Attr_CSS2 { get; set; } = "";
        public string? Attr_CSS3 { get; set; } = "";
        public string? Attr_CSS4 { get; set; } = "";
        public Page ParentPage { get; set; }
        public ICollection<Page> ChildPages { get; set; } = null;
        public ICollection<PagePermission> PagePermissions { get; set; } = null;
        public Page()
        {
            // Initialize collections in the constructor
            ChildPages = new List<Page>();
            PagePermissions = new List<PagePermission>();
        }
    }

    // RolePermission entity representing many-to-many relationship between Role and Permission
    public class RolePermission : BaseEntity
    {
        public int RoleId { get; set; }
        public int PermissionId { get; set; }

        // Navigation properties
        public Role Role { get; set; } = null;
        public Permission Permission { get; set; } = null;
    }

    // GroupPermission entity representing many-to-many relationship between Group and Permission
    public class GroupPermission : BaseEntity
    {
        public int PermissionId { get; set; }

        // Navigation properties
        public Groups Group { get; set; } = null;
        public Permission Permission { get; set; } = null;
    }

    // PagePermission entity representing many-to-many relationship between Page and Permission
    public class PagePermission : BaseEntity
    {
        public int PageId { get; set; }
        public int GroupID { get; set; }
        public int PermissionId { get; set; }
        public string? PermissionName { get; set; }

        public Page Page { get; set; } = null;
        public Groups Groups { get; set; } = null;
        public Permission Permission { get; set; } = null;
    }

    #endregion
    public class PagePermissionsRights
    {
        public int PageId { get; set; }
        public string PageName { get; set; }
        public string PageUrl { get; set; }
        public int? PageParentId { get; set; } // Nullable to accommodate root pages without parents
        public List<string> Permissions { get; set; }
    }

    public class PagePermissionsObj
    {
        public int PageId { get; set; }
        public string PageName { get; set; }
        public string PageUrl { get; set; }
        public string? PermissionName { get; set; }
        public int PermissionId { get; set; }
    }
}
