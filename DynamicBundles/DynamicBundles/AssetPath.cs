using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DynamicBundles
{
    /// <summary>
    /// Represents a path to a file or directory.
    /// That file or directory must live within the current project.
    /// 
    /// This is called "AssetPath" rather than "Path" to not clash with the .Net Path class.
    /// </summary>
    public class AssetPath
    {
        public string RootRelativePath { get; private set; }

        private Func<string, string> _rootToAbsolutePathFunc = null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rootRelativePath">
        /// Path to the file or directory. Must be relative to the root of the project (start with a ~).
        /// </param>
        /// <param name="rootToAbsolutePathFunc">
        /// Lambda that takes a root relative path and returns an absolute path.
        /// Does the same as HttpContext.Current.Server.MapPath
        /// </param>
        public AssetPath(string rootRelativePath, Func<string, string> rootToAbsolutePathFunc)
        {
            RootRelativePath = rootRelativePath;
            _rootToAbsolutePathFunc = rootToAbsolutePathFunc;
        }

        private string _absolutePath = null;
        public string AbsolutePath
        {
            get
            {
                if (_absolutePath == null)
                {
                    _absolutePath = _rootToAbsolutePathFunc(RootRelativePath);
                }

                return _absolutePath;
            }
        }

        /// <summary>
        /// The absolute counterpart of say "~/Views/Home" might be something like
        /// "d:\dev\Views\Home". In that case, the "absolute prefix" (the stuff that replaces the ~) is "d:\dev",
        /// which is 6 chars long.        ///
        /// </summary>
        private int? _absolutePathPrefixLength = null;
        private int AbsolutePathPrefixLength
        {
            get
            {
                if (_absolutePathPrefixLength == null)
                {
                    const int lengthTilde = 1;

                    _absolutePathPrefixLength =
                        AbsolutePath.Length - RootRelativePath.Length + lengthTilde;
                }

                return _absolutePathPrefixLength.Value;
            }
        }

        /// <summary>
        /// Converts a absolute file name or directory name (c:\...) to an AssetPath containing an application relative url (~/....).
        /// The file or directory must sit in the same project as the file or directory represented by this AssetPath object.
        /// Returns a new AssetPath representing the file or directory.
        /// </summary>
        /// <param name="absolutePaths"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public AssetPath AbsolutePathToAssetPath(string absolutePath)
        {
            string rootRelativePath = "~" + absolutePath.Substring(AbsolutePathPrefixLength).Replace('\\', '/');
            return new AssetPath(rootRelativePath, _rootToAbsolutePathFunc);
        }

        /// <summary>
        /// Create a new AssetPath with the same rootToAbsolutePathFunc as this AssetPath.
        /// 
        /// If the given path is not root relative, it is assumed to be relative to this AssetPath.
        /// </summary>
        public AssetPath Create(string path)
        {
            if (!path.StartsWith("~"))
            {
                return RootRelativeCombine(RootRelativePath, path);
            }

            return new AssetPath(path, _rootToAbsolutePathFunc);
        }

        private AssetPath RootRelativeCombine(string rootRelativePath, string relativePath)
        {
            // If relativePath contains .. (such as ..\dir), then rootRelativePathWithDots also has ..
            // This somehow leads to an endless loop.
            string rootRelativePathWithDots = Path.Combine(RootRelativePath, relativePath);

            string absolutePath = Path.GetFullPath(_rootToAbsolutePathFunc(rootRelativePathWithDots));
            AssetPath assetPath = AbsolutePathToAssetPath(absolutePath);
            return assetPath;
        }

        /// <summary>
        /// Returns a list with parent dirs of this file or directory, and the file or directory itself.
        /// If stopDirectory is given, it will stop when it hits that directory. Otherwise, it stops at ~
        /// 
        /// For example, suppose this object represents
        /// ~/Views/Shared/EditorTemplates/HomeAddress
        /// and the stopDirectory is 
        /// Views
        /// 
        /// Then this method returns
        /// ~/Views/Shared/EditorTemplates/HomeAddress
        /// ~/Views/Shared/EditorTemplates
        /// ~/Views/Shared
        /// </summary>
        /// <returns></returns>
        public IEnumerable<AssetPath> ParentDirs(string stopDirectory = "~")
        {
            List<AssetPath> parentDirs = new List<AssetPath>();
            string currentRootRelativePath = RootRelativePath;

            while (!currentRootRelativePath.EndsWith(stopDirectory))
            {
                parentDirs.Add(new AssetPath(currentRootRelativePath, _rootToAbsolutePathFunc));
                currentRootRelativePath = Path.GetDirectoryName(currentRootRelativePath);
            }

            return parentDirs;
        }
    }
}
