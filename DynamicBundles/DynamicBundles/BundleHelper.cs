using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace DynamicBundles
{
    public class BundleHelper
    {
        /// <summary>
        /// Takes a list of lists of file paths. Each file path is root relative (starts with ~).
        /// Turns these lists of paths into bundles. Ensures each file path is added only once.
        /// Adds the bundles to the given bundle collection, but only if there is no bundle yet with the same files.
        /// Returns a list with the bundle paths, in the same order as the originating lists
        /// in the input list of lists.
        /// </summary>
        /// <param name="bundles"></param>
        /// <param name="fileLists"></param>
        /// <param name="bundleFactory">
        /// Used to create a new bundle object.
        /// </param>
        /// <returns></returns>
        public static List<string> AddFileListsAsBundles(BundleCollection bundles,
                                                    List<List<string>> fileLists,
                                                    Func<string, Bundle> bundleFactory)
        {
            //#########################
            return null;
        }

        public static ScriptBundle ScriptBundleFactory(string bundleVirtualPath)
        {
            return new ScriptBundle(bundleVirtualPath);
        }

        public static StyleBundle StyleBundleFactory(string bundleVirtualPath)
        {
            return new StyleBundle(bundleVirtualPath);
        }
    }
}
