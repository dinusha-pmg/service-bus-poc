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
    public static class UpdateCustomer
    {
        [FunctionName("UpdateCustomer")]
        public static async System.Threading.Tasks.Task RunAsync([ServiceBusTrigger("customer-downstream-topic", "update-customer-subscr", Connection = "SB_ConnectionString")]string mySbMsg, ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            string DbConnectionString = System.Environment.GetEnvironmentVariable("DBConnectionString");
            string ServiceBusConnectionString = System.Environment.GetEnvironmentVariable("SB_ConnectionString");
            string TopicName = System.Environment.GetEnvironmentVariable("TopicName");

            CustomerRepository customerRepository = new CustomerRepository(DbConnectionString);
            UpdateCustomerDto dto = JsonConvert.DeserializeObject<UpdateCustomerDto>(mySbMsg);
            CustomerDto customerToUpdate = customerRepository.FindCustomer(dto.CustomerId);
            customerRepository.UpdateCustomer(dto.CustomerId, dto.UpdateColumnList);

            AuditTrailDto auditTrail = new AuditTrailDto()
            {
                Action = "Update Customer Response",
                ActionReason = dto.UpdateReason,
                EntityType = "Customer",
                Message = mySbMsg,
                ObjectJson = JsonConvert.SerializeObject(customerToUpdate),
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
