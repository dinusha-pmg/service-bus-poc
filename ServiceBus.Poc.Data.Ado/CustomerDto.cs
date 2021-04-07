using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.Poc.Data.Ado
{
   public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
