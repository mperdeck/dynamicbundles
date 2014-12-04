using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;
using BundleTransformer.Core.Transformers;

namespace DynamicBundles
{
    public class BundleHelper
    {
        /// <summary>
        /// Takes a list of lists of file paths. Each file path is root relative (starts with ~).
        /// Turns these lists of paths into bundles. Ensures each file path is added only once.
        /// 
        /// Adds the bundles to the given bundle collection, but only if there is no bundle yet with the same files,
        /// regardless of the order of the files or casing. That is, (a.css, b.css) is regarded as the same as (B.css, a.css).
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
                                                    List<List<AssetPath>> fileLists,
                                                    Func<string, Bundle> bundleFactory)
        {
            var bundleNames = new List<string>();
            foreach (List<AssetPath> fileList in fileLists)
            {
                if (fileList.Count > 0)
                {

                    // You can optimise things by doing the deduping inside HashCodeForList, because that
                    // represents the strings as integers (hash codes), which are faster to dedupe.

                    List<AssetPath> dedupedFilesList = fileList.Distinct().ToList();
                    string[] dedupedFileRootRelativePaths = dedupedFilesList.Select(f => f.RootRelativePath).ToArray();
                    string listHashCode = StringListHelper.HashCodeForList(dedupedFileRootRelativePaths);
                    string bundleName = "~/" + listHashCode;

                    if (bundles.GetBundleFor(bundleName) == null)
                    {
                        Bundle newBundle = bundleFactory(bundleName);
                        newBundle.Include(dedupedFileRootRelativePaths);
                        bundles.Add(newBundle);
                    }

                    bundleNames.Add(bundleName);
                }
            }

            return bundleNames;
        }

        public static ScriptBundle ScriptBundleFactory(string bundleVirtualPath)
        {
            var bundle = new ScriptBundle(bundleVirtualPath);
            //########## bundle.Transforms.Add(new CssTransformer());

            return new ScriptBundle(bundleVirtualPath);
        }

        public static StyleBundle StyleBundleFactory(string bundleVirtualPath)
        {
            return new StyleBundle(bundleVirtualPath);
        }
    }
}
