using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBundles.Models
{
    /// <summary>
    /// Stores lists of file paths by asset type.
    /// </summary>
    public class FileListsByAssetType: ListsByKey<AssetPath, AssetType>
    {
    }
}
