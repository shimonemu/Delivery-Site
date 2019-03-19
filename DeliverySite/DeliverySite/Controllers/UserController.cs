using DeliverySite.Dal;
using DeliverySite.Models;
using DeliverySite.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeliverySite.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SignUp(User usr)
        {
            if (ModelState.IsValid)
            {
                UserDal dal = new UserDal();

                List<User> obj =
                    (from x in dal.User
                     where x.UserName == usr.UserName || x.Id == usr.Id
                     select x).ToList<User>();
                
            
                UserViewModel mng = new UserViewModel();
                if (obj.Count() > 0)
                {
                    TempData["exist"] = "User Name is already exist";
                    return View("../Home/SignUp", usr);
                }

                //UserDal dal = new UserDal();
                
                
                dal.User.Add(usr);
                dal.SaveChanges();
                Session["UserName"] = usr.UserName;
                Session["UserLoggedIn"] = usr.UserName;

                return View("ConfirmSignUp", usr);
            }
            return View("../Home/SignUp", usr);
        }
    }
}