using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBundles.Models
{
    /// <summary>
    /// Stores lists of T by keys of type K.
    /// </summary>
    public class ListsByKey<T,K>
    {
        Dictionary<K, List<T>> store = new Dictionary<K, List<T>>();

        private Dictionary<K, List<T>> Store
        {
            get { return store; }
        }

        public ListsByKey()
        {
        }

        /// <summary>
        /// Appends another ListsByKey to this ListsByKey.
        /// This means that the lists of T of the other ListsByKey are appended
        /// to the lists in this ListsByKey, by key.
        /// 
        /// The order of the items in listsByKey is preserved.
        /// </summary>
        /// <param name="fileListsByAssetType"></param>
        public void Append(ListsByKey<T, K> listsByKey)
        {
            foreach (KeyValuePair<K, List<T>> kvp in listsByKey.Store)
            {
                AddRange(kvp.Value, kvp.Key);
            }
        }

        /// <summary>
        /// Adds an item to the list with the given key.
        /// </summary>
        public void Add(T item, K key)
        {
            if (!store.ContainsKey(key))
            {
                store[key] = new List<T>();
            }

            store[key].Add(item);
        }

        /// <summary>
        /// Adds a list of items to the list with the given key.
        /// </summary>
        public void AddRange(List<T> items, K key)
        {
            if (!store.ContainsKey(key))
            {
                store[key] = new List<T>();
            }

            store[key].AddRange(items);
        }

        /// <summary>
        /// Returns the list for the given key.
        /// </summary>
        /// <param name="AssetType"></param>
        /// <returns></returns>
        public List<T> GetList(K key)
        {
            if (!store.ContainsKey(key))
            {
                return new List<T>();
            }

            return store[key];
        }

        /// <summary>
        /// Returns all lists, by key.
        /// </summary>
        /// <returns></returns>
        public List<KeyValuePair<K, List<T>>> GetListOfLists()
        {
            return store.ToList();
        }


    }
}
