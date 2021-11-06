using System.Threading.Tasks;
using TrackZero.Abstract;
using TrackZero.DataTransfer;

namespace TrackZero
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConvenienceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="analyticsSpaceId"></param>
        /// <param name="trackZeroClient"></param>
        /// <returns></returns>
        public static async Task<TrackZeroOperationResult<Entity>> TrackUsingAsync(this Entity entity, string analyticsSpaceId, TrackZeroClient trackZeroClient)
        {
            return await trackZeroClient.UpsertEntityAsync(entity, analyticsSpaceId).ConfigureAwait(false);
        }


    }
}
