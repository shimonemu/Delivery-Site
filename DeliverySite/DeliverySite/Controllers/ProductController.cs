using DeliverySite.Dal;
using DeliverySite.Models;
using DeliverySite.ModelView;
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

        public ActionResult SearchProducts()
        {
            ProductDal dal = new ProductDal();
            string search = Request.Form["PrdName"];
            string search2 = Request.Form["CompCode"];
            

            ProductViewModel prd = new ProductViewModel();

            if (search == null && search2 == null)
            {
                List<Product> allProducts =
                    (from x in dal.Product
                     select x).ToList<Product>();
                prd = new ProductViewModel();
                prd.products = allProducts;
                prd.product = new Product();
                return View("ShowProducts",prd);
            }
            List<Product> obj =
                (from x in dal.Product
                 where (x.PrdName.Contains(search) && x.CompCode.Contains(search2))
                 select x).ToList<Product>();
            prd = new ProductViewModel();
            prd.products = obj;
            prd.product = new Product();
            return View("ShowProducts",prd);
        }
        


    }
}