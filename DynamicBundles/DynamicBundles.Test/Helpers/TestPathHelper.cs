using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace DynamicBundles.Test
{
    public class TestPathHelper
    {
        private string _testDirectory;

        public TestPathHelper(string testDirectory)
        {
            _testDirectory = testDirectory;
        }

        /// <summary>
        /// Converts a root relative path to an absolute path
        /// </summary>
        /// <param name="rootRelativePath"></param>
        /// <returns></returns>
        public string rootToAbsolutePath(string rootRelativePath)
        {
            // Note that this will end in \bin\Debug
            string absoluteAssemblyDir = Path.GetDirectoryName(typeof(TestPathHelper).Assembly.Location);

            string absoluteProjectDir = Path.GetDirectoryName(Path.GetDirectoryName(absoluteAssemblyDir));

            string absoluteTestDir = Path.Combine(absoluteProjectDir, _testDirectory);

            // rootRelativePath must start with ~/. The tilde will be replaced by the test dir.
            string absolutePath = Path.Combine(absoluteTestDir, rootRelativePath.Substring(2).Replace('/', '\\'));

            return absolutePath;
        }
    }
}
