using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DynamicBundles.Models;
using System.IO;
using System.Web;

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
            //###############            string absoluteDirPath = VirtualPathUtility.ToAbsolute(dirPath);
            string absoluteDirPath = HttpContext.Current.Server.MapPath(dirPath);

            var fileListsByAssetType = CacheHelper.Get(dirPath, () => GetRequiredFilesForDirectoryUnchached(absoluteDirPath), new[] { absoluteDirPath });
            return fileListsByAssetType;
        }

        /// <summary>
        /// Same as GetRequiredFilesForDirectory, but uncached.
        /// </summary>
        /// <param name="absoluteDirPath">
        /// Path to the directory. Unlike GetRequiredFilesForDirectory, must be an absolute file system path.
        /// </param>
        /// <returns></returns>
        private static FileListsByAssetType GetRequiredFilesForDirectoryUnchached(string absoluteDirPath)
        {

            IEnumerable<string> filePaths = Directory.EnumerateFiles(absoluteDirPath);
            FileListsByAssetType fileListsByAssetType = new FileListsByAssetType();

            foreach(string filePath in filePaths)
            {
                AssetType? assetType = AssetTypeOfFile(filePath);
                if (assetType != null)
                {
                    fileListsByAssetType.Add(AbsolutePathToApplicationRelativeUrl(filePath), assetType.Value);
                }
            }

            return fileListsByAssetType;
        }

        /// <summary>
        /// Takes a file path, and returns the asset type of that file.
        /// If there is no known asset type for the file, returns null.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private static AssetType? AssetTypeOfFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();

            switch (extension)
            {
                case ".js":
                    return AssetType.Script;

                case ".css":
                    return AssetType.StyleSheet;

                default:
                    return null;
            }
        }

        /// <summary>
        /// Converts a absolute file name (c:\...) to application relative url (~/....).
        /// This method won't work if the path to be converted is part of a virtual directory
        /// </summary>
        /// <param name="absolutePaths"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string AbsolutePathToApplicationRelativeUrl(string absolutePath)
        {
            var request = HttpContext.Current.Request;
            string applicationPath = request.PhysicalApplicationPath;
            string virtualDir = request.ApplicationPath;

            if (virtualDir != "/") 
            {
                virtualDir = virtualDir + "/";
            }

            var applicationRelativeUrl = "~/" + absolutePath.Replace(applicationPath, "").Replace(@"\", @"/");
            return applicationRelativeUrl;
        }
    }
}
