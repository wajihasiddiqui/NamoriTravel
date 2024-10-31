
/************************************************************ 
*_Author's     :    Faiz Muhammad Mari 
*_Designation  :    Senior .NET Developer
*_Email        :    faizmuhammadmarri@gmail.com
*_Mobile       :    03032213801 / 03013584100
*_Description  :    Adding Default User Admin User 
*_SP Name      :    [CreateDefaultValuesAndAdmin]   
*_Date         :    6/27/2024 12:39:06 PM  
*************************************************************/   
  
CREATE PROCEDURE   [dbo].[CreateDefaultValuesAndAdmin]  
AS  
BEGIN  
    SET NOCOUNT ON;  
  
    -- Variables for default values  
    -- User: Admin 
    -- Pasword: admin
    DECLARE @UserId INT;  
    DECLARE @DefaultGroupName NVARCHAR(50) = 'DefaultGroup';  
    DECLARE @DefaultDepartmentName NVARCHAR(50) = 'DefaultDepartment';  
    DECLARE @DefaultUsername NVARCHAR(50) = 'Admin';  
    DECLARE @DefaultEmail NVARCHAR(100) = 'admin@namoritravel.com';  
    DECLARE @DefaultPasswordHash NVARCHAR(MAX) = '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918';   
    DECLARE @DefaultRoles TABLE (RoleName NVARCHAR(50));  
      
    -- Insert default roles into a table variable  
    INSERT INTO @DefaultRoles (RoleName)  
    VALUES    
           ('Admin'),  
           ('IT Admin'),   
           ('Employees'),  
           ('Assistants'),  
           ('Backoffice'),  
           ('Receptionists'),  
           ('Support team'),  
           ('Service Provider'),   
           ('General Services'),   
           ('Security');  
  
    -- Permission names  
    DECLARE @PermissionNames TABLE (PermissionName NVARCHAR(50));  
    INSERT INTO @PermissionNames (PermissionName)  
    VALUES ('Visible'), ('Read'), ('Create'), ('Update'), ('Delete');  
  
    
    -- Insert default roles if not exists  
    DECLARE @RoleName NVARCHAR(50);  
  
    DECLARE RoleCursor CURSOR FOR  
    SELECT RoleName FROM @DefaultRoles;  
  
    OPEN RoleCursor;  
    FETCH NEXT FROM RoleCursor INTO @RoleName;  
  
    WHILE @@FETCH_STATUS = 0  
    BEGIN  
        IF NOT EXISTS (SELECT 1 FROM Roles WHERE RoleName = @RoleName)  
        BEGIN  
            INSERT INTO Roles (RoleName, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, IsActive, IsDeleted)  
            VALUES   (@RoleName, @UserId, GETDATE(), @UserId, GETDATE(), 1, 0);  
        END  
        FETCH NEXT FROM RoleCursor INTO @RoleName;  
    END  
  
    CLOSE RoleCursor;  
    DEALLOCATE RoleCursor;  
  
    -- Insert default permissions if not exists  
    DECLARE @PermissionName NVARCHAR(50);  
    DECLARE @PermissionId INT;  
  
    DECLARE PermissionCursor CURSOR FOR  
    SELECT PermissionName FROM @PermissionNames;  
  
    OPEN PermissionCursor;  
    FETCH NEXT FROM PermissionCursor INTO @PermissionName;  
  
    WHILE @@FETCH_STATUS = 0  
    BEGIN  
        IF NOT EXISTS (SELECT 1 FROM Permissions WHERE PermissionName = @PermissionName)  
        BEGIN  
            INSERT INTO Permissions (PermissionName, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, IsActive, IsDeleted)  
            VALUES (@PermissionName, @UserId, GETDATE(), @UserId, GETDATE(), 1, 0);  
        END  
        FETCH NEXT FROM PermissionCursor INTO @PermissionName;  
    END  
  
    CLOSE PermissionCursor;  
    DEALLOCATE PermissionCursor;  
  
    -- Insert default page if not exists  
    IF NOT EXISTS (SELECT 1 FROM Pages WHERE PageName IN('Dashboard','Page','User','Role','AccessManagement','Group','Department','Permissions') AND 
											PageURL  IN('/Dashboard','/Page','/User','/Role','/AccessManagement','/Group','/Department','/Permissions') ) 
    BEGIN  
        INSERT INTO Pages (PageName, PageURL, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, IsActive, IsDeleted,ParentPageId,PagePosition)  
        VALUES ('Dashboard', '/dashboard', @UserId, GETDATE(), @UserId, GETDATE(), 1, 0,null,1),
			   ('Setting', '', 1, GETDATE(), 1, GETDATE(), 1, 0,1,20),
               ('AccessManagement', '/AccessManagement', @UserId, GETDATE(), @UserId, GETDATE(), 1, 0,2,4),  
			   ('User', '/User', @UserId, GETDATE(), @UserId, GETDATE(), 1, 0,1,1),
			   ('Group', '/Group', @UserId, GETDATE(), @UserId, GETDATE(), 1, 0,1,2),
			   ('Role', '/Role', @UserId, GETDATE(), @UserId, GETDATE(), 1, 0,2,1),
			   ('Page', '/Page', @UserId, GETDATE(), @UserId, GETDATE(), 1, 0,2,3),
			   ('Department', '/Department', @UserId, GETDATE(), @UserId, GETDATE(), 1, 0,1,3),
			   ('Permissions', '/Permissions', @UserId, GETDATE(), @UserId, GETDATE(), 1, 0,2,1);
    END  
	
    -- Insert default group if not exists  
    IF NOT EXISTS (SELECT 1 FROM Groups WHERE GroupName = @DefaultGroupName)  
    BEGIN  
        INSERT INTO Groups (GroupName, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, IsActive, IsDeleted)  
        VALUES (@DefaultGroupName, @UserId, GETDATE(), @UserId, GETDATE(), 1, 0);
    END  
  
    -- Insert default department if not exists  
    IF NOT EXISTS (SELECT 1 FROM Departments WHERE DepartmentName = @DefaultDepartmentName)  
    BEGIN  
        INSERT INTO Departments (DepartmentName, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, IsActive, IsDeleted)  
        VALUES (@DefaultDepartmentName, @UserId, GETDATE(), @UserId, GETDATE(), 1, 0);  
    END  
  
    -- Retrieve IDs of default values  
    DECLARE @AdminRoleId INT = (SELECT Id FROM Roles WHERE RoleName = 'Admin');  
    DECLARE @GroupId INT = (SELECT Id FROM Groups WHERE GroupName = @DefaultGroupName);  
    DECLARE @DepartmentId INT = (SELECT Id FROM Departments WHERE DepartmentName = @DefaultDepartmentName);  
    DECLARE @PageId INT ;--= (SELECT Id FROM Pages WHERE PageName IN('Dashboard','Page','User','Role','AccessManagement') AND PageURL IN('Dashboard','/Page','/User','/Role','/AccessManagement'));  
   -- Insert default user if not exists  
    IF NOT EXISTS (SELECT 1 FROM Users WHERE Username = @DefaultUsername)  
    BEGIN  
        INSERT INTO Users (Username, Email, PasswordHash,DepartmentId,GroupId,RoleId,  CreatedDate,  IsActive, IsDeleted)  
        VALUES (@DefaultUsername, @DefaultEmail, @DefaultPasswordHash,@DepartmentId,@GroupId,@AdminRoleId, GETDATE(),  1, 0);  
  SET @UserId = SCOPE_IDENTITY();  
    END  
    -- Insert into GroupDepartment if not exists  
    IF NOT EXISTS (SELECT 1 FROM GroupDepartments WHERE Id = @GroupId AND DepartmentId = @DepartmentId)  
    BEGIN  
        INSERT INTO GroupDepartments (Id, DepartmentId,CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, IsActive, IsDeleted)  
        VALUES (@GroupId, @DepartmentId,@UserId, GETDATE(), @UserId, GETDATE(), 1, 0);  
    END  
  
    -- Insert into GroupPermission if not exists  
    DECLARE PermissionCursor2 CURSOR FOR  
    SELECT Id FROM Permissions  
    WHERE PermissionName IN ('Visible', 'Read', 'Create', 'Update', 'Delete');  
  
    OPEN PermissionCursor2;  
    FETCH NEXT FROM PermissionCursor2 INTO @PermissionId;  
  
    WHILE @@FETCH_STATUS = 0  
    BEGIN  
        IF NOT EXISTS (SELECT 1 FROM GroupPermissions WHERE GroupId = @GroupId AND PermissionId = @PermissionId)  
        BEGIN  
            INSERT INTO GroupPermissions (GroupId, PermissionId, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, IsActive, IsDeleted)  
            VALUES (@GroupId, @PermissionId, @UserId, GETDATE(), @UserId, GETDATE(), 1, 0);  
        END  
        FETCH NEXT FROM PermissionCursor2 INTO @PermissionId;  
    END  
  
    CLOSE PermissionCursor2;  
    DEALLOCATE PermissionCursor2;  
  /*
    -- Insert into PagePermission if not exists  
    DECLARE PermissionCursor3 CURSOR FOR  
    SELECT Id FROM Permissions  
    WHERE PermissionName IN ('Visible', 'Read', 'Create', 'Update', 'Delete');  
  
    OPEN PermissionCursor3;  
    FETCH NEXT FROM PermissionCursor3 INTO @PermissionId;  
  
    WHILE @@FETCH_STATUS = 0  
    BEGIN  
        IF NOT EXISTS (SELECT 1 FROM PagePermissions WHERE PageId = @PageId AND PermissionId = @PermissionId)  
        BEGIN  
            INSERT INTO PagePermissions (PageId, PermissionId,GroupId, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, IsActive, IsDeleted)  
            VALUES (@PageId, @PermissionId,@groupID, @UserId, GETDATE(), @UserId, GETDATE(), 1, 0);  
        END  
        FETCH NEXT FROM PermissionCursor3 INTO @PermissionId;  
    END  
  
    CLOSE PermissionCursor3;  
    DEALLOCATE PermissionCursor3;  
  */

   -- Insert into PagePermissions for each page  
    DECLARE @PageCursor CURSOR;  
    DECLARE @CurrentPageId INT;  
  
    SET @PageCursor = CURSOR FOR  
    SELECT Id FROM Pages WHERE PageName IN ('Dashboard', 'Page', 'User', 'Role', 'AccessManagement');  
  
    OPEN @PageCursor;  
    FETCH NEXT FROM @PageCursor INTO @CurrentPageId;  
  
    WHILE @@FETCH_STATUS = 0  
    BEGIN  
        DECLARE PermissionCursor3 CURSOR FOR  
        SELECT Id FROM Permissions  
        WHERE PermissionName IN ('Visible', 'Read', 'Create', 'Update', 'Delete');  
  
        OPEN PermissionCursor3;  
        FETCH NEXT FROM PermissionCursor3 INTO @PermissionId;  
  
        WHILE @@FETCH_STATUS = 0  
        BEGIN  
            IF NOT EXISTS (SELECT 1 FROM PagePermissions WHERE PageId = @CurrentPageId AND PermissionId = @PermissionId)  
            BEGIN  
                INSERT INTO PagePermissions (PageId, PermissionId,PermissionName, GroupID, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, IsActive, IsDeleted)  
                VALUES (@CurrentPageId, @PermissionId,(SELECT PermissionName from Permissions where id = @PermissionId), @GroupId, @UserId, GETDATE(), @UserId, GETDATE(), 1, 0);  
            END  
            FETCH NEXT FROM PermissionCursor3 INTO @PermissionId;  
        END  
  
        CLOSE PermissionCursor3;  
        DEALLOCATE PermissionCursor3;  
  
        FETCH NEXT FROM @PageCursor INTO @CurrentPageId;  
    END  
  
    CLOSE @PageCursor;  
    DEALLOCATE @PageCursor;  




    -- Assign 'Admin' role permissions to the default user  
    DECLARE PermissionCursor4 CURSOR FOR  
    SELECT Id FROM Permissions WHERE PermissionName IN ('Visible', 'Read', 'Create', 'Update', 'Delete');  
  
    OPEN PermissionCursor4;  
    FETCH NEXT FROM PermissionCursor4 INTO @PermissionId;  
  
    WHILE @@FETCH_STATUS = 0  
    BEGIN  
        IF NOT EXISTS (SELECT 1 FROM RolePermissions WHERE RoleId = @AdminRoleId AND PermissionId = @PermissionId)  
        BEGIN  
            INSERT INTO RolePermissions (RoleId, PermissionId, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, IsActive, IsDeleted)  
            VALUES (@AdminRoleId, @PermissionId, @UserId, GETDATE(), @UserId, GETDATE(), 1, 0);  
        END  
        FETCH NEXT FROM PermissionCursor4 INTO @PermissionId;  
    END  
  
    CLOSE PermissionCursor4;  
    DEALLOCATE PermissionCursor4;  
END  