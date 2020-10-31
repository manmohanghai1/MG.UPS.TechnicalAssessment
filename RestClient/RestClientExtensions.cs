using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MG.UPS.TechnicalAssessment.ServiceClientRest
{
    public static class HttpResponseExtensions
    {
        public static async Task<T> ReadAsync<T>(this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();

            return JToken
                .Parse(json)
                .ToObject<T>();
        }
    }
}
