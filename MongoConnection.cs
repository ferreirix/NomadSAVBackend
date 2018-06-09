using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using MongoDB.Driver;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Nomadvantage.sav
{
    public static class MongoConnection
    {
        public static MongoClient Load(IConfigurationRoot config)
        {
            string connectionString = config.GetConnectionString("MongoDBConnectionString");
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));

            settings.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            return new MongoClient(settings);
        }

        public static IMongoCollection<T> GetCollection<T>(this MongoClient mongoClient, string collection) =>
            mongoClient.GetDatabase("nomadsav")
                .GetCollection<T>(collection);
    }
}