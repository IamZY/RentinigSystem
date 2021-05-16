using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace My.RentingSystem.WebApp.Controllers
{
    public class AboutController : BaseController
    {
        //
        // GET: /About/

        public ActionResult Index()
        {
            Session["city"] = Session["rentCity"];
            return View();
        }

    }
}
