using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DynamicBundles.TestWeb.Areas.TestArea.Controllers
{
    public class HomeController : Controller
    {

        [ChildActionOnly]
        public ActionResult MyPerson()
        {
            Person s = new Person
            {
                FirstName = "Joe Area",
                LastName = "Smith",
                HomeAddress = new HomeAddress
                {
                    Street = "Area Country Lane",
                    HouseNumber = 1145,
                    City = "Rochester"
                }
            };

            return PartialView(s);
        }

        //
        // GET: /TestArea/Home/

        public ActionResult Index()
        {
            return View();
        }

    }
}
