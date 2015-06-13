using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DynamicBundles.Models;
using DynamicBundles;
using System.Collections.Generic;
using System.Web.Optimization;
using System.Linq;

namespace DynamicBundles.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CheckBundle()
        {
            string[] assetDirs = 
            new [] {
                "~/Views/Account",
                "~/Views/Shared/_Layout",
                "~/Views/Shared/_LayoutContainer"
            };

            string[][] expectedScriptFiles = 
            {
                new [] {
                    "~/Views/Shared/_LayoutContainer/SharedCode.js",
                    "~/Views/Shared/_LayoutContainer/jquery-1.8.2.js"
                }
            };

            string[][] expectedStyleFiles = 
            {
                new [] {
                    "~/Views/Shared/_LayoutContainer/Site.css",
                    "~/Views/Shared/_LayoutContainer/Reset.css",
                    "~/Views/Shared/_Layout/_Layout.css"
                }
            };

            Tester.Test("AssetsInLayoutOnly", 
                        assetDirs, 
                        expectedScriptFiles, 
                        expectedStyleFiles);
        }
    }
}
