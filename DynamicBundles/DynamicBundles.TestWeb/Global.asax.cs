using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DynamicBundles.TestWeb
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //@@@@@@@@@@@@@@@@@@@@@
          //leave the normal razor engine       ViewEngines.Engines.Clear();



            // Add DynamicBundles view engine. This functions the same as the Razor view engine,
            // but can find views sitting in their own directories, such as ~/Views/Home/Index/Index.cshtml
            // Note: this leaves the other view engines in place, so they can still be used.
            ViewEngines.Engines.Add(new DynamicBundles.DynamicBundlesViewEngine()); 
            //@@@@@@@@@@@@@@@@@@@@@

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            
   //         BundleConfig.RegisterBundles(BundleTable.Bundles);
        }






        protected void Application_EndRequest()
        {
        }


    }
}