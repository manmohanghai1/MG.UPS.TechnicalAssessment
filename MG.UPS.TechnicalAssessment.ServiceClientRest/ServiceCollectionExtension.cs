using Microsoft.Extensions.DependencyInjection;
using System;

namespace MG.UPS.TechnicalAssessment.ServiceClientRest
{
    public static class ServiceCollectionRestClientExtension
    {
        public static RestClientBuilder AddRestClient(this ServiceCollection serviceCollection, Action<RestClientOptions> options)
        {
            return new RestClientBuilder(serviceCollection)
                .Configure(options)
                .AddHttpClient();
        }
    }
}
