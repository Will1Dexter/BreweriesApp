using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Breweries.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class ETagFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var request = context.HttpContext.Request;
            var response = context.HttpContext.Response;

            if (request.Method == "GET" && response.StatusCode == 200)
            {
                var content = JsonConvert.SerializeObject(context.Result);
                var etag = GenerateETag(content);

                // Value should be in quotes according to the spec
                if (!etag.EndsWith("\""))
                    etag = "\"" + etag + "\"";

                string ifNoneMatch = request.Headers["If-None-Match"];

                if (ifNoneMatch == etag)
                {
                    context.Result = new StatusCodeResult(304);
                }

                response.Headers.Add("ETag", etag);
            }
        }

        private static string GenerateETag(string content)
        {
            using (MD5 md5 = MD5.Create())
            {
                var contentBytes = Encoding.UTF8.GetBytes(content);
                var hash = md5.ComputeHash(contentBytes);
                var hex = BitConverter.ToString(hash);
                return hex.Replace("-", "");
            }
        }
    }
}
