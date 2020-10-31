using MG.UPS.TechnicalAssessment.EmployeeManagement.Services;
using MG.UPS.TechnicalAssessment.ServiceClientRest;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace MG.UPS.TechnicalAssessment.EmployeeManagement
{
    static class Program
    {
        static void ConfigureServices(ServiceCollection services)
        {            
            services.AddScoped<IEmployeeManagementService, EmployeeManagementService>();
            services.AddScoped<IFileManagementService, FileManagementService>();
            services.AddScoped<ICsvManagementService, CsvManagementService>();
            services.AddScoped<frmEmployeeManagement>();
            services.AddRestClient(options => {
                options.BaseUrl = ConfigurationManager.AppSettings["BaseUrl"].ToString();
                options.APIKey = ConfigurationManager.AppSettings["APIKey"].ToString();
            });
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            using (ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider())
            {
                var form1 = serviceProvider.GetRequiredService<frmEmployeeManagement>();
                Application.Run(form1);
            }

        }
    }
}
