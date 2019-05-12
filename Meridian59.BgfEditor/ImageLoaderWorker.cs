using Meridian59.Files.BGF;
using System.Drawing;
using System.Threading;

namespace Meridian59.BgfEditor
{
    public class ImageLoaderWorker
    {
        protected readonly Thread thread;
        protected volatile bool IsRunning;

        public ImageLoaderWorker()
        {
            // create thread
            thread = new Thread(ThreadProc);
            thread.IsBackground = true;
        }

        /// <summary>
        /// Starts the workerthread and processes items from the BitmapFileQueue
        /// into the BgfBitmapQueue (UI thread adds these to form datasource).
        /// </summary>
        public void Start()
        {
            if (IsRunning)
                return;

            IsRunning = true;
            thread.Start();
        }

        /// <summary>
        /// Stops the workerthread.
        /// </summary>
        public void Stop()
        {
            IsRunning = false;
        }

        /// <summary>
        /// Internal thread loop
        /// </summary>
        protected void ThreadProc()
        {
            uint version = Program.CurrentFile.Version;
            Bitmap bitmap;

            while (IsRunning)
            {
                while (Program.BitmapFileQueue.TryDequeue(out string file))
                {
                    // load bitmap from file
                    bitmap = new Bitmap(file);

                    // get pixels
                    byte[] pixelData = BgfBitmap.BitmapToPixelData(bitmap);

                    // create BgfBitmap
                    BgfBitmap bgfBitmap = new BgfBitmap(
                        0, // num determined when adding to list after dequeue
                        version,
                        (uint)bitmap.Width,
                        (uint)bitmap.Height,
                        0,
                        0,
                        new BgfBitmapHotspot[0],
                        false,
                        0,
                        pixelData);

                    // cleanp temporary bitmap
                    bitmap.Dispose();

                    // enqueue for adding to frames
                    Program.BgfBitmapQueue.Enqueue(bgfBitmap);
                }

                // Done
                IsRunning = false;
            }
        }
    }
}
