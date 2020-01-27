using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Chapter1
{
    public static class Chapter1
    {
        [FunctionName("Chapter1")]
        public static async Task<IActionResult> Run(
            //[HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] 
            HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string firstname = null, lastname = null; 
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            
            dynamic inputJson = JsonConvert.DeserializeObject(requestBody);
            firstname = firstname ?? inputJson?.firstname; lastname = inputJson?.lastname;

            return (lastname + firstname) != null 
                ? (ActionResult)new OkObjectResult($"Hello, {firstname + " " + lastname}") 
                : new BadRequestObjectResult("Please pass a name on the query" + "string or in the request body");
        }
    }
}
