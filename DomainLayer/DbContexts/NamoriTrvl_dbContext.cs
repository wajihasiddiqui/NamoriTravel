using Microsoft.EntityFrameworkCore;
using DomainLayer.Entities;

namespace DomainLayer.DbContexts
{
    public sealed class NamoriTrvl_dbContext : DbContext
    {
        #region DbSets

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<GroupDepartment> GroupDepartments { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<GroupPermission> GroupPermissions { get; set; }
        public DbSet<PagePermission> PagePermissions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<XmlRequest> XmlRequests { get; set; }
        public DbSet<DotwRequest> DotwRequests { get; set; }
        public DbSet<Country>  Country { get; set; }
        public DbSet<City>  City { get; set; }
        public DbSet<RateBasis> RateBases{ get; set; }
        public DbSet<Amenities> Amenities  { get; set; }
        public DbSet<Currency>  Currency { get; set; }
        public DbSet<Busines>  Businesses { get; set; }

       
        #endregion

        #region Constructor

        public NamoriTrvl_dbContext(DbContextOptions<NamoriTrvl_dbContext> options) : base(options) { }

        #endregion

        #region OnModelCreating

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Role>().Property(r => r.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Groups>().Property(g => g.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Page>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Department>().Property(d => d.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Permission>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<RolePermission>().Property(rp => rp.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<GroupPermission>().Property(gp => gp.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<PagePermission>().Property(pp => pp.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<AuditLog>().Property(fd => fd.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<ErrorLog>().Property(fd => fd.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<XmlRequest>().Property(fd => fd.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<DotwRequest>().Property(fd => fd.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<City>().Property(fd => fd.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<RateBasis>().Property(fd => fd.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Amenities>().Property(fd => fd.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Busines>().Property(fd => fd.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Currency>().Property(fd => fd.Id).ValueGeneratedOnAdd();
            
            // Configure relationships as you have already done

            // Example:
            modelBuilder.Entity<User>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId);

            // Configure other relationships similarly...

            // Configure composite keys if any
            modelBuilder.Entity<GroupDepartment>()
                .HasKey(gd => new { gd.Id, gd.DepartmentId });

            modelBuilder.Entity<GroupDepartment>()
                .HasOne(gd => gd.Group)
                .WithMany(g => g.GroupDepartments)
                .HasForeignKey(gd => gd.Id);

            modelBuilder.Entity<GroupDepartment>()
                .HasOne(gd => gd.Department)
                .WithMany(d => d.GroupDepartments)
                .HasForeignKey(gd => gd.DepartmentId);

            modelBuilder.Entity<PagePermission>()
                 .HasOne(pp => pp.Page)
                 .WithMany(p => p.PagePermissions)
                 .HasForeignKey(pp => pp.PageId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PagePermission>()
                .HasOne(pp => pp.Groups)
                .WithMany(g => g.PagePermissions)
                .HasForeignKey(pp => pp.GroupID)
                .OnDelete(DeleteBehavior.Cascade);



        }

        #endregion
    }
}
