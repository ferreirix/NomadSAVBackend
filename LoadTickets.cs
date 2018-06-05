
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using MongoDB;
using MongoDB.Driver;
using System.Security.Authentication;
using System.Linq;
using System.Threading.Tasks;

namespace Nomadvantage.sav
{
    public static class LoadTickets
    {
        [FunctionName("LoadTickets")]
        public async static Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // string name = req.Query["name"];

            // string requestBody = new StreamReader(req.Body).ReadToEnd();
            // dynamic data = JsonConvert.DeserializeObject(requestBody);
            // name = name ?? data?.name;


            string connectionString = ""; // read from env variable
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);

            var tickets = await mongoClient
                .GetDatabase("nomadsav")
                .GetCollection<dynamic>("tickets")
                .Find(_ => true)
                .ToListAsync();

            return (ActionResult)new OkObjectResult(tickets);
        }
    }
}
