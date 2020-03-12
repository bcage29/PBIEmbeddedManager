using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace PBIManagerWeb.Common
{
    public static class HttpHelper
    {
        public static StringContent GetStringContent(object body)
        {
            return new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");
        }
    }
}
