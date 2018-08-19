using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Meridian59.Patcher
{
    public enum UpdateStage
    {
        None,
        DownloadingJson,
        HashingFiles,
        DownloadingFiles,
        Ngen,
        Finished,
        Abort
    }
}
