using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.Poc.Data.Ado
{
    public class AuditTrailDto
    {
        public int AuditTrailId { get; set; }
        public string EntityType { get; set; }
        public string Action { get; set; }
        public string ActionReason { get; set; }
        public int RequestedUserId { get; set; }
        public string ObjectJson { get; set; }
        public string Message { get; set; }
    }
}
