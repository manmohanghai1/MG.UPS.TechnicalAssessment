using System.Net.Http;
using System.Threading.Tasks;

namespace MG.UPS.TechnicalAssessment.ServiceClientRest
{
    public interface IRestClient
    {
        Task<HttpResponseMessage> DeleteAsync(string uri);
        Task<HttpResponseMessage> GetAsync(string uri);
        Task<HttpResponseMessage> PostAsync(string uri, HttpContent content);
        Task<HttpResponseMessage> PutAsync(string uri, HttpContent content);

        Task<T> GetAsync<T>(string uri);
        Task<T> PostAsync<T>(string uri, HttpContent content);
        Task<T> PutAsync<T>(string uri, HttpContent content);
        Task<T> DeleteAsync<T>(string uri);
    }
    public class RestClient : IRestClient
    {
        HttpClient httpClient;

        public RestClient(IHttpClientFactory httpClient)
        {
            this.httpClient = httpClient.CreateClient("RestClient");
        }

        public async Task<HttpResponseMessage> GetAsync(string uri)
        {
            return await SendAsync(HttpMethod.Get, uri, null);
        }

        public async Task<HttpResponseMessage> PostAsync(string uri, HttpContent content)
        {
            return await SendAsync(HttpMethod.Post, uri, content);
        }

        public async Task<HttpResponseMessage> PutAsync(string uri, HttpContent content)
        {
            return await SendAsync(HttpMethod.Put, uri, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            return await SendAsync(HttpMethod.Delete, uri, null);
        }

        public async Task<T> GetAsync<T>(string uri)
        {
            var response = await this.GetAsync(uri);
            return await response.ReadAsync<T>();
        }

        public async Task<T> PostAsync<T>(string uri, HttpContent content)
        {
            var response = await this.PostAsync(uri, content);
            return await response.ReadAsync<T>();
        }

        public async Task<T> PutAsync<T>(string uri, HttpContent content)
        {
            var response = await this.PutAsync(uri, content);
            return await response.ReadAsync<T>();
        }

        public async Task<T> DeleteAsync<T>(string uri)
        {
            var response = await this.DeleteAsync(uri);
            return await response.ReadAsync<T>();
        }

        private async Task<HttpResponseMessage> SendAsync(HttpMethod httpMethod, string uri, HttpContent content)
        {
            var request = new HttpRequestMessage(httpMethod, httpClient.BaseAddress.AbsoluteUri + uri)
            {
                Content = content,
            };

            return await httpClient.SendAsync(request);
        }
    }
}