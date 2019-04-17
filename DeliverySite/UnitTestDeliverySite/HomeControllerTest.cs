using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using DeliverySite.Controllers;
using System.Web.Mvc;
using Microsoft.CSharp.RuntimeBinder;

namespace UnitTestDeliverySite
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void About()
        {
            //Arrange
            HomeController controller = new HomeController();

            //Act
            ViewResult result = controller.About() as ViewResult;

            //Assert
            Assert.AreEqual("Your application description page.", result.ViewBag.Message);
        }

     
    }
}
