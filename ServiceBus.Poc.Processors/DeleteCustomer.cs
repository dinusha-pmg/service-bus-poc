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
    public static class DeleteCustomer
    {
        [FunctionName("DeleteCustomer")]
        public static async System.Threading.Tasks.Task RunAsync([ServiceBusTrigger("customer-downstream-topic", "delete-customer-subscr", Connection = "SB_ConnectionString")]string mySbMsg, ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            string DbConnectionString = System.Environment.GetEnvironmentVariable("DBConnectionString");
            string ServiceBusConnectionString = System.Environment.GetEnvironmentVariable("SB_ConnectionString");
            string TopicName = System.Environment.GetEnvironmentVariable("TopicName");

            CustomerRepository customerRepository = new CustomerRepository(DbConnectionString);
            DeleteCustomerDto dto = JsonConvert.DeserializeObject<DeleteCustomerDto>(mySbMsg);
            CustomerDto customerToDelete = customerRepository.FindCustomer(dto.CustomerId);
            customerRepository.DeleteCustomer(dto.CustomerId);

            AuditTrailDto auditTrail = new AuditTrailDto()
            {
                Action = "Delete Customer Response",
                ActionReason = dto.DeleteReason,
                EntityType = "Customer",
                Message = mySbMsg,
                ObjectJson = JsonConvert.SerializeObject(customerToDelete),
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
