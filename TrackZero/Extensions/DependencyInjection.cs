using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace TrackZero
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddTrackZero(IServiceCollection serviceCollection, Uri baseUri, string projectId, string projectSecret)
        {
            serviceCollection
                .AddHttpClient("TrackZero", c => c.BaseAddress = baseUri);
            return serviceCollection.AddSingleton<TrackZeroClient>(new TrackZeroClient());

        }

        public static IServiceCollection AddTrackZero(IServiceCollection serviceCollection, string connectionString)
        {

            serviceCollection
                .AddHttpClient("TrackZero", c => c.BaseAddress = baseUri);
            return serviceCollection.AddSingleton<TrackZeroClient>(sp=> new TrackZeroClient(sp.GetRequiredService<IHttpClientFactory>(), ));

        }
    }
}
