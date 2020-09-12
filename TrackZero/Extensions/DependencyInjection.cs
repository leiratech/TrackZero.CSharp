using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TrackZero
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTrackZero(this IServiceCollection serviceCollection, Uri baseUri, string projectId, string projectSecret)
        {
            serviceCollection
                .AddHttpClient("TrackZero", c => c.BaseAddress = baseUri);
            return serviceCollection.AddSingleton<TrackZeroClient>(sp => new TrackZeroClient(sp.GetRequiredService<IHttpClientFactory>(), baseUri, projectId, projectSecret));
        }

        public static IServiceCollection AddTrackZero(this IServiceCollection serviceCollection, string connectionString)
        {

            serviceCollection
                .AddHttpClient("TrackZero");
            return serviceCollection.AddSingleton<TrackZeroClient>(sp => new TrackZeroClient(sp.GetRequiredService<IHttpClientFactory>(), connectionString));

        }
    }
}
