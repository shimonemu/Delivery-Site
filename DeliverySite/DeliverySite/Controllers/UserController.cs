using DeliverySite.Dal;
using DeliverySite.Models;
using DeliverySite.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DeliverySite.Controllers
{
    public class UserController : Controller
    {

        public static string userId;
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
                    TempData["exist"] = "ID is already exist";
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

        public ActionResult Login()
        {
            User usr = new User();
            ViewBag.UserNow = "null";
            return View(usr);
        }
        
        public ActionResult CheckLogin(User usr)
        {
            UserDal dal = new UserDal();
            List<User> usrList = (from x in dal.User where x.UserName == usr.UserName && x.Password == usr.Password select x).ToList<User>();
            if (usrList.Count() == 0)
            {
                ViewBag.WarningMessage = "Login Failed.";
                
                return View("Login", usr);
            }
            else
            {
                userId = usrList[0].Id;
                Session["UserLoggedIn"] = usr.UserName;
                Session["UserName"] = usr.UserName;
                return View("../Home/Index");
            }
        }

        public ActionResult ShowUsers()
        {
            UserDal dal = new UserDal();
            UserViewModel usr = new UserViewModel();

            List<User> allUsers =
                (from x in dal.User
                 select x).ToList<User>();
            usr = new UserViewModel();
            usr.users = allUsers;
            usr.user = new User();

            return View(usr);
            
        }

        public ActionResult UserWindow()
        {
            return View();
        }

        public ActionResult Delete(string id)
        {
            UserDal db = new UserDal();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            UserViewModel usr = new UserViewModel();
            db.User.Remove(user);
            db.SaveChanges();

            List<User> users =
                (from x in db.User
                 select x).ToList<User>();
            usr.user = user;
            usr.users = users;
            //return View("ShowUsers",usr);
            return RedirectToAction("../User/ShowUsers",usr);

        }

        // POST: Patientsssss/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserDal db = new UserDal();
            User user = db.User.Find(id);
            db.User.Remove(user);
            db.SaveChanges();
            return RedirectToAction("../User/ShowUsers");
        }

        protected override void Dispose(bool disposing)
        {
            UserDal db = new UserDal();
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult ConfirmChangePassword()
        {
            string password = Request.Form["password"];
            string confirmPassword = Request.Form["confirmPassword"];

            if (password.Length < 8 || password.Length > 16 || confirmPassword.Length < 8 || confirmPassword.Length > 16)
            {
                TempData["NotGoodPass"] = "The Password have to be 8 to 16 chars long";
                return View("ChangePassword");
            }

            if (password != confirmPassword)
            {
                TempData["changePassword"] = "The passwords you entered are not match";
                return View("ChangePassword");
            }
            UserDal dal = new UserDal();

            User usr = new User();
            usr = dal.User.Find(userId);

            usr.Password = password;
            dal.SaveChanges();
            TempData["changePassword"] = "Password has been changed!";
            return View("ChangePassword");
        }

    }
}