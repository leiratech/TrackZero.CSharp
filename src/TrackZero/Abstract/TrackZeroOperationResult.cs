using System;

namespace TrackZero.Abstract
{
    /// <summary>
    /// TrackZero Operation Result encapsulation object.
    /// </summary>
    /// <typeparam name="TOperationData"></typeparam>
    public class TrackZeroOperationResult<TOperationData>
    {
        internal static TrackZeroOperationResult<TOperationData> GenericFailure => new TrackZeroOperationResult<TOperationData>(default, false, null);

        /// <summary>
        /// The data related to the operation.
        /// </summary>
        public TOperationData OperationData { get; set; }

        /// <summary>
        /// Set to True if the operation is successful, False if the operation fails.
        /// </summary>
        public bool IsSuccessfull { get; set; }

        /// <summary>
        /// Filled with the exception if a local exception is encountered.
        /// </summary>
        public Exception Exception { get; set; }

        internal TrackZeroOperationResult()
        { }

        internal TrackZeroOperationResult(TOperationData operationData)
        {
            IsSuccessfull = true;
            OperationData = operationData;
            Exception = null;
        }

        internal TrackZeroOperationResult(Exception ex)
        {
            OperationData = default;
            IsSuccessfull = false;
            Exception = ex;
        }

        internal TrackZeroOperationResult(TOperationData OperationData, bool isSuccessfull, Exception exception)
        {
            this.OperationData = OperationData;
            IsSuccessfull = isSuccessfull;
            Exception = exception;
        }
    }

    /// <summary>
    /// TrackZero Operation Result encapsulation object.
    /// </summary>
    public class TrackZeroOperationResult
    {
        internal static TrackZeroOperationResult GenericFailure => new TrackZeroOperationResult(false, null);

        /// <summary>
        /// Set to True if the operation is successful, False if the operation fails.
        /// </summary>
        public bool IsSuccessfull { get; set; }

        /// <summary>
        /// Filled with the exception if a local exception is encountered.
        /// </summary>
        public Exception Exception { get; set; }

        private TrackZeroOperationResult()
        { }

        internal TrackZeroOperationResult(Exception ex)
        {
            IsSuccessfull = false;
            Exception = ex;
        }

        internal TrackZeroOperationResult(bool isSuccessfull, Exception exception)
        {
            IsSuccessfull = isSuccessfull;
            Exception = exception;
        }
    }
}
