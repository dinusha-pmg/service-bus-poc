using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceBus.Poc.Data.Repository
{
   public class CustomerRepository
    {
        DataContext context;

        public CustomerRepository()
        {
            context = new DataContext();
        }
        public int AddCustomer(Customer customer)
        {
            context.Customers.Add(customer);
            if (context.SaveChanges()>=1)
            {
                return customer.CustomerId;
            }
            else
            {
                return 0;
            }
        }
        public void UpdateCustomer(Customer customer)
        {
            context.Entry(customer).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
        }

        public void DeleteCustomr(int id)
        {
            Customer customer = context.Customers.Where(e => e.CustomerId == id).FirstOrDefault();
            context.Customers.Remove(customer);
            context.SaveChanges();
        }
        public Customer FindCustomer(Predicate<Customer> predicate)
        {
            return context.Customers.Find(predicate);
        }
    }
}
