using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Optimization;

namespace DynamicBundles.Test
{
    internal class Tester
    {
        public static void Test(string testRootDir, string[] assetDirs, string[][] expectedScriptFiles, string[][] expectedStyleFiles)
        {
            var log = new Dictionary<string, string[]>();

            Dictionary<string, Bundle> bundles = new Dictionary<string, Bundle>();

            var _dynamicBundlesBuilder =
                 new DynamicBundlesBuilder(new TestDynamicBundleCollection(bundles),
                                         new TestCacheHelper(),
                                         new BundleFactories(log));

            var pathHelper = new TestPathHelper(testRootDir);
            Func<string, string> rootToAbsolutePathFunc = (rootRelativePath) => pathHelper.rootToAbsolutePath(rootRelativePath);

            List<AssetPath> assetDirectoryList =
                assetDirs.Select(a => new AssetPath(a, rootToAbsolutePathFunc)).ToList();

            List<string> scriptBundleVirtualPaths;
            List<string> styleBundleVirtualPaths;

            _dynamicBundlesBuilder.Builder(assetDirectoryList, out scriptBundleVirtualPaths, out styleBundleVirtualPaths);

            string[][] actualScriptFiles = scriptBundleVirtualPaths.Select(n => log[n]).ToArray();
            string[][] actualStyleFiles = styleBundleVirtualPaths.Select(n => log[n]).ToArray();

            ArrayEqualityTesters.AssertTwoDimStringArraysEqual(actualScriptFiles, expectedScriptFiles);
            ArrayEqualityTesters.AssertTwoDimStringArraysEqual(actualStyleFiles, expectedStyleFiles);
        }
    }
}
