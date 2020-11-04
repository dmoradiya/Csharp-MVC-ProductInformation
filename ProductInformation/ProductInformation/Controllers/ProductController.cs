using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductInformation.Models;

namespace ProductInformation.Controllers
{
    public class ProductController : Controller
    {
        // Actions (View Endpoints):
        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public IActionResult Create(string categoryID, string name)
        {
            // If you're using GET submission, you can use Request.Query.Count to check for query string parameters.
            if (Request.Method == "POST")
            {
                try
                {
                    CreateProduct(categoryID, name);
                    ViewBag.Message = $"Successfully Created Product {name}!";
                }
                catch (Exception e)
                {
                    ViewBag.CategoryID = categoryID;
                    ViewBag.Name = name;
                    ViewBag.Message = "Error: " + e.Message;
                    ViewBag.Error = true;
                }
            }

            ViewBag.Categories = CategoryController.GetCategories();
            return View();
        }

        public IActionResult List()
        {
            ViewBag.Products = GetProducts();
            return View();
        }



        // Data Methods:
        public List<Product> GetProducts()
        {
            List<Product> results;
            using (ProductInfoContext context = new ProductInfoContext())
            {
                results = context.Products.Include(x => x.Category).ToList();
            }
            return results;
        }

        public void CreateProduct(string categoryID, string name)
        {
            int parsedCategoryID;

            // Trim the values so we don't need to do it a bunch of times later.
            categoryID = categoryID != null ? categoryID.Trim() : null;
            name = name != null ? name.Trim() : null;

            // Check for individual validation cases and throw an exception if they fail.
            // I'll show you how to bundle up multiple simultaneous exceptions tomorrow.

            // No value for category ID.
            if (string.IsNullOrWhiteSpace(categoryID))
            {
                throw new Exception("Category ID Not Provided");
            }

            // Category ID fails parse.
            if (!int.TryParse(categoryID, out parsedCategoryID))
            {
                throw new Exception("Category ID Not Valid");
            }

            // No value for name.
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("Name Not Provided");
            }

            using (ProductInfoContext context = new ProductInfoContext())
            {
                if (context.Categories.Where(x => x.ID == parsedCategoryID).Count() < 1)
                {
                    throw new Exception("Category Does Not Exist");
                }

                context.Products.Add(new Product()
                {
                    CategoryID = int.Parse(categoryID),
                    Name = name
                });
                context.SaveChanges();
            }
        }
    }
}
