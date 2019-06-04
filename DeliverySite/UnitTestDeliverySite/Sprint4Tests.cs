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
    public class Sprint4Tests
    {
        [TestMethod]
        public void ViewReviewsTest()
        {
            CompanyController controller = new CompanyController();

            ViewResult result = controller.ViewReviews() as ViewResult;

            Assert.AreEqual("ViewReviews", result.ViewName);

        }
    }
}
