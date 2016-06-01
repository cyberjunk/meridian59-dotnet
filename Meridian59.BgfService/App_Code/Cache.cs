using Meridian59.Files.BGF;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

/// <summary>
/// 
/// </summary>
public static class Cache
{
    private static ConcurrentDictionary<string, BgfFile> bgf =
        new ConcurrentDictionary<string, BgfFile>();
        
	public static bool GetBGF(string key, out BgfFile value)
    {
        // not yet cached
        if (!bgf.TryGetValue(key, out value))
        {
            // build bgf path
            string filePath = HttpRuntime.AppDomainAppPath + "bgf/" + key + ".bgf";
            
            // bgf file doees not exist
            if (!File.Exists(filePath))
                return false;
           
            else
            {
                try
                {
                    // read from disk
                    value = new BgfFile(filePath);

                    // must decompress all due to multithreading (important!)
                    value.DecompressAll();

                    // add to cache
                    bgf.TryAdd(key, value);
                    
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