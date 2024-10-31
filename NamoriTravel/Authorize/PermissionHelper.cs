namespace NamoriTravel.Authorize
{
    public static class PermissionHelper
    {
        public static bool HasPermission(Dictionary<string, List<string>> permissionsDict, string pageName, string permission)
        {
            try
            {
                if (permissionsDict == null)
                    return false;

                return permissionsDict != null &&
                       permissionsDict.ContainsKey(pageName) &&
                       permissionsDict[pageName].Contains(permission);
            }
            catch
            {
                return false;
            }
        }
    }
}
