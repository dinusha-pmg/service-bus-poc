using System;
using System.Text;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ServiceBus.Poc.Data.Ado;
using ServiceBus.Poc.Dto;

namespace ServiceBus.Poc.Processors
{
    public static class AddCustomer
    {
        [FunctionName("AddCustomer")]
        public static async System.Threading.Tasks.Task RunAsync([ServiceBusTrigger("customer-downstream-topic", "create-customer-subcr", Connection = "SB_ConnectionString")]string mySbMsg, ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            string DbConnectionString = System.Environment.GetEnvironmentVariable("DBConnectionString");
            string ServiceBusConnectionString = System.Environment.GetEnvironmentVariable("SB_ConnectionString");
            string TopicName = System.Environment.GetEnvironmentVariable("TopicName");

            CustomerRepository customerRepository = new CustomerRepository(DbConnectionString);
            AddCustomerDto dto= JsonConvert.DeserializeObject<AddCustomerDto>(mySbMsg);
            CustomerDto customer = new CustomerDto()
            {
                Address = dto.Address,
                CustomerName = dto.CustomerName,
                PhoneNumber = dto.PhoneNumber
            };
            int customerId = customerRepository.AddCustomer(customer);
            customer.CustomerId = customerId;

            AuditTrailDto auditTrail = new AuditTrailDto()
            {
                Action = "Add Customer Response",
                EntityType = "Customer",
                ActionReason=string.Empty,
                Message = mySbMsg,
                ObjectJson = JsonConvert.SerializeObject(customer),
                RequestedUserId = dto.RequestedUserId
            };

            var topicClient = new TopicClient(ServiceBusConnectionString, TopicName);
            Message message = new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(auditTrail)))
            {
                Label = auditTrail.GetType().Name,
                ContentType = "application/json"
            };
            await topicClient.SendAsync(message);
        }
    }
}
