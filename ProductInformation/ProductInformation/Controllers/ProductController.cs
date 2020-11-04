using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductInformation.Models;

namespace ProductInformation.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult List()
        {
            ViewBag.Products = GetProduct();
            return View();
        }
        public List<Product> GetProduct()
        {
            List<Product> Results;
            using (ProductInfoContext context = new ProductInfoContext())
            {
                Results = context.Products.ToList();
            }
            return Results;
        }
    }
}
