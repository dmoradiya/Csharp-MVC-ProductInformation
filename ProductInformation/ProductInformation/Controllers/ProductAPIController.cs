using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductInformation.Models;
using ProductInformation.Models.Exceptions;

namespace ProductInformation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        // Common HTTP Methods:
        /*
            GET: Read / Query - Get data (typically in JSON format from APIs).
            POST: Submission of a new entity.
            PUT: Update of an existing entity (full replace).
            PATCH: Update of an existing entity (partial replace, typically with instructions).
            DELETE: Deletes an entity.
        */

        // Common HTTP Status Codes:
        /*
            200: "Ok" - Success, OK, everything's good.
            400: "Bad Request" - Parameters aren't of the right type, etc.
            404: "Not Found" - Tried to access a resource that's not there.
            409: "Conflict" - The proposed entity breaks a business logic rule, etc.
        */

        // Less Common HTTP Status Codes:
        /*
            301: "Moved Permanently" - Whatever you're trying to access has changed URL / locations.
            401: "Unauthorized" - User is not logged in, and therefore doesn't have rights to access the resource.
            403: "Forbidden" - User is logged in, but doesn't have rights to access the resource.
            410: "Gone" - Whatever they're trying to access is gone with no new location known.
            418: "I'm A Teapot" - Cannot brew a cup of coffee with a teapot (joke entry).
            422: "Unprocessable Entity" - Kind of similar to conflict, the entity breaks business logic rules.
            500: "Internal Server Error" - Something's broke, who knows what
        */

        [HttpGet("All")]
        public ActionResult<IEnumerable<Product>> AllProducts_GET()
        {
            return new ProductController().GetProducts();
        }
        [HttpGet("ByID")]
        public ActionResult<Product> ProductByID_GET(string productID)
        {
            ActionResult<Product> result;
            try
            {
                result = new ProductController().GetProductByID(productID);
            }
            catch (ArgumentNullException e)
            {
                result = BadRequest(e.Message);
            }
            catch (ArgumentException e)
            {
                result = BadRequest(e.Message);
            }
            catch (InvalidOperationException e)
            {
                result = NotFound(e.Message);
            }
            return result;
        }

        [HttpGet("ByCategoryID")]
        public ActionResult<IEnumerable<Product>> ProductsByCategoryID_GET(string categoryID)
        {
            ActionResult<IEnumerable<Product>> result;
            try
            {
                result = new ProductController().GetProductsByCategoryID(categoryID);

            }
            catch (ArgumentNullException e)
            {
                result = BadRequest(e.Message);
            }
            catch (ArgumentException e)
            {
                result = BadRequest(e.Message);
            }
            catch (KeyNotFoundException e)
            {
                result = NotFound(e.Message);
            }
            return result;
        }

        [HttpPost("Create")]
        public ActionResult<Product> ProductCreate_POST(string categoryID, string name)
        {
            ActionResult<Product> result;
            try
            {
                result = new ProductController().CreateProduct(categoryID, name);
            }
            catch (ValidationException e)
            {
                string error = "Error(s) During Creation: " +
                    e.ValidationExceptions.Select(x => x.Message)
                    .Aggregate((x, y) => x + ", " + y);

                result = BadRequest(error);
            }
            catch (Exception e)
            {
                result = StatusCode(500, "Unknown error occurred, please try again later.");
            }
            return result;

        }

        [HttpPut("Update")]
        public ActionResult<Product> ProductUpdate_PUT(string productID, string name)
        {
            ActionResult<Product> result;
            try
            {
                result = new ProductController().UpdateProductByID(productID, name);
            }
            catch (ArgumentNullException e)
            {
                result = BadRequest(e.Message);
            }
            catch (ArgumentException e)
            {
                result = BadRequest(e.Message);
            }
            catch (InvalidOperationException e)
            {
                result = NotFound(e.Message);
            }
            return result;

        }

        [HttpPatch("Patch")]
        public ActionResult<Product> ProductUpdate_PATCH(string productID, string propertyName, string newValue)
        {
            ActionResult<Product> result;
            try
            {
                if (propertyName == "Name")
                {
                    result = new ProductController().UpdateProductByID(productID, newValue);
                }
                else
                {
                    result = BadRequest("Unknown property specified, please try again.");
                }
            }
            catch (ArgumentNullException e)
            {
                result = BadRequest(e.Message);
            }
            catch (ArgumentException e)
            {
                result = BadRequest(e.Message);
            }
            catch (InvalidOperationException e)
            {
                result = NotFound(e.Message);
            }
            return result;

        }
    }
}
