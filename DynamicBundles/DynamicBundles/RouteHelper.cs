using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using DynamicBundles.Models;

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
        /// * files with no Area, with controller Shared, and that live in a directory with a name starting with _Layout
        /// * files with no Area, with controller Shared
        /// * files with an Area, with controller Shared, and that live in a directory with a name starting with _Layout
        /// * files with an Area, with controller Shared
        /// * all other files.
        /// </summary>
        /// <param name="filePaths"></param>
        /// <returns></returns>
        public static List<List<string>> FilePathsSortedByRoute(List<string> filePaths)
        {
            var filePathsBySortKey = new ListsByKey<string, string>();

            foreach (string filePath in filePaths)
            {
                filePathsBySortKey.Add(filePath, FilePathSortKey(filePath));
            }

            var sortedListOfLists = filePathsBySortKey.GetListOfLists();
            sortedListOfLists.Sort((firstPair, nextPair) => firstPair.Key.CompareTo(nextPair.Key));

            List<List<string>> result = sortedListOfLists.Select(p=>p.Value).ToList();
            return result;
        }

        /// <summary>
        /// Takes a file path and returns a string formatted like this:
        /// 
        /// * files with no Area, with controller Shared, and that live in a directory with a name starting with _Layout:
        ///   @___@
        ///   
        /// * files with no Area, with controller Shared, but don't live in a directory with a name starting with _Layout:
        ///   @___@@
        ///   
        /// * files with an Area, with controller Shared, and that live in a directory with a name starting with _Layout
        ///   [Area]___@
        ///   
        /// * files with an Area, with controller Shared, but don't live in a directory with a name starting with _Layout:
        ///   [Area]___@@
        ///   
        /// * all other files, with no area
        ///   @___[Controller]
        ///   
        /// * all other files, with an area
        ///   [Area]___[Controller]
        /// 
        /// Note that alphabetically, @ sorts before any letter or digit.
        /// Also, @___@@ sorts after @___@.
        /// 
        /// The method assumes that the file path is root relative, and follows MVC conventions:
        /// 
        /// * path with an Area:
        ///   ~/Areas/TestArea/Views/home/index/indexspecific.css
        /// 
        /// * path without an Area:
        ///   ~/Views/home/index/indexspecific.css
        /// 
        /// If the file path doesn't adhere to this, the method returns 
        /// @
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string FilePathSortKey(string filePath)
        {
            if (!filePath.StartsWith("~/"))
            {
                throw new Exception(string.Format("FilePathSortKey - filePath {0} does not start with ~/", filePath));
            }

            string[] filePathComponents = filePath.Split(new char[] { '/' });

            // Note that the first element in filePathComponents will be the ~
            // The last element is the file name itself.

            string area = "@";
            int nbrComponents = filePathComponents.Length;
            int controllerIdx = 2;

            if (string.CompareOrdinal(filePathComponents[1], "Areas") == 0)
            {
                if ((nbrComponents < 6) || (string.CompareOrdinal(filePathComponents[3], "Views") != 0))
                {
                    return "@";
                }

                area = filePathComponents[2];
                controllerIdx = 4;
            }
            else if (string.CompareOrdinal(filePathComponents[1], "Views") == 0)
            {
                if (nbrComponents < 4)
                {
                    return "@";
                }
            }
            else
            {
                return "@";
            }

            string controller = filePathComponents[controllerIdx];
            if (string.CompareOrdinal(controller, "Shared") == 0)
            {
                controller = "@";
            }

            string postfix = "@";
            int postControllerDirectoryIdx = (controllerIdx + 1);
            if (nbrComponents > postControllerDirectoryIdx)
            {
                string postControllerDirectory = filePathComponents[postControllerDirectoryIdx];
                if (postControllerDirectory.ToLower().StartsWith("_layout"))
                {
                    postfix = "";
                }
            }

            string sortKey = area + "___" + controller + postfix;

            return sortKey;
        }
    }
}


