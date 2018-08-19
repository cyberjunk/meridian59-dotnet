using System;
using System.Runtime.Serialization;

namespace Meridian59.Patcher
{
    /// <summary>
    /// An instance of this class represents an entry from the patchinfo.txt JSON array.
    /// </summary>
    [DataContract]
    public class PatchFile
    {
        #region JSON values
        [DataMember] public string Basepath { get; protected set; }
        [DataMember] public string Filename { get; protected set; }
        [DataMember] public string MyHash { get; protected set; }
        [DataMember] public long Length { get; protected set; }
        [DataMember] public bool Download { get; protected set; }
        [DataMember] public int Version { get; protected set; }
        #endregion

        /// <summary>
        /// Track error counts of the patchfile here.
        /// </summary>
        public int ErrorCount;

        /// <summary>
        /// Used for locking on access of LengthDone.
        /// </summary>
        private object lengthDonelockObject;

        /// <summary>
        /// Provides value for the LengthDone property.
        /// </summary>
        private long lengthDone;

        /// <summary>
        /// Used for locking on access of HashedStatus.
        /// </summary>
        private object hashedStatuslockObject;

        /// <summary>
        /// Provides value for the LengthDone property.
        /// </summary>
        private PatchFileHashedStatus hashedStatus;

        /// <summary>
        /// Private constructor. Instances created by deserializer.
        /// </summary>
        private PatchFile()
        {
        }

        /// <summary>
        /// Pseudo constructor for initialization after deserialization
        /// </summary>
        /// <param name="ctx"></param>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext ctx)
        {
            this.lengthDonelockObject = new Object();
            this.hashedStatuslockObject = new Object();
            this.ErrorCount = 0;
            this.lengthDone = 0;
            this.hashedStatus = PatchFileHashedStatus.NotHashed;
            this.Basepath = Basepath.Replace("\\\\", "/").Replace("\\", "/");         
        }

        /// <summary>
        /// Provides threadsafe access to the amount of already loaded bytes.
        /// Includes a locking!
        /// </summary>
        public long LengthDone
        {
            get 
            {
                long val = 0;
                lock (lengthDonelockObject) { val = lengthDone; }
                return val; 
            }
            set 
            { 
                long val = value;
                lock (lengthDonelockObject) { lengthDone = val; } 
            }
        }

        public PatchFileHashedStatus HashedStatus
        {
            get
            {
                PatchFileHashedStatus val = PatchFileHashedStatus.NotHashed;
                lock (hashedStatuslockObject) { val = hashedStatus; }
                return val;
            }
            set
            {
                PatchFileHashedStatus val = value;
                lock (hashedStatuslockObject) { hashedStatus = val; }
            }
        }

        /// <summary>
        /// EventArgs carrying a 'PatchFile' instance.
        /// </summary>
        public class EventArgs : System.EventArgs
        {
            public readonly PatchFile File;

            public EventArgs(PatchFile File)
            {
                this.File = File;
            }
        }
    }
}
