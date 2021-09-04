using System.Threading.Tasks;
using TrackZero.Abstract;
using TrackZero.DataTransfer;

namespace TrackZero
{
    public static class ConvenienceExtensions
    {
        public static async Task<TrackZeroOperationResult<Entity>> TrackUsingAsync(this Entity entity, TrackZeroClient trackZeroClient)
        {
            return await trackZeroClient.UpsertEntityAsync(entity).ConfigureAwait(false);
        }

        public static async Task<TrackZeroOperationResult<Event>> TrackUsingAsync(this Event @event, TrackZeroClient trackZeroClient)
        {
            return await trackZeroClient.UpsertEventAsync(@event).ConfigureAwait(false);
        }
    }
}
