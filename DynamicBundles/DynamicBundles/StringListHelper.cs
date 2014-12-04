using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBundles
{
    public static class StringListHelper
    {
        /// <summary>
        /// Returns a string that is unique to the given list of strings.
        /// Order and casing are not relevant here, so (a.css, b.css) and (B.css, a.css)
        /// give the same code.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        /// <remarks>
        /// See http://stackoverflow.com/questions/670063/getting-hash-of-a-list-of-strings-regardless-of-order
        /// </remarks>
        public static string HashCodeForList(IEnumerable<string> list)
        {
           List<int> codes = new List<int>();
           foreach (string item in list) 
           {
              codes.Add(item.ToLower().GetHashCode());
           }

           codes.Sort();
           long hash = 0;

           foreach (int code in codes) 
           {
              unchecked {
                 hash *= 251; // multiply by a prime number
                 hash += code; // add next hash code
              }
           }

           return hash.ToString("X");
        }
    }
}
