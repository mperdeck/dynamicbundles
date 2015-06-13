using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Optimization;

namespace DynamicBundles
{
    public interface IDynamicBundleCollection
    {
        bool Exists(string bundleName);
        void Add(Bundle bundleName);
    }

    /// <summary>
    /// Encapsulates the MVC BundleCollection. The difference is that this class
    /// implements IBundleCollection, so it can be replaced by a mock 
    /// during unit testing.
    /// </summary>
    internal class DynamicBundleCollection : IDynamicBundleCollection
    {
        private BundleCollection _bundles = null;

        public DynamicBundleCollection(BundleCollection bundles)
        {
            _bundles = bundles;
        }

        /// <summary>
        /// Returns true if a bundle with the passed in name exists.
        /// </summary>
        public bool Exists(string bundleName)
        {
            return (_bundles.GetBundleFor(bundleName) != null);
        }

        /// <summary>
        /// Adds the given Bundle to the bundle collection
        /// </summary>
        public void Add(Bundle bundle)
        {
            _bundles.Add(bundle);
        }
    }
}
