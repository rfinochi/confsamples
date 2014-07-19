using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using DemoWebAPI.Models;

namespace DemoWebAPI.Controllers
{
    public class ProductsController : ApiController
    {
        // GET /api/values
        public IEnumerable<Product> GetAllProducts()
        {
            return new List<Product>()
            {
                new Product() { Id = 1, Name = "fnord 1", Price=1.99M },
                new Product() { Id = 2, Name = "fnord 2", Price=2.99M },
                new Product() { Id = 3, Name = "fnord 3", Price = 3.99M }
            };
        }

        // GET /api/values/5
        public Product GetProductById(int id)
        {
            if (id < 1 || id > 3)
            {
                throw new HttpResponseException(System.Net.HttpStatusCode.NotFound);
            }
            return new Product() { Id = id, Name = "fnord " + id, Price = id + 0.99M };
        }
    }
}