using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductInformation.Models;
using ProductInformation.Models.Exceptions;



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
            if (Request.Query.Count > 0)
            {
                try
                {
                    CreateProduct(categoryID, name);
                    ViewBag.Message = $"Successfully Created Product {name}!";
                }
                // Catch ONLY ValidationException. Any other Exceptions (FormatException, DivideByZeroException, etc) will not get caught, and will break the whole program.
                catch (ValidationException e)
                {
                    ViewBag.CategoryID = categoryID;
                    ViewBag.Name = name;
                    ViewBag.Message = "There exist problem(s) with your submission, see below.";
                    ViewBag.Exception = e;
                    ViewBag.Error = true;
                }
            }



            ViewBag.Categories = CategoryController.GetCategories();
            return View();
        }

        public IActionResult List(string filter)
        {
            if (filter == "Electronics")
            {
                ViewBag.Products = GetElectronicsProducts();
                ViewBag.Filter = true;
            }
            else
            {
                ViewBag.Products = GetProducts();
                ViewBag.Filter = false;
            }
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
        public List<Product> GetElectronicsProducts()
        {
            List<Product> results;
            using (ProductInfoContext context = new ProductInfoContext())
            {
                results = context.Products.Include(x => x.Category).Where(x => x.Category.Name == "Electronics").ToList();
            }
            return results;
        }
        public void CreateProduct(string categoryID, string name)
        {
            int parsedCategoryID = 0;
            ValidationException exception = new ValidationException();

            // Trim the values so we don't need to do it a bunch of times later.
            // Common validation points (1) and (4a).
            categoryID = !string.IsNullOrWhiteSpace(categoryID) ? categoryID.Trim() : null;
            name = !string.IsNullOrWhiteSpace(name) ? name.Trim() : null;

            // Check for individual validation cases and throw an exception if they fail.
            // I'll show you how to bundle up multiple simultaneous exceptions tomorrow.


            using (ProductInfoContext context = new ProductInfoContext())
            {

                //-------------------------
                // Validation of CategoryID
                //-------------------------

                // No value for category ID.
                if (string.IsNullOrWhiteSpace(categoryID))
                {
                    exception.ValidationExceptions.Add(new Exception("Category ID Not Provided"));
                }
                else
                {
                    // Category ID fails parse.
                    // Common validation points (5) and (5a).
                    if (!int.TryParse(categoryID, out parsedCategoryID))
                    {
                        exception.ValidationExceptions.Add(new Exception("Category ID Not Valid"));
                    }
                    else
                    {
                        // Category ID exists.
                        // Common validation point (7).
                        if (!context.Categories.Any(x => x.ID == parsedCategoryID))
                        {
                            exception.ValidationExceptions.Add(new Exception("Category Does Not Exist"));
                        }
                    }
                }

                // Validation point (6) doesn't have an example in this solution, but it could be achieved similarly to (7), or by the nature of the Single() method, which will throw an exception if it doesn't exist.

                //-------------------
                // Validation of Name
                //-------------------

                // No value for name.
                // Common validation point (4).
                if (string.IsNullOrWhiteSpace(name))
                {
                    exception.ValidationExceptions.Add(new Exception("Name Not Provided"));
                }
                else
                {
                    // Name is a duplicate.
                    // Not a common validation point necessarily, but does perform (2).
                    if (context.Products.Any(x => x.Name.ToUpper() == name.ToUpper()))
                    {
                        exception.ValidationExceptions.Add(new Exception("Name Already Exists"));
                    }
                    else
                    {
                        if (name.Length > 30)
                        {
                            // Name too long
                            // Common validation point (3).
                            exception.ValidationExceptions.Add(new Exception("The Maximum Length of a Name is 30 Characters"));
                        }
                        else
                        {
                            if (name.ToUpper() == "PAPER CUPS" && parsedCategoryID == context.Categories.Where(x => x.Name == "Kitchen").Single().ID)
                            {
                                exception.ValidationExceptions.Add(new Exception("Only Glass Glasses Allowed Here"));
                            }
                        }
                    }
                }
               




                if (exception.ValidationExceptions.Count > 0)
                {
                    throw exception;
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
