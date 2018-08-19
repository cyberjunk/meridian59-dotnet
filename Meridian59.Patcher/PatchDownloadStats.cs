using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Meridian59.Patcher
{
    public class PatchDownloadStats
    {
        /// <summary>
        /// Last download amount seen (for calculating speed).
        /// Only accessed by DownloadForm, no lock.
        /// </summary>
        public long lastLengthDone;

        /// <summary>
        /// Total files seen in JSON file, accessed only from Patcher.
        /// </summary>
        public long totalFilesFromJson;

        /// <summary>
        /// Number of files checked via hash/Download property/exclusion.
        /// </summary>
        private long totalFilesChecked;

        /// <summary>
        /// Stats for amount of bytes/number of files to download.
        /// </summary>
        private long lengthToDownload;
        private long lengthDownloaded;
        private long numFilesToDownload;
        private long numFilesDone;

        /// <summary>
        /// Provides threadsafe access to total number of files checked.
        /// Includes a locking!
        /// </summary>
        public long TotalFilesChecked
        {
            get
            {
                return Interlocked.Read(ref totalFilesChecked);
            }
        }

        /// <summary>
        /// Provides threadsafe access to total amount to download.
        /// Includes a locking!
        /// </summary>
        public long LengthToDownload
        {
            get
            {
                return Interlocked.Read(ref lengthToDownload);
            }
        }

        /// <summary>
        /// Provides threadsafe access to total amount downloaded so far.
        /// Includes a locking!
        /// </summary>
        public long LengthDownloaded
        {
            get
            {
                return Interlocked.Read(ref lengthDownloaded);
            }
        }

        /// <summary>
        /// Provides threadsafe access to total number of files to download.
        /// Includes a locking!
        /// </summary>
        public long NumFilesToDownload
        {
            get
            {
                return Interlocked.Read(ref numFilesToDownload);
            }
        }

        /// <summary>
        /// Provides threadsafe access to total number of files completed.
        /// Includes a locking!
        /// </summary>
        public long NumFilesDone
        {
            get
            {
                return Interlocked.Read(ref numFilesDone);
            }
        }

        /// <summary>
        /// Thread-safe addition of files to download.
        /// </summary>
        public void IncrementFilesToDownload()
        {
            Interlocked.Increment(ref numFilesToDownload);
        }

        /// <summary>
        /// Thread-safe addition of files checked.
        /// </summary>
        public void IncrementFilesChecked()
        {
            Interlocked.Increment(ref totalFilesChecked);
        }

        /// <summary>
        /// Thread-safe addition of completed files.
        /// </summary>
        public void IncrementFilesDownloaded()
        {
            Interlocked.Increment(ref numFilesDone);
        }

        /// <summary>
        /// Thread-safe addition of downloaded bytes.
        /// </summary>
        /// <param name="Bytes"></param>
        public void AddDownloadedBytes(long Bytes)
        {
            Interlocked.Add(ref lengthDownloaded, Bytes);
        }

        /// <summary>
        /// Thread-safe addition of bytes to download.
        /// </summary>
        /// <param name="Bytes"></param>
        public void AddBytesToDownload(long Bytes)
        {
            Interlocked.Add(ref lengthToDownload, Bytes);
        }

        public PatchDownloadStats()
        {
            this.totalFilesFromJson = 0;
            this.totalFilesChecked = 0;
            this.lastLengthDone = 0;
            this.lengthToDownload = 0;
            this.lengthDownloaded = 0;
            this.numFilesToDownload = 0;
            this.numFilesDone = 0;
        }
    }
}
