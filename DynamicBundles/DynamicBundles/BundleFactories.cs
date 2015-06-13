using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace DynamicBundles
{
    public class BundleFactories
    {
        private Dictionary<string, string[]> _log = null;

        public BundleFactories()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="log">
        /// A record of all bundle paths and the files in each bundle will be kept here.
        /// </param>
        public BundleFactories(Dictionary<string, string[]> log)
        {
            _log = log;
        }

        /// <summary>
        /// Creates a script bundle with the given path (= name) and files
        /// </summary>
        public ScriptBundle ScriptBundleFactory(string bundleVirtualPath, string[] fileRootRelativePaths)
        {
            return BundleWithPaths(new ScriptBundle(bundleVirtualPath), fileRootRelativePaths);
        }

        /// <summary>
        /// Creates a style bundle with the given path (= name) and files
        /// </summary>
        public StyleBundle StyleBundleFactory(string bundleVirtualPath, string[] fileRootRelativePaths)
        {
            return BundleWithPaths(new StyleBundle(bundleVirtualPath), fileRootRelativePaths);
        }

        private T BundleWithPaths<T>(T bundle, string[] fileRootRelativePaths) where T: Bundle
        {
            bundle.Include(fileRootRelativePaths);
            
            if (_log != null)
            {
                _log[bundle.Path] = fileRootRelativePaths;
            }

            return bundle;
        }
    }
}
