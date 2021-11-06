using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using TrackZero.DataTransfer;

namespace TrackZero.Example1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // For full documentation, please visist https://trackzero.io

            var serviceProvider =
                new ServiceCollection()

                // This adds TrackZero as a Singleton to Dependency Injection. Make sure to add your API Key
                .AddTrackZero("{Place your API Key Here}")
                .BuildServiceProvider();

            // You can get the trackZero instance by requesting it from the service provider, or by adding the TrackZeroClient to the constructor class.
            var trackZeroClient = serviceProvider.GetRequiredService<TrackZeroClient>();

            // Prepare your customer space, assuming your customer id is 7766
            string analyticsSpaceId = "7766";
            await trackZeroClient.CreateAnalyticsSpaceAsync(analyticsSpaceId);

            // Prepare your entity (Order object), if we already have an entity of Type "Order" and Id 1, this will update the information stored in TrackZero with the ones we set here.
            Entity order = new Entity("Order", 1)
                // Adding attributes (Properties) to send to TrackZero and make it reportable.
                .AddAttribute("Quantity", 99.0)
                .AddAttribute("Total Amount", 99.0)
                .AddAttribute("Time", DateTimeOffset.UtcNow)
                // Adding Reference Attributes that will link to other entities.
                // Since the Entity of Type "Product" and Id 758 doesn't exist in our project, it will be created automatically. We can add more attributes to it later.
                .AddEntityReferencedAttribute("Item1", "Product", 758)
                .AddEntityReferencedAttribute("Item2", "Product", 214);

            // Send the Entity to TrackZero
            await trackZeroClient.UpsertEntityAsync(order, analyticsSpaceId)
                // If the context/scheduler do not matter to your application flow, use ConfigureAwait(false) as it aids performance.
                // For more details on ConfigureAwait, check this great blog post https://devblogs.microsoft.com/dotnet/configureawait-faq/
                .ConfigureAwait(false);


            // In order for your customer to run analytics on the data, we need to create a session key then redirect them to the TrackZero Spaces portal.
            // We will need to specify the custoemr space id and the duration of the session. In this case the duration is 30 minutes.
            // the session duration cannot be less than 5 minutes or longer than 60 minutes.
            var session = await trackZeroClient.CreateAnalyticsSpacePortalSessionAsync(analyticsSpaceId, TimeSpan.FromMinutes(30));
            
            // redirect the user to the url stored in session.OperationData.Url
            // code omitted as this is not a backend practice.

            // Cleanup.
            await trackZeroClient.DeleteAnalyticsSpaceAsync("7766");

        }
    }
}
