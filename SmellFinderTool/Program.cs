using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmellFinderTool.Core.Services.Implementations;
using SmellFinderTool.Extensions;

namespace SmellFinderTool
{
    public class Program
    {
        static async Task Main() {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            
            var serviceProvider = ServiceExtension.RegisterServices(configuration);

            IServiceScope scope = serviceProvider.CreateScope();
            
            await scope.ServiceProvider
                .GetRequiredService<ConsoleApplication>()
                .Run();
  
            ServiceExtension.DisposeServices();
        }
    }
}
