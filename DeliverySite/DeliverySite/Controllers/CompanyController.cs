﻿using DeliverySite.Dal;
using DeliverySite.Models;
using DeliverySite.ModelView;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace DeliverySite.Controllers
{
    public class CompanyController : Controller
    {
        public static string companyCode;
        public static Company staticCompnay = new Company();
        // GET: Company
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewReviews()
        {
            ReviewDal reviewDal = new ReviewDal();
            ReviewViewModel revView = new ReviewViewModel();
            List<Review> companies =
                (from x in reviewDal.Review
                 where x.CompCode == staticCompnay.CompCode
                 select x).ToList<Review>();

            revView.reviews = companies;
      
            return View("ViewReviews",revView);
        }

        public ActionResult Login()
        {
            Company cmp = new Company();
            ViewBag.UserNow = "null";
            return View("CompanyLogin", cmp);
        }

        public ActionResult ContactManager()
        {
            ManagerDal managerDal = new ManagerDal();
            List<Manager> allManagers = managerDal.Manager.ToList<Manager>();
            ManagerViewModel mngViewModel = new ManagerViewModel();
            mngViewModel.managers = allManagers;
            mngViewModel.manager = new Manager();
            mngViewModel.company = staticCompnay;
            return View(mngViewModel);
        }

        public ActionResult ViewMonthlyReport()
        {
            OrderDal orderDal = new OrderDal();
            DateTime before30days = new DateTime();
            before30days = DateTime.Today.AddDays(-30);

            List<Order> last30DaysOrder =
                 (from x in orderDal.Order
                  where x.Date > before30days && x.Date <= DateTime.Today.Date && x.CompanyCode==staticCompnay.CompCode
                  select x).ToList<Order>();

            //List<Order> last30DaysOrder2 =
            //    (from x in orderDal.Order
            //     where ((DateTime.Compare(x.Date, before30days.Date) > 0) && (DateTime.Compare(x.Date, DateTime.Today.Date) < 0))||DateTime.Compare(x.Date, DateTime.Today.Date)==0
            //     select x).ToList<Order>();

            ReviewViewModel revModel = new ReviewViewModel();
            revModel.orders = last30DaysOrder;
            return View("ViewMonthlyReport",revModel);
        }

        public ActionResult SendMail(ManagerViewModel mngView,string sub,string txt)
        {
            ManagerDal mngDal = new ManagerDal();
            Manager mng = mngDal.Manager.Find(mngView.manager.Id);

            List<Manager> allManagers = mngDal.Manager.ToList<Manager>();
            ManagerViewModel mngViewModel = new ManagerViewModel();
            mngViewModel.managers = allManagers;
            mngViewModel.manager = new Manager();
            mngViewModel.company = staticCompnay;

            if (mng == null)
            {
                TempData["errorFindTheManager"] = "The Chosen Manager Was Not Found";
                return View("ContactManager", mngViewModel);
            }

            //For Tests
            if (sub == null && txt == null)
            {
                string subject = Request.Form["reason"];
                string text = Request.Form["textarea"];

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("kfir2037@gmail.com");
                mail.To.Add(mng.Mail);
                mail.Subject = subject;
                mail.Body = text;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("kfir2037", "0542666134");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);

                TempData["mailSent"] = "Your mail was sent. The manager will be in touch";
                ViewBag.Test = "test SUCCEEDED";
                return View("ContactManager", mngViewModel);
            }
            else
            {
                string subject = sub;
                string text = txt;

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("kfir2037@gmail.com");
                mail.To.Add(mng.Mail);
                mail.Subject = subject;
                mail.Body = text;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("kfir2037", "0542666134");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);

                ViewBag.Test = "test SUCCEEDED";
                TempData["mailSent"] = "Your mail was sent. The manager will be in touch";
                
                return View("ContactManager", mngViewModel);
            }
        }
        public ActionResult CheckLogin(Company cmp)
        {
            CompanyDal dal = new CompanyDal();
            List<Company> cmpList = (from x in dal.Company where x.CompCode == cmp.CompCode && x.Password == cmp.Password select x).ToList<Company>();
            if (cmpList.Count() == 0)
            {
                ViewBag.WarningMessage = "Login Failed.";

                return View("CompanyLogin", cmp);
            }
            else
            {
                companyCode = cmpList[0].CompCode;
                Session["CompanyLoggedIn"] = cmpList[0].CompName;
                Session["UserName"] = cmpList[0].CompName;
                Session["CompCode"] = cmpList[0].CompCode;
                staticCompnay = cmpList[0];
                return View("../Home/Index");
            }
        }

        public ActionResult ShowCompanies()
        {
            CompanyDal dal = new CompanyDal();
            CompanyViewModel cmp = new CompanyViewModel();

            List<Company> allCompanies =
                (from x in dal.Company
                 select x).ToList<Company>();
            cmp = new CompanyViewModel();
            cmp.companies = allCompanies;
            cmp.company = new Company();

            return View("ShowCompanies",cmp);
        }

        public ActionResult CompanyWindow()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        public ActionResult Details(string id)
        {
            ProductDal db = new ProductDal();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View("Details",product);
        }

        public ActionResult Edit(string id)
        {
            ProductDal db = new ProductDal();

            Product product = db.Product.Find(id);

            //Patient patient = db.Patient.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Test = "Test SUCCEEDED";

            return View(product);
        }

        [HttpPost]
        public ActionResult Edit([Bind(Include = "PrdName,price,CompCode,PrdId")] Product product)
        {
            ProductDal db = new ProductDal();

            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Company/EditProducts",product);
            }
            return View(product);
        }

        public ActionResult Delete(string id)
        {
            ProductDal db = new ProductDal();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Patientsssss/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ProductDal db = new ProductDal();
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();

            return RedirectToAction("../Company/EditProducts");
        }

        protected override void Dispose(bool disposing)
        {
            ProductDal db = new ProductDal();
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ConfirmChangePassword(string pass, string conpass, string userIdTest)
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
                    TempData["changePassword"] = "The Passwords you entered are not match";
                    return View("ChangePassword");
                }
            }
            else
            {
                CompanyDal dal1 = new CompanyDal();

                Company usr1 = new Company();
                usr1 = dal1.Company.Find(userIdTest);

                usr1.Password = pass;
                dal1.SaveChanges();
                ViewBag.TestChgPass = "Test Succeeded";
                return View("ChangePassword");
            }
            CompanyDal dal = new CompanyDal();

            Company comp = new Company();
            comp = dal.Company.Find(companyCode);

            comp.Password = password;
            dal.SaveChanges();
            TempData["changePassword"] = "Password has been changed!";
            return View("ChangePassword");
        }


        public ActionResult EditProducts(Product prd)
        {
            ProductDal dal = new ProductDal();

            List<Product> productsList =
                (from x in dal.Product
                 where x.CompCode == companyCode
                 select x).ToList<Product>();

            ProductViewModel viewModel = new ProductViewModel();
            viewModel.products = productsList;
            viewModel.product = new Product();
            viewModel.product = prd;

            return View(viewModel);
        }

        public ActionResult AddCompanyToDB(Company com)
        {
            CompanyViewModel cvm = new CompanyViewModel();
            Company obj = new Company();

            obj.CompCode = com.CompCode;
            obj.CompName = com.CompName;
            obj.Password = com.Password;


            CompanyDal dal = new CompanyDal();

            if (ModelState.IsValid)
            {
                    List<Company> id_exist_list =
                (from x in dal.Company
                 where x.CompCode == com.CompCode
                 select x).ToList<Company>();

                if (id_exist_list.Count() > 0)
                {
                    TempData["existComp"] = "Company Code is already exist";
                    cvm.companies = dal.Company.ToList<Company>();
                    cvm.company = com;
                    return View("../Company/AddCompany", cvm);
                }


                dal.Company.Add(com);
                dal.SaveChanges();
                TempData["AddNewCompanySuccess"] = "The new Company was Added";
                cvm.companies = dal.Company.ToList<Company>();
                cvm.company = new Company();
                ViewBag.Test = "Test SUCCEEDED";
                return View("../Company/AddCompany", cvm);

            }
            else
            {
                cvm.company = obj;
            }

            cvm.companies = dal.Company.ToList<Company>();
            TempData["AddNewCompanyFailure"] = "The new Company was not added";
            return View("../Company/AddCompany", cvm);
        }

        public ActionResult AddCompany()
        {
            CompanyDal dal = new CompanyDal();
            CompanyViewModel ViewModel = new CompanyViewModel();

            List<Company> allCompanies =
                (from x in dal.Company
                 select x).ToList<Company>();

            ViewModel.companies = allCompanies;
            ViewModel.company = new Company();

            return View("../Company/AddCompany", ViewModel);
        }
    }
}
