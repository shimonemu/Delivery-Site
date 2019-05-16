using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DeliverySite.Controllers;
using DeliverySite.Dal;
using DeliverySite.Models;
using DeliverySite.ModelView;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestDeliverySite
{
    [TestClass]
    public class Sprint3Tests
    {
        [TestMethod]
        public void AddManagerTest()
        {
            ManagerController controller = new ManagerController();

            Manager mng = new Manager();
            mng.Id = "999999999";
            mng.FirstName = "test";
            mng.LastName = "test";
            mng.UserName = "testTest";
            mng.Password = "1234test";
            mng.Mail = "aaa@gmail.com";

            ViewResult result = controller.AddManagerToDB(mng) as ViewResult;

            Assert.AreEqual("Test SUCCEEDED", result.ViewBag.Test);

        }

        [TestMethod]
        public void ContactCompanyTest()
        {
            UserController controller = new UserController();

            string cc = "777777";
            string sub = "test";
            string txt = "testetst test";


            ViewResult result = controller.SendMail(cc, sub, txt) as ViewResult;

            Assert.AreEqual("test SUCCEDED", result.ViewBag.Test);

        }

        [TestMethod]
        public void ContactManagerTest()
        {
            CompanyController controller = new CompanyController();

            string sub = "test";
            string txt = "testetst test";

            ManagerDal managerDal = new ManagerDal();
            List<Manager> allManagers = managerDal.Manager.ToList<Manager>();
            ManagerViewModel mngViewModel = new ManagerViewModel();
            mngViewModel.managers = allManagers;
            mngViewModel.manager = new Manager();
            mngViewModel.manager.Id = "999999999";


            ViewResult result = controller.SendMail(mngViewModel, sub, txt) as ViewResult;

            Assert.AreEqual("test SUCCEEDED", result.ViewBag.Test);

        }


        [TestMethod]
        public void DeleteManagerTest()
        {
            ManagerDal db = new ManagerDal();
            Manager mng = db.Manager.Find("999999999");
            db.Manager.Remove(mng);
            db.SaveChanges();

            Assert.IsNotNull(mng);
        }

        [TestMethod]
        public void EditProductTest()
        {
            CompanyController controller = new CompanyController();

            ViewResult result = controller.Edit("9999") as ViewResult;

            Assert.AreEqual("Test SUCCEEDED", result.ViewBag.Test);
        }

        [TestMethod]
        public void MakeOrderTest()
        {
            UserController controller = new UserController();

            ProductViewModel pvm = new ProductViewModel();
            pvm.product = new Product();
            pvm.product.PrdId = "9999";
            string id = "204688764";

            ViewResult result = controller.AddOrderToDb(pvm,id) as ViewResult;

            Assert.AreEqual("MakeOrder", result.ViewName);
        }

        [TestMethod]
        public void ReorderTest()
        {
            UserController controller = new UserController();

            ViewResult result = controller.ReOrder(3) as ViewResult;

            Assert.AreEqual("ShowOrders", result.ViewName);
        }


        [TestMethod]
        public void AddReviewTest()
        {
            UserController controller = new UserController();

            ReviewViewModel rvm = new ReviewViewModel();
            rvm.order = new Order();
            rvm.order.OrderNum = 3;
            string userid = "204688764";
            string details = " TEST TEST";

            ViewResult result = controller.AddReviewToDb(rvm, userid, details) as ViewResult;

            Assert.AreEqual("UserWindow", result.ViewName);


        }

        [TestMethod]
        public void IncomeReportTest()
        {
            CompanyController controller = new CompanyController();

            ViewResult result = controller.ViewMonthlyReport() as ViewResult;

            Assert.AreEqual("ViewMonthlyReport", result.ViewName);
        }

        [TestMethod]
        public void OrdersNumberTest()
        {
            ManagerController controller = new ManagerController();

            ViewResult result = controller.OrdersNumber("777777") as ViewResult;

            Assert.AreEqual("OrdersNumber", result.ViewName);
        }

        [TestMethod]
        public void ViewOrdersTest()
        {
            ManagerController controller = new ManagerController();

            ViewResult result = controller.ShowOrders() as ViewResult;

            Assert.AreEqual("ShowOrders", result.ViewName);
        }

        [TestMethod]
        public void ProductDetailsTest()
        {
            CompanyController controller = new CompanyController();

            ViewResult result = controller.Details("9999") as ViewResult;

            Assert.AreEqual("Details", result.ViewName);
        }

        [TestMethod]
        public void ShowCompaniesTest()
        {
            ManagerController controller = new ManagerController();

            ViewResult result = controller.EditCompanies() as ViewResult;

            Assert.AreEqual("EditCompanies", result.ViewName);
        }


        [TestMethod]
        public void DeleteUserTest()
        {

            UserController controller = new UserController();

            //Act
            var result = controller.DeleteConfirmed("204688764") as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("../User/ShowUsers", result.RouteValues["Action"]);

        }

        [TestMethod]
        public void DeleteProductTest()
        {
            //Arrange
            CompanyController controller = new CompanyController();

            //Act
            Product prd = new Product();
            prd.PrdId = "9999";
            Company cmp = new Company();
            cmp.CompCode = "777777";
            cmp.Password = "123456test";
            var result = controller.DeleteConfirmed(prd.PrdId) as RedirectToRouteResult;
            ViewResult result2 = controller.CheckLogin(cmp) as ViewResult;

            //Assert
            Assert.AreEqual("../Company/EditProducts", result.RouteValues["Action"]);


        }

        [TestMethod]
        public void DeleteCompanyTest()
        {
            ManagerController controller = new ManagerController();

            //Act
            var result = controller.DeleteConfirmed("777777") as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("../Manager/EditCompanies", result.RouteValues["Action"]);
        }


    }
}
