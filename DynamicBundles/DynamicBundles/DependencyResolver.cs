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
    public interface IDependencyResolver
    {
        FileListsByAssetType GetRequiredFilesForDirectory(AssetPath dirPath);
    }

    public class DependencyResolver : IDependencyResolver
    {
        ICacheHelper _cacheHelper = null;

        public DependencyResolver(ICacheHelper cacheHelper)
        {
            _cacheHelper = cacheHelper;
        }

        /// <summary>
        /// Takes the path to a directory and returns the files in that directory, the files in the parent directories (down to the Views or ~ dir), 
        /// and all the files
        /// that that directory depends on (via .nuspec files).
        /// 
        /// It does not go into sub directories.
        /// 
        /// Uses caching to reduce trips to the file system.
        /// </summary>
        /// <param name="dirPath">
        /// Path to the directory. 
        /// </param>
        /// <returns>
        /// The required files, split by asset type.
        /// </returns>
        public FileListsByAssetType GetRequiredFilesForDirectory(AssetPath dirPath)
        {
            string absolutePath = dirPath.AbsolutePath;
            var fileListsByAssetType = _cacheHelper.Get(absolutePath, () => GetRequiredFilesForDirectoryUnchached(dirPath), new[] { absolutePath });
            return fileListsByAssetType;
        }

        /// <summary>
        /// Same as GetRequiredFilesForDirectory, but uncached.
        /// </summary>
        /// <param name="dirPath">
        /// Path to the directory. 
        /// </param>
        /// <returns></returns>
        private FileListsByAssetType GetRequiredFilesForDirectoryUnchached(AssetPath dirPath)
        {
            FileListsByAssetType fileListsByAssetType = new FileListsByAssetType();

            // Note: parentDirs will contain dirPath itself.
            //
            // dirPath.ParentDirs will give you something like
            /// ~/Views/Shared/EditorTemplates/HomeAddress
            /// ~/Views/Shared/EditorTemplates
            /// ~/Views/Shared
            // However, the longer directory tends to have the more specific files. If there is a dependency between files in these 
            // directories, it would be from more specific to less specific, not the other way around. So process the directories in
            // reverse order, so CSS and JS files in more common directories are loaded first.

            IEnumerable<AssetPath> parentDirs = dirPath.ParentDirs("Views").Reverse();
            foreach (AssetPath parentDir in parentDirs)
            {
                AddRequiredFilesSingleDirectory(fileListsByAssetType, parentDir);
            }

            return fileListsByAssetType;
        }

        /// <summary>
        /// Adds all files in a single directory to a fileListsByAssetType
        /// </summary>
        /// <param name="fileListsByAssetType"></param>
        /// <param name="dirPath"></param>
        private void AddRequiredFilesSingleDirectory(FileListsByAssetType fileListsByAssetType, AssetPath dirPath)
        {
            string absolutePath = dirPath.AbsolutePath;
            IEnumerable<string> filePaths = Directory.EnumerateFiles(absolutePath);

            // Need to first process the nuspec files, and then the asset files.
            // This because the asset files may depend on the files pointed at by the nuspec files.
            for (int i = 0; i < 2; i++)
            {
                foreach (string filePath in filePaths)
                {
                    if (i == 0)
                    {
                        if (NuspecFile.IsNuspecFile(filePath))
                        {
                            fileListsByAssetType.Append(GetDependencies(filePath, dirPath));
                        }
                    } 
                    else
                    {
                        AssetType? assetType = AssetTypeOfFile(filePath);
                        if (assetType != null)
                        {
                            fileListsByAssetType.Add(dirPath.AbsolutePathToAssetPath(filePath), assetType.Value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Reads the dependencies in a Nuspec file. These are directories.
        /// Accumulates the assets in those directories to in a FileListsByAssetType.
        /// This is then returned.
        /// 
        /// This method calls the dependency resolver concurrently.
        /// </summary>
        /// <param name="absoluteNuspecPath">
        /// Path of the nuspec file
        /// </param>
        /// <param name="nuspecFileDirPath">
        /// The directory where the nuspec file is located.
        /// </param>
        /// <returns></returns>
        private FileListsByAssetType GetDependencies(string absoluteNuspecPath, AssetPath nuspecFileDirPath)
        {
            FileListsByAssetType fileListsByAssetType = new FileListsByAssetType();

            var nuspecFile = new NuspecFile(absoluteNuspecPath);
            List<AssetPath> dependencyAssetPaths = nuspecFile.DependencyIds.Select(d => nuspecFileDirPath.Create(d)).ToList();

            // Call the dependency resolver concurrently for each dependency
            foreach (AssetPath dependencyAssetPath in dependencyAssetPaths)
            {
                fileListsByAssetType.Append(GetRequiredFilesForDirectory(dependencyAssetPath));
            }

            return fileListsByAssetType;
        }

        /// <summary>
        /// Takes a file path, and returns the asset type of that file.
        /// If there is no known asset type for the file, returns null.
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private AssetType? AssetTypeOfFile(string filePath)
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
    }
}
