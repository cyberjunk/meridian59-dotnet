using System;
using System.Collections.Concurrent;
using System.Web;
using System.IO;
using Meridian59.Files.BGF;

/// <summary>
/// Loads BGF from disk and keeps them in memory
/// </summary>
public static class BgfCache
{
    private static readonly ConcurrentDictionary<string, BgfFile> cache =
        new ConcurrentDictionary<string, BgfFile>();

    /// <summary>
    /// Loads all BGF from subfolder "bgf" in document root
    /// </summary>
    public static void Preload()
    {
        string filePath = HttpRuntime.AppDomainAppPath + "bgf/";
        string[] files  = Directory.GetFiles(filePath, "*.bgf");

        foreach (string s in files)
        {
            try
            {
                BgfFile bgf = new BgfFile(s);    // read from disk
                bgf.DecompressAll();             // must decompress all (important!)
                cache.TryAdd(bgf.Filename, bgf); // add to cache
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
    public static bool GetBGF(string Key, out BgfFile Value)
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
                    // read from disk
                    Value = new BgfFile(filePath);

                    // must decompress all due to multithreading (important!)
                    Value.DecompressAll();

                    // add to cache
                    cache.TryAdd(Key, Value);
                    
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
