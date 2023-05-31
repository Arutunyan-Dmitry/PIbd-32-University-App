using Newtonsoft.Json;
using System.Text;

namespace DepartmentHTTPClient
{
    public class APIClient
    {
        private static readonly HttpClient client = new HttpClient();

        private static readonly string ServerIp = "http://192.168.43.174:7249/";
        public static int DepartmentId { get; set; }
        public static string DepartmentName { get; set; }

        public static T GetRequest<T>(string requestUrl)
        {
            var response = client.GetAsync(ServerIp + requestUrl);
            var result = response.Result.Content.ReadAsStringAsync().Result;
            if (response.Result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<T>(result);
            }
            else
            {
                throw new Exception(result);
            }
        }

        public static void PostRequest<T>(string requestUrl, T model)
        {
            var json = JsonConvert.SerializeObject(model);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(ServerIp + requestUrl, data);
            var result = response.Result.Content.ReadAsStringAsync().Result;
            if (!response.Result.IsSuccessStatusCode)
            {
                throw new Exception(result);
            }
        }
    }
}
