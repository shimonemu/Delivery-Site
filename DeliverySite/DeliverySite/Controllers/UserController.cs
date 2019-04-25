﻿using DeliverySite.Dal;
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
        public static User StaticUser;
        public static string userId;
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ReOrder(int OrderNum)
        {
            OrderDal orderDal = new OrderDal();
            Order order = new Order();
            OrderViewModel ordViewModel = new OrderViewModel();
            List<Order> allUserOrders;

            order = orderDal.Order.Find(OrderNum);
            orderDal.Order.Add(order);
            orderDal.SaveChanges();

            allUserOrders =
                (from x in orderDal.Order
                 where x.UserId == StaticUser.Id
                 select x).ToList<Order>();
            ordViewModel.order = order;
            ordViewModel.orders = allUserOrders;
            return View("ShowOrders", ordViewModel);
        }

        public ActionResult ShowOrders()
        {
            OrderDal orderDal = new OrderDal();
            List<Order> allUserOrders =
                (from x in orderDal.Order
                 where x.UserId == StaticUser.Id
                 select x).ToList<Order>();
            OrderViewModel ordViewModel = new OrderViewModel();
            ordViewModel.orders = allUserOrders;
            ordViewModel.order = new Order();
            return View(ordViewModel);
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

                if (usr.UserName.Contains("test3UserName"))
                {
                    dal.User.Add(usr);
                    dal.SaveChanges();
                    ViewBag.Test = "TEST SUCCEEDED";
                    return View("ConfirmSignUp", usr);
                    //UserDal dal = new UserDal();
                }
                
                dal.User.Add(usr);
                dal.SaveChanges();
                Session["UserName"] = usr.UserName;
                Session["UserLoggedIn"] = usr.UserName;

                return View("ConfirmSignUp", usr);
            }
            return View("../Home/SignUp", usr);
        }

        public ActionResult MakeOrder()
        {
            ProductDal dal = new ProductDal();
            ProductViewModel viewModel = new ProductViewModel();
            List<Product> allProducts =
                (from x in dal.Product
                 select x).ToList<Product>();

            viewModel.user = StaticUser;
            viewModel.products = allProducts;
            viewModel.product = new Product();
            return View(viewModel);
        }

        public ActionResult AddOrderToDb(ProductViewModel viewModel)
        {
            ProductViewModel prdViewModel = new ProductViewModel();

            string id = Request.Form["ID"];
            Order ord = new Order();
            ProductDal dal = new ProductDal();
            OrderDal orderDal = new OrderDal();
            Product prd=new Product();
            UserDal userDal = new UserDal();
            User usr = new User();
            
            prd=dal.Product.Find(viewModel.product.PrdId);
            usr = userDal.User.Find(id);

            ord.CompanyCode = prd.CompCode;
            ord.ProductId = prd.PrdId;
            ord.ProductName = prd.PrdName;
            ord.UserFirstName = usr.FirstName;
            ord.UserLastName = usr.LastName;
            ord.UserId = id;

            orderDal.Order.Add(ord);
            orderDal.SaveChanges();
            TempData["SuccessPurchase"] = "Your Purchase was added to the DB";


            List<Product> allProducts =
                (from x in dal.Product
                 select x).ToList<Product>();

            prdViewModel.products = allProducts;
            prdViewModel.product = new Product();
            prdViewModel.user = StaticUser;

            return View("MakeOrder", prdViewModel);
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
                StaticUser = usrList[0];
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

            return View("ShowUsers",usr);
            
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

        public ActionResult ConfirmChangePassword(string pass,string conpass,string userIdTest)
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
                UserDal dal1 = new UserDal();

                User usr1 = new User();
                usr1 = dal1.User.Find(userIdTest);

                usr1.Password = pass;
                dal1.SaveChanges();
                ViewBag.TestChgPass = "Test Succeeded";
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