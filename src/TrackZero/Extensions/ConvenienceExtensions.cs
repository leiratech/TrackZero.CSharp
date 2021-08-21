using System.Threading.Tasks;
using TrackZero.DataTransfer;

namespace TrackZero
{
    public static class ConvenienceExtensions
    {
        public static async Task<Entity> TrackUsingAsync(this Entity entity, TrackZeroClient trackZeroClient)
        {
            return await trackZeroClient.UpsertEntityAsync(entity).ConfigureAwait(false);
        }

        public static async Task<Event> TrackUsingAsync(this Event @event, TrackZeroClient trackZeroClient)
        {
            return await trackZeroClient.TrackEventAsync(@event).ConfigureAwait(false);
        }
    }
}
