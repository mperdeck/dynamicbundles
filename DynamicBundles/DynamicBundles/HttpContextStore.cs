using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DynamicBundles
{
    /// <summary>
    /// Strongly typed access to the HttpContext.Items store, which holds
    /// per-request info.
    /// </summary>
    public static class HttpContextStore
    {
        private static readonly string AssetDirectoryItemKey = "__DynamicBundles_AssetDirectory";
        private static readonly string FirstTimeItemKey = "__DynamicBundles_FirstTime";
        private static readonly string TopBundleNamesItemKey = "__DynamicBundles_TopBundleNames";
        private static readonly string BottomBundleNamesItemKey = "__DynamicBundles_BottomBundleNames";

        /// <summary>
        /// Adds a directory with assets (script files, etc.) to the stored list of asset directories.
        /// </summary>
        /// <param name="dirPath"></param>
        public static void AddAssetDirectory(AssetPath dirPath)
        {
            List<AssetPath> assetDirectoryList = (List<AssetPath>)HttpContext.Current.Items[AssetDirectoryItemKey];
            if (assetDirectoryList == null)
            {
                assetDirectoryList = new List<AssetPath>();
                HttpContext.Current.Items[AssetDirectoryItemKey] = assetDirectoryList;
            }

            assetDirectoryList.Add(dirPath);
        }

        /// <summary>
        /// Gets the list of asset directories
        /// </summary>
        /// <returns></returns>
        public static List<AssetPath> GetAssetDirectories()
        {
            List<AssetPath> assetDirectoryList = (List<AssetPath>)HttpContext.Current.Items[AssetDirectoryItemKey];

            // There should be at least one asset directory - that of the main view.
            if (assetDirectoryList == null)
            {
                throw new Exception("GetAssetDirectories - assetDirectoryList is null");
            }

            return assetDirectoryList;
        }

        /// <summary>
        /// Returns true if this is the first time this method is called
        /// for this request. Returns false otherwise.
        /// </summary>
        /// <returns></returns>
        public static bool FirstTime()
        {
            object firstTimeSet = HttpContext.Current.Items[FirstTimeItemKey];

            if (firstTimeSet != null) { return false; }

            HttpContext.Current.Items[FirstTimeItemKey] = true;

            return true;
        }

        /// <summary>
        /// Stores the list with names of bundles that need to be rendered 
        /// near the top of the _Layout file.
        /// </summary>
        public static void StoreTopBundleNames(List<string> bundleNames)
        {
            HttpContext.Current.Items[TopBundleNamesItemKey] = bundleNames;
        }

        /// <summary>
        /// Gets the list with names of bundles that need to be rendered 
        /// near the top of the _Layout file.
        /// </summary>
        public static List<string> GetTopBundleNames()
        {
            List<string> bundleNames = (List<string>)HttpContext.Current.Items[TopBundleNamesItemKey];

            if (bundleNames == null)
            {
                throw new Exception("GetTopBundleNames - bundleNames is null");
            }

            return bundleNames;
        }

        /// <summary>
        /// Stores the list with names of bundles that need to be rendered 
        /// near the bottom of the _Layout file.
        /// </summary>
        public static void StoreBottomBundleNames(List<string> bundleNames)
        {
            HttpContext.Current.Items[BottomBundleNamesItemKey] = bundleNames;
        }

        /// <summary>
        /// Gets the list with names of bundles that need to be rendered 
        /// near the bottom of the _Layout file.
        /// </summary>
        public static List<string> GetBottomBundleNames()
        {
            List<string> bundleNames = (List<string>)HttpContext.Current.Items[BottomBundleNamesItemKey];

            if (bundleNames == null)
            {
                throw new Exception("GetBottomBundleNames - bundleNames is null");
            }

            return bundleNames;
        }
    }
}
