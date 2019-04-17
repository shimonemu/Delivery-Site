using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using DeliverySite.Controllers;
using System.Web.Mvc;
using Microsoft.CSharp.RuntimeBinder;
using DeliverySite.Models;
using System.Data;
using System.Data.Entity;


namespace UnitTestDeliverySite
{
    [TestClass]
    public class ProductControllerTest
    {
        [TestMethod]
        public void AddProductTest()
        {
            //Arrange
            ProductController controller = new ProductController();

            //Act
            Product prd = new Product();
            prd.PrdId = "1111";
            prd.PrdName = "abc";
            prd.price = 100;
            prd.CompCode = "111222";
            ViewResult result = controller.AddProductToDB(prd) as ViewResult;

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void DeleteProductTest()
        {
            //Arrange
            CompanyController controller = new CompanyController();

            //Act
            Product prd = new Product();
            prd.PrdId = "1111";
            Company cmp = new Company();
            cmp.CompCode = "111222";
            cmp.Password = "123456788";
            var result = controller.DeleteConfirmed(prd.PrdId) as RedirectToRouteResult;
            ViewResult result2 = controller.CheckLogin(cmp) as ViewResult;
            
            //Assert
            Assert.AreEqual("../Company/EditProducts", result.RouteValues["Action"]);
            

        }

        [TestMethod]
        public void SearchProductTest()
        {
            ProductController controller = new ProductController();

            var result = controller.SearchProducts("abc") as ViewResult;

            Assert.AreEqual("ShowProducts", result.ViewName);

        }




        }
}
