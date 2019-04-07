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
    public class CompanyController : Controller
    {
        // GET: Company
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            Company cmp = new Company();
            ViewBag.UserNow = "null";
            return View("CompanyLogin", cmp);
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
                Session["CompanyLoggedIn"] = cmpList[0].CompName;
                Session["UserName"] = cmpList[0].CompName;
                Session["CompCode"] = cmpList[0].CompCode;
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

            return View(cmp);

        }

        public ActionResult CompanyWindow()
        {
            return View();
        }
    }
}
