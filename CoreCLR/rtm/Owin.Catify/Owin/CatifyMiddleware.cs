using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Owin.Catify
{
    public class CatifyMiddleware
    {
        private readonly RequestDelegate _next;
        public CatifyMiddleware (RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            context.Response.Body = new ImageSourceHighjackerStream(
			 	"http://thecatapi.com/api/images/get", 
			 	context.Response.Body, 
			 	Encoding.UTF8);

            return _next(context);
        }
    }
}