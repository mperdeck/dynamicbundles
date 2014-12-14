using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;
using BundleTransformer.Core.Transformers;

namespace DynamicBundles
{
    internal class BundleHelper
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
        public static List<string> AddFileListsAsBundles(IDynamicBundleCollection bundles,
                                                    List<List<AssetPath>> fileLists,
                                                    Func<string, string[], Bundle> bundleFactory)
        {
            var bundleNames = new List<string>();
            foreach (List<AssetPath> fileList in fileLists)
            {
                if (fileList.Count > 0)
                {

                    // You can optimise things by doing the deduping inside HashCodeForList, because that
                    // represents the strings as integers (hash codes), which are faster to dedupe.

                    IEnumerable<string> filePathsList = fileList.Select(f => f.RootRelativePath);

                    // Reverse the order of the files. Files that have been added later tend to be more generic
                    // (such as files in _Layout are added before those in _LayoutContainer). This way, files that depend on the 
                    // more generic files are loaded later.
                    string[] dedupedFileRootRelativePaths = filePathsList.Distinct().Reverse().ToArray();

                    string listHashCode = StringListHelper.HashCodeForList(dedupedFileRootRelativePaths);
                    string bundleName = "~/" + listHashCode;

                    if (!bundles.Exists(bundleName))
                    {
                        Bundle newBundle = bundleFactory(bundleName, dedupedFileRootRelativePaths);
                        bundles.Add(newBundle);
                    }

                    bundleNames.Add(bundleName);
                }
            }

            return bundleNames;
        }
    }
}
