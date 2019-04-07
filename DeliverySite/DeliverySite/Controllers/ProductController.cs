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

        public ActionResult AddProduct()
        {
            ProductDal dal = new ProductDal();
            List<Product> objProducts = dal.Product.ToList<Product>();
            ProductViewModel pvm = new ProductViewModel();

            pvm.product = new Product();
            pvm.products = objProducts;
            return View(pvm);
        }

        public ActionResult getJson()
        {
            ProductDal Dal = new ProductDal();
            List<Product> val =
                    (from x in Dal.Product
                     select x).ToList<Product>();

            return Json(val, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddProductToDB(Product prd)
        {
            ProductViewModel pvm = new ProductViewModel();

            Product obj = new Product();
            obj.PrdId = prd.PrdId;
            obj.PrdName = prd.PrdName;
            obj.price = prd.price;
            obj.CompCode = prd.CompCode;


            ProductDal dal = new ProductDal();

            if (ModelState.IsValid)
            {
                List<Product> obj2 =
                        (from x in dal.Product
                         where x.PrdId == prd.PrdId
                         select x).ToList<Product>();
                ProductViewModel mng2 = new ProductViewModel();

                if (obj2.Count() > 0)
                {
                    TempData["existPrd"] = "This product is already exist";
                    pvm.products = dal.Product.ToList<Product>();
                    pvm.product = prd;
                    return View("AddProduct", pvm);
                }

                dal.Product.Add(prd);
                dal.SaveChanges();
                pvm.product = new Product();
                TempData["existPrd"] = "The product added successfuly!";
                return View("AddProduct", pvm);
            }
            else
            {
                TempData["existPrd"] = "Product Addition Failed!";
                pvm.product = prd;
            }

            pvm.products = dal.Product.ToList<Product>();
            List<Product> objProduct = dal.Product.ToList<Product>();
            return View("AddProduct",pvm);
        }
        


    }
}