using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using MongoDB.Driver;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using MongoDB.Bson;
using Newtonsoft.Json.Serialization;

namespace Nomadvantage.sav
{
    public static class TicketsService
    {
        [FunctionName(nameof(LoadTickets))]
        public async static Task<IActionResult> LoadTickets(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            TraceWriter log,
            ExecutionContext context)
        {
            log.Info($"C# HTTP trigger function processed a request {nameof(LoadTickets)}");

            var config = Config.Load(context);
            string name = req.Query["name"];

            // string requestBody = new StreamReader(req.Body).ReadToEnd();
            // dynamic data = JsonConvert.DeserializeObject(requestBody);
            // name = name ?? data?.name;

            var tickets = await MongoConnection.Load(config)
                .GetCollection<Ticket>("tickets")
                .Find(_ => true)
                .ToListAsync();

            return new JsonResult(tickets, JsonSettings);
        }

        [FunctionName(nameof(CreateTicket))]
        public async static Task<IActionResult> CreateTicket(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            TraceWriter log,
            ExecutionContext context)
        {
            log.Info($"C# HTTP trigger function processed a request {nameof(CreateTicket)}");

            var config = Config.Load(context);

            Ticket data = JsonConvert.DeserializeObject<Ticket>(GetRequestBody(req));

            var ticketsColletion = MongoConnection.Load(config).GetCollection<Ticket>("tickets");

            await ticketsColletion.InsertOneAsync(data);

            return new JsonResult(data, JsonSettings);
        }


        [FunctionName(nameof(UpdateTicket))]
        public async static Task<IActionResult> UpdateTicket(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
            TraceWriter log,
            ExecutionContext context)
        {
            log.Info($"C# HTTP trigger function processed a request {nameof(UpdateTicket)}");

            var config = Config.Load(context);

            Ticket ticket = JsonConvert.DeserializeObject<Ticket>(GetRequestBody(req));

            var updateResult = await MongoConnection.Load(config)
                .GetCollection<Ticket>("tickets")
                .ReplaceOneAsync(item => item.Id == ticket.Id, ticket); //, new UpdateOptions {IsUpsert = true}

            return new OkObjectResult(updateResult);
        }

        private static string GetRequestBody(HttpRequest req) =>
            new StreamReader(req.Body).ReadToEnd();

        public static JsonSerializerSettings JsonSettings => new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Objects,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
    }
}
