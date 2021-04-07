using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServiceBus.Poc.Data.Ado;
using ServiceBus.Poc.Dto;

namespace ServiceBus.Poc.Processors
{
    public static class AddAuditTrail
    {
        [FunctionName("AddAuditTrail")]
        public static void Run([ServiceBusTrigger("customer-downstream-topic", subscriptionName: "esb-support-customer-subscr", Connection = "SB_ConnectionString")] string mySbMsg, ILogger log)
        {
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {mySbMsg}");
            string DbConnectionString = System.Environment.GetEnvironmentVariable("DBConnectionString");
            AuditTrailRepository auditTrailRepository = new AuditTrailRepository(DbConnectionString);
            var dto = JsonConvert.DeserializeObject<AuditTrailDto>(mySbMsg);
            if(dto.Message ==null && dto.EntityType==null)
            {
                JObject jo = JObject.Parse(mySbMsg);
                dto = new AuditTrailDto
                {
                    EntityType = "Customer",
                    Message = mySbMsg,
                    RequestedUserId = Convert.ToInt32( jo["RequestedUserId"]),
                    ObjectJson=string.Empty
                };
                if(jo.ContainsKey("UpdateReason"))
                {
                    dto.Action = "Update Customer Request";
                    dto.ActionReason = jo["UpdateReason"].ToString();
                }
               else if (jo.ContainsKey("DeleteReason"))
                {
                    dto.Action = "Delete Customer Request";
                    dto.ActionReason = jo["DeleteReason"].ToString();
                }
                else
                {
                    dto.Action = "Add Customer Request";
                    dto.ActionReason = string.Empty;
                }
            }
            auditTrailRepository.AddAuditTrail(dto);
            var t = dto.GetType();
            log.LogInformation(t.Name);
        }
    }
}
