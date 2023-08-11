using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SmellFinderTool.Core.Services.Implementations;
using SmellFinderTool.Extensions;

namespace SmellFinderTool
{
    public class Program
    {
        static async Task Main() {
            var serviceProvider = ServiceExtension.RegisterServices();
            
            IServiceScope scope = serviceProvider.CreateScope();
            
            await scope.ServiceProvider
                .GetRequiredService<ConsoleApplication>()
                .Run();
  
            ServiceExtension.DisposeServices();
        }
    }
}
