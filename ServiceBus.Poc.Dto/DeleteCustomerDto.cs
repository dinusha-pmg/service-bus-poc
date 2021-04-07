using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.Poc.Dto
{
   public class DeleteCustomerDto:BaseDto
    {
        public int CustomerId { get; set; }
        public string DeleteReason { get; set; }
    }
}
