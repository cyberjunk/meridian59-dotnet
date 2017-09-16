using System;
using System.Collections.Concurrent;
using System.Web;
using System.IO;
using Meridian59.Files.BGF;
using System.Runtime;
using System.Collections.Generic;

namespace Meridian59.BgfService
{
    /// <summary>
    /// Loads BGF from disk and keeps them in memory
    /// </summary>
    public static class BgfCache
    {
        public class Entry
        {
            public BgfFile Bgf;
            public DateTime LastModified;
            public long Size;
            public uint Num;
        }

        private static readonly ConcurrentDictionary<string, Entry> cache =
            new ConcurrentDictionary<string, Entry>();

        public static DateTime LastModified { get; private set; }
        public static long LastModifiedStamp { get; private set; }

        /// <summary>
        /// Loads all BGF from subfolder "bgf" in document root
        /// </summary>
        public static void Load()
        {
            cache.Clear();

            string filePath = HttpRuntime.AppDomainAppPath + "bgf/";
            string[] files = Directory.GetFiles(filePath, "*.bgf");

            uint num = 1;
            foreach (string s in files)
            {
                try
                {
                    // get file info
                    FileInfo info = new FileInfo(s);

                    // read from disk and decompress all (important!)
                    BgfFile bgf = new BgfFile(s);
                    bgf.DecompressAll();

                    Entry entry = new Entry();
                    entry.Bgf = bgf;

                    // get filesystem lastwrite date
                    DateTime fileDate = info.LastWriteTimeUtc;

                    // store truncated to seconds
                    entry.LastModified = fileDate.AddTicks(-(fileDate.Ticks % TimeSpan.TicksPerSecond));
                    entry.Size = info.Length;

                    // set pseudo id (for appearance hashing)
                    entry.Num = num;
                    num++;

                    // add to cache
                    cache.TryAdd(bgf.Filename, entry);
                }
                catch (Exception) { }
            }

            // save when the cache was filled
            DateTime now = DateTime.UtcNow;
            LastModified = now.AddTicks(-(now.Ticks % TimeSpan.TicksPerSecond));
            LastModifiedStamp = (now.Ticks - 621355968000000000) / 10000000;

            // save array variant for iteration
            //pairs = BgfCache.cache.ToArray();

            // compact large object heap next run
            GCSettings.LargeObjectHeapCompactionMode =
                GCLargeObjectHeapCompactionMode.CompactOnce;

            // execute maximum GC run
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
        }

        /// <summary>
        /// Tries to retrieve a BGF from cache (or disk)
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool GetBGF(string Key, out Entry Value)
        {
            return cache.TryGetValue(Key, out Value);
        }

        /// <summary>
        /// Returns iterator for cache entries
        /// </summary>
        /// <returns></returns>
        public static IEnumerator<KeyValuePair<string, BgfCache.Entry>> GetEnumerator()
        {
            return cache.GetEnumerator();
        }
    }
}
