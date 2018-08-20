using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace functions.authenticated.fullframework
{
    public static class AadAuthenticatedFull
    {
        [FunctionName("aadauthenticatedfull")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]
            HttpRequestMessage req,
            TraceWriter log)
        {
            var name = ClaimsPrincipal.Current?.Identity?.Name;
            var claims = ClaimsPrincipal.Current?.Claims?
                .ToDictionary(c => c.Type, c => c.Value);

            var result = new AuthData
            {
                name = name ?? "Unauthenticated",
                claims = claims
            };

            return req.CreateResponse(HttpStatusCode.OK, result);
        }

        public class AuthData
        {
            public string name;
            public Dictionary<string, string> claims;
        }
    }
}
;