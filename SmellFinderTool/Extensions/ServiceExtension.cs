using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmellFinderTool.Core.Models;
using SmellFinderTool.Core.Services.Implementations;
using SmellFinderTool.Core.Services.Implementations.ReportWriterStrategies;
using SmellFinderTool.Core.Services.Interfaces;

namespace SmellFinderTool.Extensions
{
    public static class ServiceExtension
    {
        #region Fields
        private static ServiceProvider _serviceProvider;
        #endregion

        #region Methods
        public static ServiceProvider RegisterServices(IConfiguration configuration)
        {
            var services = new ServiceCollection();

            ReportSettings reportSettings = configuration.GetSection("Report").Get<ReportSettings>();
           
           // Register services
            services.AddScoped<IReportWriterStrategy, JSONReportWriterStrategy>();
            services.AddScoped<IReportWriterStrategy, YAMLReportWriterStrategy>();
            services.AddScoped<IReportWriterStrategy, TextPlainReportWriterStrategy>();
            services.AddSingleton<IDisplayerService, DisplayerService>();
            services.AddSingleton<IFileSystemManagerService, FileSystemManagerService>();
            services.AddScoped<IReportBuilderService, ReportBuilderService>();
            services.AddScoped<IParserService, ParserService>();
            services.AddScoped<IAnalizerService, AnalizerService>();
            services.AddTransient<ConsoleApplication>();
            services.AddSingleton(reportSettings);

            return services.BuildServiceProvider(true);
        }

        public static void DisposeServices()
        {
            if (_serviceProvider == null) return;
            if (_serviceProvider is IDisposable) ((IDisposable)_serviceProvider).Dispose();
        }
        #endregion
    }
}