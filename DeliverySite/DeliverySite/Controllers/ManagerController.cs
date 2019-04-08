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
    public class ManagerController : Controller
    {
        public static string managerId;

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
                managerId = mngList[0].Id;
                Session["ManagerLoggedIn"] = mng.UserName;
                Session["UserName"] = mng.UserName;
                return View("../Home/Index");
            }
        }

        

        public ActionResult ManagerWindow()
        {
            return View();
        }




        public ActionResult ChangePassword()
        {            
            return View();
        }

        public ActionResult EditCompanies()
        {
            CompanyDal dal = new CompanyDal();
            CompanyViewModel viewModel = new CompanyViewModel();

            List<Company> allCompanies =
                (from x in dal.Company
                 select x).ToList<Company>();

            viewModel.companies = allCompanies;
            viewModel.company = new Company();

            return View(viewModel);
        }

        public ActionResult Delete(string id)
        {
            CompanyDal db = new CompanyDal();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Company company = db.Company.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Patientsssss/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CompanyDal db = new CompanyDal();
            Company company = db.Company.Find(id);
            db.Company.Remove(company);
            db.SaveChanges();
            return RedirectToAction("../Manager/EditCompanies");
        }

        protected override void Dispose(bool disposing)
        {
            CompanyDal db = new CompanyDal();
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ConfirmChangePassword()
        {
            string password = Request.Form["password"];
            string confirmPassword = Request.Form["confirmPassword"];
            if (password != confirmPassword)
            {
                TempData["changePassword"] = "The passwords you entered are not match";
                return View("ChangePassword");
            }
            ManagerDal dal = new ManagerDal();

            Manager mng = new Manager();
            mng = dal.Manager.Find(managerId);

            mng.Password = password;
            dal.SaveChanges();
            TempData["changePassword"] = "Password has been changed!";
            return View("ChangePassword");
        }


    }
}