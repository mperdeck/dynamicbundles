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
       private static DynamicBundlesBuilder _dynamicBundlesBuilder =
           new DynamicBundlesBuilder(new DynamicBundleCollection(BundleTable.Bundles), 
                                    new CacheHelper(),
                                    new BundleFactories());

        /// <summary>
        /// Called when a view file is being processed, such as the main view file, partial views and layout views.
        /// </summary>
        /// <param name="viewVirtualPath">
        /// Root relative path to the view.
        /// </param>
        public static void InitializePage(string viewVirtualPath)
        {
            // Store the directory that the view file lives in
            HttpContextStore.AddAssetDirectory(new AssetPath(Path.GetDirectoryName(viewVirtualPath), HttpContext.Current.Server.MapPath));
        }

        /// <summary>
        /// ExecutePageHierarchy is executed after all InitializePage calls,
        /// but before the bundles are created.
        /// </summary>
        public static void ExecutePageHierarchy()
        {
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
        /// <remarks>
        /// When this method is called, you have to add any bundles that need to be added,
        /// so the ones that need to be rendered at the top of the page can be rendered here.
        /// That is, when this runs, the time for gathering file dependencies is over and you need to
        /// process them into bundles.
        /// </remarks>
        public static IHtmlString DynamicBundlesTopRender()
        {
            if (!HttpContextStore.FirstTime()) { return new HtmlString(""); }

            List<AssetPath> assetDirectoryList = HttpContextStore.GetAssetDirectories();
            List<string> scriptBundleVirtualPaths;
            List<string> styleBundleVirtualPaths;

            _dynamicBundlesBuilder.Builder(assetDirectoryList, out scriptBundleVirtualPaths, out styleBundleVirtualPaths);

            HttpContextStore.StoreBottomBundleNames(scriptBundleVirtualPaths);

            // Note that Styles.Render assumes that all the bundles are StyleBundles
            return Styles.Render(styleBundleVirtualPaths.ToArray());
        }

        /// <summary>
        /// Same as DynamicBundlesTopRender, but to be used near the bottom of the page
        /// (would render script bundles).
        /// </summary>
        /// <returns></returns>
        public static IHtmlString DynamicBundlesBottomRender()
        {
            // Note that this assumes that all the bundles are ScriptBundles
            return Scripts.Render(HttpContextStore.GetBottomBundleNames().ToArray());
        }
    }
}
