using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ServiceBus.Poc.Data.Ado
{
    public class CustomerRepository
    {
        string ConnectionString;
        public CustomerRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public int AddCustomer(CustomerDto customer)
        {
            int id = 0;
            //output INSERTED.ID
            string query = "INSERT INTO [dbo].[Customers] ([CustomerName] ,[Address] ,[PhoneNumber] ,[RequestedUserId]) VALUES (@customerName, @address,@phoneNumber,@requestedUserId)";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn)
                {
                    CommandType = System.Data.CommandType.Text
                };
                cmd.Parameters.AddWithValue("@customerName", customer.CustomerName);
                cmd.Parameters.AddWithValue("@address", customer.Address);
                cmd.Parameters.AddWithValue("@phoneNumber", customer.PhoneNumber);
                cmd.Parameters.AddWithValue("@requestedUserId", "");
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            return id;
        }

        public void UpdateCustomer(int customerId, List<KeyValuePair<string, string>> columns)
        {
            string query = "UPDATE [dbo].[Customers] SET ";
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            foreach (KeyValuePair<string, string> kvp in columns)
            {
                query = query + kvp.Key + " = @" + kvp.Key + ",";
                sqlParams.Add(new SqlParameter("@" + kvp.Key, kvp.Value));
            }
            _ = query.Remove(query.Length - 1);
            _ = query + "WHERE CustomerId = @CustomerId";
            sqlParams.Add(new SqlParameter("@CustomerId", customerId));
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn)
                { CommandType = System.Data.CommandType.Text };

                cmd.Parameters.AddRange(sqlParams.ToArray());
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void DeleteCustomer(int customerId)
        {
            string query = "DELETE FROM [dbo].[Customers] WHERE CustomerId=@customerId";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn)
                { CommandType = System.Data.CommandType.Text };

                cmd.Parameters.AddWithValue("@customerId", customerId);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public CustomerDto FindCustomer(int customerId)
        {
            CustomerDto customer = new CustomerDto();
            string query = "SELECT * FROM [dbo].[Customers] WHERE CustomerId=@customerId";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn)
                { CommandType = System.Data.CommandType.Text };

                cmd.Parameters.AddWithValue("@customerId", customerId);
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    customer.Address = rdr[""].ToString();
                    customer.CustomerId = Convert.ToInt32(rdr[""]);
                    customer.CustomerName = rdr[""].ToString();
                    customer.PhoneNumber = rdr[""].ToString();
                }

                conn.Close();

            }
            return customer;
        }
    }
}
