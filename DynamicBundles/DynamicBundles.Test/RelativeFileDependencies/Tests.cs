using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DynamicBundles.Models;
using DynamicBundles;
using System.Collections.Generic;
using System.Web.Optimization;
using System.Linq;

namespace DynamicBundles.Test.RelativeFileDependencies
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void RelativeFileDependencies()
        {
            string[] assetDirs = 
            new [] {
                "~/Views/Account/Details",
                "~/Views/Shared/_Layout",
                "~/Views/Shared/_LayoutContainer"
            };

            string[][] expectedScriptFiles = 
            {
                new string[] {
                    "~/Views/Shared/_LayoutContainer/jquery-1.8.2.js"
                },
                new string[] {
                    "~/Views/Account/AccountDetailsAssets/AccountDetailsAssets.js"
                }
            };

            string[][] expectedStyleFiles = 
            {
                new [] {
                    "~/Views/Shared/_LayoutContainer/Site.css",
                    "~/Views/Shared/_Layout/_Layout.css"
                },
                new string[] {
                    "~/Views/Account/AccountDetailsAssets/AccountDetailsAssets.css"
                }
            };

            Tester.Test("RelativeFileDependencies", 
                        assetDirs, 
                        expectedScriptFiles, 
                        expectedStyleFiles);
        }
    }
}
