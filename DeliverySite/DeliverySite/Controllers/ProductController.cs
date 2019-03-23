using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeliverySite.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShowProducts()
        {

            ProductDal dal = new ProductDal();
            ProductViewModel pat = new ProductViewModel();
    
            List<Product> allProducts =
                (from x in dal.Product
                 select x).ToList<Product>();
            pat = new ProductViewModel();
            pat.products = allProducts;
            pat.product = new Product();

            return View(pat);

        }
    }
}