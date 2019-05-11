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

            //ViewResult result2 = controller.CheckLogin(usr) as ViewResult;

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
        public void AddCompany()
        {
            CompanyController controller = new CompanyController();

            Company cmp = new Company();
            cmp.CompCode = "777777";
            cmp.CompName = "TestCompany";
            cmp.Password = "123456test";
            cmp.Mail = "aaa@gmail.com";

            ViewResult result = controller.AddCompanyToDB(cmp) as ViewResult;

            Assert.AreEqual("Test SUCCEEDED",result.ViewBag.Test);
        }

        [TestMethod]
        public void AddProductTest()
        {
            //Arrange
            ProductController controller = new ProductController();

            //Act
            Product prd = new Product();
            prd.PrdId = "9999";
            prd.PrdName = "TestProduct";
            prd.price = 100;
            prd.CompCode = "777777";
            ViewResult result = controller.AddProductToDB(prd) as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ChangePasswordCompany()
        {
            CompanyController controller = new CompanyController();

            var result = controller.ConfirmChangePassword("test55555", "test55555", "777777") as ViewResult;

            Assert.AreEqual("Test Succeeded", result.ViewBag.TestChgPass);
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
