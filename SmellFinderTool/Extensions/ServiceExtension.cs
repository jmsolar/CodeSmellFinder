using System;
using Microsoft.Extensions.DependencyInjection;
using SmellFinderTool.Core.Services.Implementations;
using SmellFinderTool.Core.Services.Interfaces;

namespace SmellFinderTool.Extensions
{
    public static class ServiceExtension
    {
        #region Fields
        private static ServiceProvider _serviceProvider;
        #endregion

        #region Methods
        public static ServiceProvider RegisterServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IDisplayerService, DisplayerService>();
            services.AddSingleton<IFileSystemManagerService, FileSystemManagerService>();
            services.AddScoped<IReportBuilderService, ReportBuilderService>();
            services.AddScoped<IParserService, ParserService>();
            services.AddScoped<IAnalizerService, AnalizerService>();
            services.AddTransient<ConsoleApplication>();

            _serviceProvider = services.BuildServiceProvider(true);

            return _serviceProvider;
        }

        public static void DisposeServices()
        {
            if (_serviceProvider == null) return;
            if (_serviceProvider is IDisposable) ((IDisposable)_serviceProvider).Dispose();
        }
        #endregion
    }
}