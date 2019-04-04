using DeliverySite.Dal;
using DeliverySite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeliverySite.Controllers
{
    public class ManagerController : Controller
    {
        // GET: Manager
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            Manager mng = new Manager();
            ViewBag.UserNow = "null";
            return View("ManagerLogin",mng);
        }

        public ActionResult CheckLogin(Manager mng)
        {
            ManagerDal dal = new ManagerDal();
            List<Manager> mngList = (from x in dal.Manager where x.UserName == mng.UserName && x.Password == mng.Password select x).ToList<Manager>();
            if (mngList.Count() == 0)
            {
                ViewBag.WarningMessage = "Login Failed.";

                return View("ManagerLogin", mng);
            }
            else
            {
                Session["ManagerLoggedIn"] = mng.UserName;
                Session["UserName"] = mng.UserName;
                return View("../Home/Index");
            }
        }

        public ActionResult ManagerWindow()
        {
            return View();
        }
    }
}