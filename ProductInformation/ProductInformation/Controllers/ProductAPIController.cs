using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProductInformation.Models;

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
    }
}
