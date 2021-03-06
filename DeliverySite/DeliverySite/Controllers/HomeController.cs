﻿using DeliverySite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeliverySite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SignOut()
        {
            Session["UserName"] = null;
            Session["ManagerLoggedIn"] = null;
            Session["UserLoggedIn"] = null;
            Session["CompanyLoggedIn"] = null;
            Session["UserId"] = null;
            Session["BigFont"] = null;
            

            return View("Index");
        }

        public ActionResult Login()
        {

            ViewBag.Message = "ManagerLogin";
            if (Session["UserName"] != null)
            {
                ViewBag.UserName = Session["UserName"];
                return RedirectToAction("ManagerWindow", "Manager", new { username = Session["UserName"].ToString() });
            }
            else
            {
                return View();
            }
        }

        public ActionResult SignIn()
        {
            return View();
        }

        public ActionResult SignUp()
        {
            User usr = new User();
            usr = new User();


            return View(usr);
        }

        //public ActionResult Submit()
        //{
        //    Doctor doc = new Doctor();
        //    doc.Id = Request.Form["id"];
        //    doc.Password = Request.Form["pass"];
        //    return View("~/Views/Manager/ManagerWindow.cshtml", doc);

        //}
    }
}