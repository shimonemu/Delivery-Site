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
        public void SearchProductTest()
        {
            ProductController controller = new ProductController();

            var result = controller.SearchProducts("abc") as ViewResult;

            Assert.AreEqual("ShowProducts", result.ViewName);

        }




        }
}
