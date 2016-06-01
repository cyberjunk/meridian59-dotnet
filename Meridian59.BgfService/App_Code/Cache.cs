using Meridian59.Files.BGF;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// 
/// </summary>
public static class Cache
{
    private static ConcurrentDictionary<string, BgfFile> bgf =
        new ConcurrentDictionary<string, BgfFile>();
        
	public static bool GetBGF(string key, out BgfFile value)
    {
        return bgf.TryGetValue(key, out value);
    }

    public static bool AddBGF(string key, BgfFile value)
    {
        return bgf.TryAdd(key, value);
    }
}