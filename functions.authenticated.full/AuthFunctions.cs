using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace functions.authenticated.full
{
    public static class AuthFunctions
    {
        [FunctionName("IsAuthenticated")]
        public static HttpResponseMessage IsAuthenticated(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequestMessage request,
           TraceWriter log)
        {
            // simple test to check whether we have an authenticated user
            bool authenticated = Thread.CurrentPrincipal.Identity.IsAuthenticated;

            return request.CreateResponse(HttpStatusCode.OK, Thread.CurrentPrincipal.Identity.Name, "application/json");
        }

        [FunctionName("GetClaims")]
        public static HttpResponseMessage GetClaims(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
            HttpRequestMessage request,
            TraceWriter log)
        {
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var claimsPrincipal = Thread.CurrentPrincipal as ClaimsPrincipal;
                if (claimsPrincipal is null)
                {
                    return request.CreateResponse(HttpStatusCode.OK, "no claims", "application/json");
                }

                var claims = claimsPrincipal?.Claims.Select(c => new {c.Type, c.Value});
                return request.CreateResponse(HttpStatusCode.OK, claims, "application/json");
            }
            else
            {
                return request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Not Authorized");
            }
        }
       
    }
}
