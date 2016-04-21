using System.Collections.Concurrent;

namespace Meridian59.Patcher
{
    /// <summary>
    /// Implements a thread-safe queue for 'PatchFile' items.
    /// </summary>
    public class PatchFileQueue : ConcurrentQueue<PatchFile>
    {
    }
}
