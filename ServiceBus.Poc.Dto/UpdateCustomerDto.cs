using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.Poc.Dto
{
    public class UpdateCustomerDto:BaseDto
    {
        public int CustomerId { get; set; }
        public List<KeyValuePair<string,string>> UpdateColumnList { get; set; }
        public string UpdateReason { get; set; }
    }
}
