using System;

namespace Meridian59.Patcher
{
    public class JsonFileProgress
    {
        /// <summary>
        /// Used for locking on access of bytesReceived.
        /// </summary>
        private object bytesReceivedlockObject;

        private long bytesReceived;

        /// <summary>
        /// Used for locking on access of bytesTotal.
        /// </summary>
        private object bytesTotallockObject;

        private long bytesTotal;

        public JsonFileProgress()
        {
            this.bytesReceivedlockObject = new Object();
            this.bytesTotallockObject = new Object();
            this.bytesReceived = 0;
            this.bytesTotal = 0;
        }

        /// <summary>
        /// Provides threadsafe access to the amount of downloaded bytes.
        /// Includes a locking!
        /// </summary>
        public long BytesReceived
        {
            get
            {
                long val = 0;
                lock (bytesReceivedlockObject) { val = bytesReceived; }
                return val;
            }
            set
            {
                long val = value;
                lock (bytesReceivedlockObject) { bytesReceived = val; }
            }
        }

        /// <summary>
        /// Provides threadsafe access to the amount of total bytes.
        /// Includes a locking!
        /// </summary>
        public long BytesTotal
        {
            get
            {
                long val = 0;
                lock (bytesTotallockObject) { val = bytesTotal; }
                return val;
            }
            set
            {
                long val = value;
                lock (bytesTotallockObject) { bytesTotal = val; }
            }
        }
    }
}
