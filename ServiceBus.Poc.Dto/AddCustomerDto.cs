using System;

namespace ServiceBus.Poc.Dto
{
    public class AddCustomerDto:BaseDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
