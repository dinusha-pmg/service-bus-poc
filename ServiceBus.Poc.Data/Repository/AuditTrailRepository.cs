using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceBus.Poc.Data.Repository
{
  public  class AuditTrailRepository
    {
        DataContext context;
        public AuditTrailRepository()
        {
            context = new DataContext();
        }

        public void AddAuditTrail(AuditTrail auditTrail)
        {
            context.AuditTrails.Add(auditTrail);
            context.SaveChanges();
        }
    }
}
