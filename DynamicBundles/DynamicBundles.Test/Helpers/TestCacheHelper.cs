using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DynamicBundles;

namespace DynamicBundles.Test
{
    public class TestCacheHelper : ICacheHelper
    {
        public T Get<T>(string cacheKey, Func<T> createItem, IEnumerable<string> directories)
        {
            T item = createItem();
            return item;
        }
    }
}
