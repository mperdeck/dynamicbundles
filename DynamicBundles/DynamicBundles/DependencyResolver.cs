using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicBundles.Models;

namespace DynamicBundles
{
    public class DependencyResolver
    {
        /// <summary>
        /// Takes the path to a directory and returns the files in that directory, and all the files
        /// that that directory depends on (via .nuspec files).
        /// 
        /// Uses caching to reduce trips to the file system.
        /// </summary>
        /// <param name="dirPath">
        /// Path to the directory. Must be relative to the root of the project (start with a ~).
        /// </param>
        /// <returns>
        /// The required files, split by asset type.
        /// </returns>
        public static FileListsByAssetType GetRequiredFilesForDirectory(string dirPath)
        {
            //#########################

            return null;
        }


    }
}
