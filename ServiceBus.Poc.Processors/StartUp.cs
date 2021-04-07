using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(ServiceBus.Poc.Processors.StartUp))]
namespace ServiceBus.Poc.Processors
{
    public class StartUp : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string dbConnString = "Server=TI-NB-366;Database=esb_poc;Trusted_Connection=True;MultipleActiveResultSets=true";
            //builder.Services.AddDbContext<DataContext>(opt => opt.UseSqlServer(dbConnString));
        }
    }
}
