using kloudscript.Test.API.Entity;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace kloudscript.Test.API.Services
{
    public interface IStateByZipService
    {
        Task<StateByZipEntity?> GetStateByZipAsync(string zipCode);
    }
    public class StateByZipService : IStateByZipService
    {
        private  HttpClient? httpClient { get; set; }
        private  void InitHttpClient()
        {
            int connectionTimeOut = 2; 
            string apiBaseAddress = "https://tools.usps.com/"; 

            var innerHttpHandler = new HttpClientHandler { UseDefaultCredentials = true, PreAuthenticate = true };
            httpClient = new HttpClient(innerHttpHandler);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("Connection", "Keep-alive");
            httpClient.Timeout = new TimeSpan(0, connectionTimeOut, 0);
            httpClient.BaseAddress = new Uri(apiBaseAddress);
        }
        public async Task<StateByZipEntity?> GetStateByZipAsync(string zipCode)
        {
            if (httpClient == null)
            {
                InitHttpClient();
            }
            
            Dictionary<string, string> inputPara = new Dictionary<string, string>();
            inputPara.Add("zip",zipCode);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{httpClient.BaseAddress}tools/app/ziplookup/cityByZip"),
                Content = new FormUrlEncodedContent(new Dictionary<string, string> {{ "zip",zipCode },})
            };

            using (var response = await httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<StateByZipEntity>(body);

            }
        }
    }
}
