using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicBundles
{
    public class RouteHelper
    {

        /// <summary>
        /// Takes a list of paths and puts them in separate lists, by area and controller.
        /// So all files in Area "Admin" and Controller "Home" go into one list, whilst
        /// all files without Area and Controller "About" go into another list.
        /// 
        /// Within the lists, the original order of the files is preserved. So if 
        /// the input list contains two files in Area "Admin" and Controller "Home", they are 
        /// added to the result list in the same order as in the input list.
        /// 
        /// The list of lists is sorted as follows, to ensure that the caller can
        /// place library files (such as jQuery) ahead of other files in the bundles:
        /// 
        /// * files with no Area, with controller Shared, and that live in a directory called _Layout
        /// * files with no Area, with controller Shared
        /// * files with an Area, with controller Shared, and that live in a directory called _Layout
        /// * files with an Area, with controller Shared
        /// * all other files.
        /// </summary>
        /// <param name="filePaths"></param>
        /// <returns></returns>
        public static List<List<string>> FilePathsSortedByRoute(List<string> filePaths)
        {

            return null;
        }





    }
}
