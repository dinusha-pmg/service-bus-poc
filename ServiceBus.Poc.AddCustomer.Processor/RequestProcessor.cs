using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ServiceBus.Poc.AddCustomer.Processor
{
    public static class RequestProcessor
    {
        [FunctionName("Function1")]
        public static void Run([ServiceBusTrigger("customer-downstream-topic", "create-customer-subcr", Connection = "SB_ConnectionString")]string mySbMsg, ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
        }
    }
}
