using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynamicBundles.TestWeb.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View("Index/Index");
        }

        public ActionResult About()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult MyChild()
        {
            string s = "xyz";
            return PartialView("MyChild", s);
        }

        [ChildActionOnly]
        public ActionResult MyPerson()
        {
            Person s = new Person { 
                FirstName = "Joe", 
                LastName = "Smith", 
                HomeAddress = new HomeAddress { 
                    Street = "Country Lane",
                    HouseNumber = 45,
                    City = "Rochester"
                } 
            };
            
            return PartialView(s);
        }

    }
}
