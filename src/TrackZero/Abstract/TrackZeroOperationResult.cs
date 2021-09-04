using System;

namespace TrackZero.Abstract
{
    public class TrackZeroOperationResult<TOperationData>
    {
        internal static TrackZeroOperationResult<TOperationData> GenericFailure => new TrackZeroOperationResult<TOperationData>(default, false, null);

        public TOperationData OperationData { get; set; }
        public bool IsSuccessfull { get; set; }
        public Exception Exception { get; set; }

        private TrackZeroOperationResult()
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

        private TrackZeroOperationResult(TOperationData OperationData, bool isSuccessfull, Exception exception)
        {
            this.OperationData = OperationData;
            IsSuccessfull = isSuccessfull;
            Exception = exception;
        }
    }
}
