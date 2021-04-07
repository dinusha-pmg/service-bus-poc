using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceBus.Poc.Data
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int RequestedUserId { get; set; }
    }
}
