using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace ServiceBus.Poc.Data.Ado
{
    public class AuditTrailRepository
    {
        string ConnectionString;

        public AuditTrailRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public void AddAuditTrail(AuditTrailDto auditTrail)
        {
            string query = "INSERT INTO [dbo].[AuditTrails] ([EntityType],[Action],[ActionReason],[RequestedUserId],[ObjectJson],[Message])" +
     "VALUES(@EntityType, @Action, @ActionReason, @RequestedUserId, @ObjectJson, @Message)";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn)
                {
                    CommandType = System.Data.CommandType.Text
                };
                cmd.Parameters.AddWithValue("@EntityType", auditTrail.EntityType);
                cmd.Parameters.AddWithValue("@Action", auditTrail.Action);
                cmd.Parameters.AddWithValue("@ActionReason", auditTrail.ActionReason);
                cmd.Parameters.AddWithValue("@RequestedUserId", auditTrail.RequestedUserId);
                cmd.Parameters.AddWithValue("@ObjectJson", auditTrail.ObjectJson);
                cmd.Parameters.AddWithValue("@Message", auditTrail.Message);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
