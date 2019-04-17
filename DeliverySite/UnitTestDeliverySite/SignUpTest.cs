using System;
using System.Web.Mvc;
using DeliverySite.Controllers;
using DeliverySite.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestDeliverySite
{
    [TestClass]
    public class SignUpTest
    {
        [TestMethod]
        public void UserSignupTest()
        {
            UserController controller = new UserController();

            User usr = new User();
            usr.FirstName = "testFN";
            usr.LastName = "testLN";
            usr.Password = "123456789";
            usr.UserName = "test3UserName";
            usr.Id = "204688764";

            ViewResult result = controller.SignUp(usr) as ViewResult;

            Assert.AreEqual("TEST SUCCEEDED", result.ViewBag.Test);
            
        }

        [TestMethod]
        public void ChangePasswordTest()
        {
            UserController controller = new UserController();

            var result = controller.ConfirmChangePassword("test55555", "test55555", "204688764") as ViewResult;

            Assert.AreEqual("Test Succeeded", result.ViewBag.TestChgPass);
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
        public void AddCompany()
        {
            CompanyController controller = new CompanyController();

            Company cmp = new Company();
            cmp.CompCode = "777777";
            cmp.CompName = "TestCompany";
            cmp.Password = "123456test";

            ViewResult result = controller.AddCompanyToDB(cmp) as ViewResult;

            Assert.AreEqual("Test SUCCEEDED",result.ViewBag.Test);
        }

        [TestMethod]
        public void ChangePasswordCompany()
        {
            CompanyController controller = new CompanyController();

            var result = controller.ConfirmChangePassword("test55555", "test55555", "777777") as ViewResult;

            Assert.AreEqual("Test Succeeded", result.ViewBag.TestChgPass);
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



        [TestMethod]
        public void ChangePasswordManager()
        {
            ManagerController controller = new ManagerController();

            var result = controller.ConfirmChangePassword("test55555", "test55555", "456456456") as ViewResult;

            Assert.AreEqual("Test Succeeded", result.ViewBag.TestChgPass);
        }

        [TestMethod]
        public void ShowUsersTest()
        {
            UserController controller = new UserController();

            var result = controller.ShowUsers() as ViewResult;

            Assert.AreEqual("ShowUsers", result.ViewName);
        }

        [TestMethod]
        public void ShowCompaniesTest()
        {
            CompanyController controller = new CompanyController();

            var result = controller.ShowCompanies() as ViewResult;

            Assert.AreEqual("ShowCompanies", result.ViewName);
        }









    }
}
