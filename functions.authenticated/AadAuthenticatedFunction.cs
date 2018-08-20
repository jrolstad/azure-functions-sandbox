
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace functions.authenticated
{
    public static class AadAuthenticatedFunction
    {
        [FunctionName("aadauthenticated")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequest req, 
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var name = ClaimsPrincipal.Current?.Identity?.Name;
            var claims = ClaimsPrincipal.Current?.Claims?
                .ToDictionary(c => c.Type, c => c.Value);

            dynamic result = new ExpandoObject();
            result.name = name ?? "Unauthenticated";
            result.claims = claims;

            return new OkObjectResult(result);
        }
    }
}
