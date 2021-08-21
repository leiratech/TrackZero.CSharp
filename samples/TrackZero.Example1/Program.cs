using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrackZero.DataTransfer;

namespace TrackZero.Example1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var serviceProvider =
                new ServiceCollection()

                // This adds TrackZero as a Singleton to Dependency Injection. Make sure to add your API Key
                .AddTrackZero("e227989c-3ff5-4a5a-aaff-1fb18e0ad599.FqQInMoGACcfZJ3DmqLjTfvrYBIuFlNM")
                .BuildServiceProvider();

            // You can get the trackZero instance by requesting it from the service provider, or by adding the TrackZeroClient to the constructor class.
            var trackZeroClient = serviceProvider.GetRequiredService<TrackZeroClient>();

            // Prepare your entity (User object), if we already have an entity of Type "User" and Id 1, this will update the information stored in TrackZero with the ones we set here.
            Entity user = new Entity("user", 1)
                // Adding attributes (Properties) to send to TrackZero and make it reportable.
                .AddAttribute("Name", "Jane Doe")
                .AddAttribute("Birthdate", new DateTime(1988, 11, 7))
                // Adding Reference Attributes that will link to other entities.
                // Since the Entity of Type "Country" and Id "US" doesn't exist in our project, it will be created automatically. We will add more attributes to it later.
                .AddEntityReferencedAttribute("Home Country", "Country", "US");

            // Send the Entity to TrackZero
            await trackZeroClient.UpsertEntityAsync(user)
                // If the context/scheduler do not matter to your application flow, use ConfigureAwait(false) as it aids performance.
                // For more details on ConfigureAwait, check this great blog post https://devblogs.microsoft.com/dotnet/configureawait-faq/
                .ConfigureAwait(false);

            // Prepare the event. Every event has a minimum requirement of Name (Event Name), and Emitter info (The entitiy that triggered the event).
            Event myEvent = new Event("User", 1, "Subscribed")
            {
                StartTime = DateTime.UtcNow, //(Optional) If you don't specify StartTime, it will be automatically set to the current time UTC
                EndTime = DateTime.UtcNow, //(Optional) If you don't specify EndTime, it will be automatically set to the current time UTC
                Id = Guid.NewGuid(), // (Optional) Specifying your own Id prevents duplication if it happens and you send this event again. When not specified, it will be automatically set to a NewGuid.
            }
                // Just like entities, You can add attributes (Properties) to send to TrackZero and make it reportable.
                .AddAttribute("Paid Amount", 9.99)
                // Adding Reference Attributes that will link to other entities.
                // Since the Entity of Type "Country" and Id "US" doesn't exist in our project, it will be created automatically. We will add more attributes to it later.
                .AddEntityReferencedAttribute("User Source", "Marketing Campaign", "Appstore Direct Marketing");

            // Send the Event to TrackZero
            await trackZeroClient.TrackEventAsync(myEvent)
                // If the context/scheduler do not matter to your application flow, use ConfigureAwait(false) as it aids performance.
                // For more details on ConfigureAwait, check this great blog post https://devblogs.microsoft.com/dotnet/configureawait-faq/
                .ConfigureAwait(false);
        }
    }
}
