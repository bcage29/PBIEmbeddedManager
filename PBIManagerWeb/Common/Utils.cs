using System.Web;

namespace PBIManagerWeb.Common
{
    public class Utils
    {
        public static string[] ParseAzureResourceId(string id)
        {
            string[] resourceParts = HttpUtility.UrlDecode(id).Split("/");
            return resourceParts;
        }
    }
}
