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
        /// <param name="accountApiKey"></param>
        /// <param name="throwExcpetions"></param>
        /// <returns></returns>
        public static IServiceCollection AddTrackZero(this IServiceCollection serviceCollection, string accountApiKey, bool throwExcpetions = true)
        {
            serviceCollection
                .AddHttpClient("TrackZero", c =>
                {
                    c.BaseAddress = new Uri("https://api.trackzero.io");
                    c.DefaultRequestHeaders.Add("X-API-KEY", accountApiKey);
                    c.DefaultRequestHeaders.Add("X-API-VERSION", "1.0");
                });
            return serviceCollection.AddSingleton<TrackZeroClient>(sp =>
            {
                return new TrackZeroClient(sp, sp.GetRequiredService<IHttpClientFactory>(), throwExcpetions);
            });
        }
    }
}
