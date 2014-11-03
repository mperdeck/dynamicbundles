using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicBundles.Models;
using System.IO;

namespace DynamicBundles
{
    public class DependencyResolver
    {
        /// <summary>
        /// Takes the path to a directory and returns the files in that directory, and all the files
        /// that that directory depends on (via .nuspec files).
        /// 
        /// It does not go into sub directories.
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
            var fileListsByAssetType = CacheHelper.Get(dirPath, ()=> GetRequiredFilesForDirectoryUnchached(dirPath), new [] { dirPath });
            return fileListsByAssetType;
        }

        /// <summary>
        /// Same as GetRequiredFilesForDirectory, but uncached.
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        private static FileListsByAssetType GetRequiredFilesForDirectoryUnchached(string dirPath)
        {


                    Dim dependenciesFilePaths As IEnumerable(Of String) = Directory.EnumerateFiles(componentDirectory, dependencyFileWildcard)










            return null;
        }
    }
}
