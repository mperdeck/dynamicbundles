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
    public class FileListsByAssetType
    {
        Dictionary<AssetType, List<String>> store = new Dictionary<AssetType, List<string>>();

        public FileListsByAssetType()
        {
        }

        /// <summary>
        /// Appends another FileListsByAssetType to this FileListsByAssetType.
        /// This means that the lists of files of the other FileListsByAssetType are appended
        /// to the lists in this FileListsByAssetType, by asset type.
        /// 
        /// The order of the files is preserved.
        /// </summary>
        /// <param name="fileListsByAssetType"></param>
        public void Append(FileListsByAssetType fileListsByAssetType)
        {
            //###############
        }

        /// <summary>
        /// Returns the file list for the given asset type.
        /// </summary>
        /// <param name="AssetType"></param>
        /// <returns></returns>
        public List<String> GetFilesList(AssetType AssetType)
        {
            return null;
        }



    }
}
