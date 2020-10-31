using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;

namespace MG.UPS.TechnicalAssessment.ServiceClientRest
{
    public class RestClientBuilder
    {
        IServiceCollection services;
        public RestClientBuilder(IServiceCollection serviceCollection)
        {
            this.services = serviceCollection;
        }

        internal RestClientBuilder Configure(Action<RestClientOptions> options)
        {
            services.Configure(options);
            return this;
        }

        internal RestClientBuilder AddHttpClient()
        {           
            services.AddScoped<IRestClient, RestClient>();

            services.AddHttpClient<RestClient>()
                .ConfigureHttpClient((serviceProvider, client) =>
                {
                    var options = serviceProvider
                                        .GetRequiredService<IOptions<RestClientOptions>>()
                                        .Value;

                    client.BaseAddress = new Uri(options.BaseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", options.APIKey);
                });

            return this;
        }
    }
}