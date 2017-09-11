using System;
using System.Collections.Concurrent;
using System.Web;
using System.IO;
using Meridian59.Files.BGF;

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
        }

        private static readonly ConcurrentDictionary<string, Entry> cache =
            new ConcurrentDictionary<string, Entry>();

        /// <summary>
        /// Loads all BGF from subfolder "bgf" in document root
        /// </summary>
        public static void Preload()
        {
            string filePath = HttpRuntime.AppDomainAppPath + "bgf/";
            string[] files = Directory.GetFiles(filePath, "*.bgf");

            foreach (string s in files)
            {
                try
                {
                    // read from disk and decompress all (important!)
                    BgfFile bgf = new BgfFile(s);
                    bgf.DecompressAll();

                    Entry entry = new Entry();
                    entry.Bgf = bgf;
                    entry.LastModified = File.GetLastWriteTimeUtc(s);

                    // add to cache
                    cache.TryAdd(bgf.Filename, entry);
                }
                catch (Exception) { }
            }

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
            // not yet cached
            if (!cache.TryGetValue(Key, out Value))
            {
                // build bgf path
                string filePath = HttpRuntime.AppDomainAppPath + "bgf/" + Key + ".bgf";

                // bgf file doees not exist
                if (!File.Exists(filePath))
                    return false;

                // try load it
                else
                {
                    try
                    {
                        // read from disk and decompress all (important!)
                        BgfFile bgf = new BgfFile(filePath);
                        bgf.DecompressAll();

                        Entry entry = new Entry();
                        entry.Bgf = bgf;
                        entry.LastModified = File.GetLastWriteTimeUtc(filePath);

                        // add to cache
                        cache.TryAdd(bgf.Filename, entry);

                        return true;
                    }
                    catch (Exception) { return false; }
                }
            }

            // returned from cache
            else
                return true;
        }
    }
}
