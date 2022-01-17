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

            // Prepare your entity (Order object), if we already have an entity of Type "Order" and Id 1,
            // this will update the information stored in TrackZero with the ones we set here.
            Entity order = new Entity("Orders", 1)
                // Adding attributes (Properties) to send to TrackZero and make it reportable.
                .AddAttribute("Quantity", 99.0)
                .AddAttribute("Total Amount", 99.0)
                .AddAttribute("Time", DateTimeOffset.UtcNow)
                // Adding Reference Attributes that will link to other entities.
                // Since the Entity of Type "Product" and Id 758 doesn't exist in our project, it will be created automatically.
                // We can add more attributes to it later.
                .AddEntityReferencedAttribute("Items", "Products", 758)
                .AddEntityReferencedAttribute("Items", "Products", 214)
                // Adding reference to the user who made the order
                .AddEntityReferencedAttribute("Order By", "Users", 245)
                // To link this order to a country, you can simple pass the lat and long of the order and
                // TrackZero will automatically manage this data appropriately to allow Map Charts.
                .AddAutomaticallyTranslatedGeoPoint(41.037086118695825, 28.98489855136287);

            // Send the Entity to TrackZero
            await trackZeroClient.UpsertEntityAsync(order, analyticsSpaceId)
                // If the context/scheduler do not matter to your application flow, use ConfigureAwait(false) as it aids performance.
                // For more details on ConfigureAwait, check this great blog post https://devblogs.microsoft.com/dotnet/configureawait-faq/
                .ConfigureAwait(false);
           
            // In order for your customer to run analytics on the data, we need to create a session key then redirect them to the TrackZero Spaces portal.
            // We will need to specify the custoemr space id and the duration of the session. In this case the duration is 30 minutes.
            // the session duration cannot be less than 5 minutes or longer than 14400 minutes (4 hours).
            var session = await trackZeroClient.CreateAnalyticsSpacePortalSessionAsync(analyticsSpaceId, TimeSpan.FromMinutes(30));

            // redirect the user to the url stored in session.OperationData.Url
            // OR
            // Embed the Dashboards (session.OperationData.EmbeddedDashboarsdUrl) and Reports (session.OperationData.EmbeddedReportsUrl)
            // in an iframe tag in your pages so the user doesn't leave your site.
            // code omitted as this is a frontend practice.

            // delete the Entity "Order" with id 1 from TrackZero.
            await trackZeroClient.DeleteEntityAsync(new EntityReference("Orders", 1), analyticsSpaceId).ConfigureAwait(false);

            // Cleanup.
            await trackZeroClient.DeleteAnalyticsSpaceAsync(analyticsSpaceId).ConfigureAwait(false);

        }
    }
}
