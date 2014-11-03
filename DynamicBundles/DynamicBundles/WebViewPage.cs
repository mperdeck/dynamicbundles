using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;
using System.Web;
using System.IO;
using DynamicBundles.Models;

namespace DynamicBundles
{
    /// <summary>
    /// All view classes inherit either this class, or WebViewPage<T>
    /// </summary>
    public abstract class WebViewPage: System.Web.Mvc.WebViewPage
    {
        protected override void InitializePage()
        {
            WebViewPageStatic.InitializePage(this.VirtualPath);
            base.InitializePage();
        }

        public override void ExecutePageHierarchy()
        {
            WebViewPageStatic.ExecutePageHierarchy();
            base.ExecutePageHierarchy();
        }

        public static IHtmlString DynamicBundlesTopRender()
        {
            return WebViewPageStatic.DynamicBundlesTopRender();
        }

        public static IHtmlString DynamicBundlesBottomRender()
        {
            return WebViewPageStatic.DynamicBundlesBottomRender();
        }
    }

    public abstract class WebViewPage<T>: System.Web.Mvc.WebViewPage<T>
    {
        protected override void InitializePage()
        {
            WebViewPageStatic.InitializePage(this.VirtualPath);
            base.InitializePage();
        }

        public override void ExecutePageHierarchy()
        {
            WebViewPageStatic.ExecutePageHierarchy();
            base.ExecutePageHierarchy();
        }

        public static IHtmlString DynamicBundlesTopRender()
        {
            return WebViewPageStatic.DynamicBundlesTopRender();
        }

        public static IHtmlString DynamicBundlesBottomRender()
        {
            return WebViewPageStatic.DynamicBundlesBottomRender();
        }
    }

    internal static class WebViewPageStatic
    {
        /// <summary>
        /// Called when a view file is being processed, such as the main view file, partial views and layout views.
        /// </summary>
        /// <param name="viewVirtualPath">
        /// Root relative path to the view.
        /// </param>
        public static void InitializePage(string viewVirtualPath)
        {
            // Store the directory that the view file lives in
            HttpContextStore.AddAssetDirectory(Path.GetDirectoryName(viewVirtualPath));
        }

        /// <summary>
        /// ExecutePageHierarchy is executed after all InitializePage calls,
        /// but before the bundles are created.
        /// </summary>
        public static void ExecutePageHierarchy()
        {
            if (!HttpContextStore.FirstTime()) { return; }

            List<string> assetDirectoryList = HttpContextStore.GetAssetDirectories();

            var fileListsByAssetType = new FileListsByAssetType();

            foreach (string assetDirectory in assetDirectoryList)
            {
                FileListsByAssetType requiredFilesByAssetType = DependencyResolver.GetRequiredFilesForDirectory(assetDirectory);
                fileListsByAssetType.Append(requiredFilesByAssetType);
            }

            // fileListsByAssetType now contains all required files by asset type

            List<string> styleBundleVirtualPaths = CreateBundles(fileListsByAssetType, AssetType.StyleSheet, BundleHelper.StyleBundleFactory);
            HttpContextStore.StoreBottomBundleNames(styleBundleVirtualPaths);

            List<string> scriptBundleVirtualPaths = CreateBundles(fileListsByAssetType, AssetType.Script, BundleHelper.ScriptBundleFactory);
            HttpContextStore.StoreTopBundleNames(scriptBundleVirtualPaths);
        }

        private static List<string> CreateBundles(FileListsByAssetType fileListsByAssetType, AssetType assetType, Func<string, Bundle> bundleFactory)
        {
            List<String> styleSheetFiles = fileListsByAssetType.GetFilesList(AssetType.StyleSheet);
            List<List<string>> styleSheetFilesByAreaController = RouteHelper.FilePathsSortedByRoute(styleSheetFiles);
            List<string> bundleVirtualPaths = BundleHelper.AddFileListsAsBundles(BundleTable.Bundles, styleSheetFilesByAreaController, bundleFactory);
            return bundleVirtualPaths;
        }

        /// <summary>
        /// Call this method from the top level _Layout view, in the head section.
        /// It replaces the normal 
        /// @Styles.Render("....") and
        /// @Scripts.Render("....")
        /// lines that sit near the top of the page.
        /// 
        /// It renders all the bundles that the page needs (these can be different per page) and need to go 
        /// near the top (mainly style bundles).
        /// </summary>
        /// <returns></returns>
        public static IHtmlString DynamicBundlesTopRender()
        {
            return Styles.Render(HttpContextStore.GetTopBundleNames().ToArray());
        }

        /// <summary>
        /// Same as DynamicBundlesTopRender, but to be used near the bottom of the page
        /// (would render script bundles).
        /// </summary>
        /// <returns></returns>
        public static IHtmlString DynamicBundlesBottomRender()
        {
            return Styles.Render(HttpContextStore.GetBottomBundleNames().ToArray());
        }
    }
}
