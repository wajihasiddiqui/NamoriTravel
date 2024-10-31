namespace NamoriTravel.Common
{
    public class AppSettings
    {
        public static string DotWConnect_Url { get; private set; }

        public static void Initialize(IConfiguration configuration)
        {
            DotWConnect_Url = configuration["Api_URL:DotWConnect_Url"] ?? "https://xmldev.dotwconnect.com/gatewayV4.dotw";
        }

    }
}
