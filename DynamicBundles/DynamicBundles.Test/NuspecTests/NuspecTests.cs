using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DynamicBundles;

namespace DynamicBundles.Test.NuspecTests
{
    [TestClass]
    public class NuspecTests
    {
        [TestMethod]
        public void PathIsNuspec()
        {
            Assert.IsTrue(NuspecFile.IsNuspecFile("~/Views/blah.nuspec"));
        }

        [TestMethod]
        public void PathIsNotNuspec()
        {
            Assert.IsFalse(NuspecFile.IsNuspecFile("~/Views/blah.xml"));
        }
        
        [TestMethod]
        public void EmptyNuspec()
        {
            TestNuspecFileDepencyIds("empty.nuspec", new string[] {});
        }

        [TestMethod]
        public void OneDependencyNuspec()
        {
            TestNuspecFileDepencyIds("onedependency.nuspec", new string[] { "~/View/Library" });
        }

        [TestMethod]
        public void MultipleDependenciesNuspec()
        {
            TestNuspecFileDepencyIds("multipledependencies.nuspec", 
                                        new string[] { "~/Views/Library", "~/Packages" });
        }

        [TestMethod]
        public void NugetNuspec()
        {
            TestNuspecFileDepencyIds("nuget.nuspec",
                                        new string[] { "Common.Logging", "WebActivatorEx" });
        }

        /// <summary>
        /// Ensures that after a nuspec file is loaded in memory, it has the expected dependency ids.
        /// </summary>
        /// <param name="nuspecfileName">
        /// Nuspec file to test
        /// </param>
        /// <param name="expectedIds">
        /// Expected dependency ids.
        /// </param>
        private void TestNuspecFileDepencyIds(string nuspecfileName, IEnumerable<string> expectedIds)
        {
            ArrayEqualityTesters.AssertOneDimStringArraysEqual(NuspecFileDepencyIds(nuspecfileName).ToArray(), expectedIds.ToArray());
        }

        private IEnumerable<string> NuspecFileDepencyIds(string nuspecfileName)
        {
            var pathHelper = new TestPathHelper("NuspecTests/NuspecFiles");
            string nuspecAbsolutePath = pathHelper.rootToAbsolutePath("~/" + nuspecfileName);

            var nuspecFile = new NuspecFile(nuspecAbsolutePath);
            return nuspecFile.DependencyIds;
        }
    }
}
