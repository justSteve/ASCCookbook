using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Chapter1
{
    public static class Chapter1
    {
        [FunctionName("Recipe1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]
            HttpRequest req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");
            string firstname = null, lastname = null;
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            dynamic inputJson = JsonConvert.DeserializeObject(GetJsonStringFromQueryString(requestBody));
            firstname = firstname ?? inputJson?.firstname;
            lastname = inputJson?.lastname;

            return (lastname + firstname) != null
                ? (ActionResult)new OkObjectResult($"Hello, {firstname + " " + lastname}")
                : new BadRequestObjectResult("Please pass a name on the query" + "string or in the request body");
        }
        public static string GetJsonStringFromQueryString(string queryString)
        {
            //https://stackoverflow.com/questions/31146621/json-net-convert-complex-querystring-to-jsonstring?lq=1
            var nvs = HttpUtility.ParseQueryString(queryString);
            var dict = nvs.AllKeys.ToDictionary(k => k, k => nvs[k]);
            return JsonConvert.SerializeObject(dict, new KeyValuePairConverter());
        }
    }
}
