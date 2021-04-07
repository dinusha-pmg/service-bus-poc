using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceBus.Poc.Dto;


namespace ServiceBus.Poc.Client
{
    public static class CustomerController
    {
       private static string ServiceBusConnectionString = System.Environment.GetEnvironmentVariable("SB_ConnectionString");
       private static string TopicName = System.Environment.GetEnvironmentVariable("TopicName");
        //string Subscription = System.Environment.GetEnvironmentVariable("Subscription");
        
        [FunctionName("AddCustomer")]
        public static async Task<IActionResult> AddCustomer(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function \"Add Customer\" processed a request.");
            

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var dto = JsonConvert.DeserializeObject<AddCustomerDto>(requestBody);

            var topicClient = new TopicClient(ServiceBusConnectionString, TopicName);
            Message message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dto)))
            {
                Label = dto.GetType().Name,
                ContentType = "application/json"
            };
            await topicClient.SendAsync(message);


            return new OkObjectResult("Customer create request added to service bus.");
        }

        [FunctionName("UpdateCustomer")]
        public static async Task<IActionResult> UpdateCustomer(
                [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequest req,
                ILogger log)
        {
            log.LogInformation("C# HTTP trigger function \"Update Customer\" processed a request.");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var dto = JsonConvert.DeserializeObject<UpdateCustomerDto>(requestBody);

            var topicClient = new TopicClient(ServiceBusConnectionString, TopicName);
            Message message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dto)))
            {
                Label = dto.GetType().Name,
                ContentType = "application/json"
            };
            await topicClient.SendAsync(message);
            return new OkObjectResult("Customer update request added to service bus.");
        }

        [FunctionName("DeleteCustomer")]
        public static async Task<IActionResult> DeleteCustomer(
           [HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)] HttpRequest req,
           ILogger log)
        {
            log.LogInformation("C# HTTP trigger function \"DeleteCustomer\" processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var dto = JsonConvert.DeserializeObject<DeleteCustomerDto>(requestBody);

            var topicClient = new TopicClient(ServiceBusConnectionString, TopicName);
            Message message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(dto)))
            {
                Label = dto.GetType().Name,
                ContentType = "application/json"
            };
            await topicClient.SendAsync(message);


            return new OkObjectResult("Customer delete request added to service bus.");
        }
    }
}

