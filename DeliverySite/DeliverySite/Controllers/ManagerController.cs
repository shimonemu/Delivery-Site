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
        public static Manager staticManager;
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

        public ActionResult AddNewManager()
        {
            ManagerDal dal = new ManagerDal();
            List<Manager> allManagers = dal.Manager.ToList<Manager>();
            ManagerViewModel cvm = new ManagerViewModel();
            cvm.manager = new Manager();
            cvm.managers = allManagers;
            return View(cvm);
        }

        public ActionResult AddManagerToDB(Manager doc)
        {
            ManagerViewModel cvm = new ManagerViewModel();
            Manager obj = new Manager();

            obj.FirstName = doc.FirstName;
            obj.LastName = doc.LastName;
            obj.Id = doc.Id;
            obj.Password = doc.Password;
            obj.UserName = doc.UserName;
            obj.Mail = doc.Mail;

            ManagerDal dal = new ManagerDal();

            if (ModelState.IsValid)
            {
                ManagerViewModel doc2 = new ManagerViewModel();

                if (dal.Manager.Find(obj.Id)!=null)
                {
                    TempData["existMng"] = "The given Id is already exist";
                    cvm.managers = dal.Manager.ToList<Manager>();
                    cvm.manager = doc;
                    return View("../Manager/AddNewManager", cvm);
                }
                dal.Manager.Add(doc);
                dal.SaveChanges();
                TempData["AddNewManagerSuccess"] = "The new Manager was Added";
                cvm.managers = dal.Manager.ToList<Manager>();
                cvm.manager = new Manager();
                return View("AddNewManager", cvm);

            }
            else
            {
                cvm.manager = obj;
            }

            cvm.managers = dal.Manager.ToList<Manager>();
            TempData["AddNewManagerFailure"] = "The new Manager was not added";
            return View("AddNewManager", cvm);
        }

        public ActionResult ShowOrders()
        {
            OrderDal dal = new OrderDal();
            List<Order> allOrders = dal.Order.ToList<Order>();
            OrderViewModel ordView = new OrderViewModel();
            ordView.orders = allOrders;
            ordView.order = new Order();
            return View(ordView);
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
                staticManager = mngList[0];
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

        public ActionResult ConfirmChangePassword(string pass, string conpass, string managerIdTest)
        {
            string password, confirmPassword;

            if (pass == null && conpass == null)
            {

                password = Request.Form["password"];
                confirmPassword = Request.Form["confirmPassword"];

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
            }

            else
            {
                ManagerDal dal1 = new ManagerDal();

                Manager usr1 = new Manager();
                usr1 = dal1.Manager.Find(managerIdTest);

                usr1.Password = pass;
                dal1.SaveChanges();
                ViewBag.TestChgPass = "Test Succeeded";
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