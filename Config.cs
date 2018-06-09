using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;

namespace Nomadvantage.sav
{
    public static class Config
    {
        public static IConfigurationRoot Load(ExecutionContext context) =>
             new ConfigurationBuilder()
                .SetBasePath(context.FunctionAppDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

    }
}