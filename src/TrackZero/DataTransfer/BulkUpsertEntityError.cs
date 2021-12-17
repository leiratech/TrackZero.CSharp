using System;

namespace TrackZero.DataTransfer
{
    /// <summary>
    /// Errors related to Bulk Upserting an Entity
    /// </summary>
    [Obsolete]
    public enum BulkUpsertEntityError
    {
        /// <summary>
        /// No Error
        /// </summary>
        None = 0,
        /// <summary>
        /// Obsolete
        /// </summary>
        Forbid = 1,
        /// <summary>
        /// Obsolete
        /// </summary>
        InsufficientApiKeyPrevilages = 2,
        /// <summary>
        /// Obsolete
        /// </summary>
        SubscriptionExpired = 3,
        /// <summary>
        /// Obsolete
        /// </summary>
        StorageLimitExceeded = 4,
        /// <summary>
        /// Obsolete
        /// </summary>
        UnknownError = 5,
        /// <summary>
        /// Obsolete
        /// </summary>
        InvalidSpaceId = 6,
    }


}
