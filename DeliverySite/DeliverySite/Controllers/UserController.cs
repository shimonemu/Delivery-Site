using DeliverySite.Dal;
using DeliverySite.Models;
using DeliverySite.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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

        protected override void Dispose(bool disposing)
        {
            UserDal db = new UserDal();
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ContactCompany(string id)
        {
            CompanyDal compDal = new CompanyDal();
            Company comp = compDal.Company.Find(id);

            return View(comp);
        }

        public ActionResult SendMail(string cc,string sub,string txt)
        {
            //For Tests
            if (cc == null && sub == null && txt == null)
            {
                string compCode = Request.Form["ID"];
                string subject = Request.Form["reason"];
                string text = Request.Form["textarea"];

                CompanyDal compDal = new CompanyDal();
                Company comp = compDal.Company.Find(compCode);

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("kfir2037@gmail.com");
                mail.To.Add(comp.Mail);
                mail.Subject = subject;
                mail.Body = text;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("kfir2037", "0542666134");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);

                TempData["mailSent"] = "Your mail was sent. The Company will be in touch";
                ViewBag.Test = "test SUCCEDED";


                return View("ContactCompany", comp);
            }
            else
            {
                string compCode = cc;
                string subject =sub;
                string text = txt;

                CompanyDal compDal = new CompanyDal();
                Company comp = compDal.Company.Find(compCode);

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("kfir2037@gmail.com");
                mail.To.Add(comp.Mail);
                mail.Subject = subject;
                mail.Body = text;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("kfir2037", "0542666134");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);

                TempData["mailSent"] = "Your mail was sent. The Company will be in touch";
                ViewBag.Test = "test SUCCEDED";


                return View("ContactCompany", comp);
            }
        }

        public ActionResult ReOrder(int id)
        {
            OrderDal orderDal = new OrderDal();
            Order order = new Order();
            OrderViewModel ordViewModel = new OrderViewModel();
            List<Order> allUserOrders;

            order = orderDal.Order.Find(id);
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
            ord.price = prd.price;
            ord.Date = DateTime.Today.Date;

            //DateTime today = DateTime.Today;
            //DateTime sevenDaysEarlier = today.AddDays(-32);

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
                var bigFont = Request.Form["bigFont"];
                if (bigFont != null)
                {
                    Session["BigFont"] = "BigFont";
                }
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

        public ActionResult AddReviewToDb(ReviewViewModel RevViewModel)
        {
            Review review = new Review();
            ReviewDal reviewDal = new ReviewDal();
            review.Details = Request.Form["Review"];
            review.OrderNum = RevViewModel.order.OrderNum;
            review.UserId = Request.Form["ID"];
            UserDal userDal = new UserDal();
            OrderDal orderDal = new OrderDal();
            Order ord = new Order();
            ord=orderDal.Order.Find(RevViewModel.order.OrderNum);
            User user = new User();
            user = userDal.User.Find(userId);
            review.UserFirstName = user.FirstName;
            review.UserLastName = user.LastName;
            review.UserName = user.UserName;
            review.CompCode = ord.CompanyCode;

            reviewDal.Review.Add(review);
            reviewDal.SaveChanges();
            return View("UserWindow");
        }

        public ActionResult AddReview()
        {
            OrderDal orderDal = new OrderDal();
            List<Order> UserOrders =
                (from x in orderDal.Order
                 where x.UserId == StaticUser.Id
                 select x).ToList<Order>();

            ReviewViewModel revView = new ReviewViewModel();
            revView.orders = UserOrders;
            revView.user = StaticUser;
            revView.order = new Order();

            return View(revView);
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