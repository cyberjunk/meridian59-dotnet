using System.Collections.Generic;

namespace Meridian59.Patcher
{
    /// <summary>
    /// Implements a thread-safe queue for 'PatchFile' items.
    /// </summary>
    public class PatchFileQueue
    {
        /// <summary>
        /// Basic queue class from .net
        /// </summary>
        protected Queue<PatchFile> queue;

        /// <summary>
        /// Constructor
        /// </summary>
        public PatchFileQueue()
        {
            queue = new Queue<PatchFile>();
        }

        public void Enqueue(PatchFile Item)
        {
            lock (queue)
            {
                // enqueue item
                queue.Enqueue(Item);
            }
        }

        public bool TryDequeue(out PatchFile Item)
        {
            bool returnValue = false;
            Item = default(PatchFile);

            lock (queue)
            {
                if (queue.Count > 0)
                {
                    // dequeue item
                    Item = queue.Dequeue();

                    //
                    returnValue = true;
                }
            }

            return returnValue;
        }

        public int Count
        {
           get
           {
              int returnValue = 0;
              lock(queue)
              {
                 returnValue = queue.Count;
              }
              return returnValue;
           }
        }
    }
}
