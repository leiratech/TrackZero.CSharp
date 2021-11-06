using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace TrackZero
{
    /// <summary>
    /// 
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="projectApiKey"></param>
        /// <param name="throwExcpetions"></param>
        /// <returns></returns>
        public static IServiceCollection AddTrackZero(this IServiceCollection serviceCollection, string projectApiKey, bool throwExcpetions = true)
        {
            serviceCollection
                .AddHttpClient("TrackZero", c =>
                {
                    c.BaseAddress = new Uri("https://betaapi.trackzero.io");
                    //c.BaseAddress = new Uri("https://localhost:5001");
                    c.DefaultRequestHeaders.Add("X-API-KEY", projectApiKey);
                    c.DefaultRequestHeaders.Add("X-API-VERSION", "1.0");
                });
            return serviceCollection.AddSingleton<TrackZeroClient>(sp =>
            {
                return new TrackZeroClient(sp, sp.GetRequiredService<IHttpClientFactory>(), throwExcpetions);
            });
        }
    }
}
