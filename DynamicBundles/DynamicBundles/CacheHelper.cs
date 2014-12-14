using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace DynamicBundles
{
    public interface ICacheHelper
    {
        T Get<T>(string cacheKey, Func<T> createItem, IEnumerable<string> directories);
    }

    public class CacheHelper : ICacheHelper
    {
        private static object lockThis = new Object();

        /// <summary>
        /// Gets an item from cache. 
        /// If the item is not in cache, creates it and stores it, with a dependency on a collection of directories -
        /// so if a file is added/deleted or changed, the cache item is removed from cache.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the item.
        /// </typeparam>
        /// <param name="cacheKey">
        /// Cache key.
        /// </param>
        /// <param name="createItem">
        /// Lambda that creates the item.
        /// </param>
        /// <param name="directories">
        /// New cache item is dependend on these directories.
        /// </param>
        /// <returns></returns>
        public T Get<T>(string cacheKey, Func<T> createItem, IEnumerable<string> directories)
        {
            T item = (T)HttpContext.Current.Cache[cacheKey];

            if (item == null)
            {
                lock(lockThis)
                {
                    item = (T)HttpContext.Current.Cache[cacheKey];
                    if (item == null)
                    {
                        item = createItem();
                        HttpContext.Current.Cache.Insert(cacheKey, item, new CacheDependency(directories.ToArray()));
                    }
                }
            }

            return item;
        }
    }
}
