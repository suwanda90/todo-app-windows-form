using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.BaseEntity;
using ApplicationCore.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace ToDoApp
{
    static class Program
    {

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var host = Host.CreateDefaultBuilder()
             .ConfigureServices((context, services) =>
             {
                 services.AddDbContext<ApplicationDbContext>(options =>
                 {
                     options.UseSqlServer(ConfigurationManager.AppSettings["ConnectionString"]);
                 });

                 ConfigureServices(services);
             })
             .Build();

            var services = host.Services;
            var mainForm = services.GetRequiredService<Form1>();
            Application.Run(mainForm);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IEfRepository<,>), typeof(EfRepository<,>));

            services.AddScoped<IGroupTaskService, GroupTaskService>();
            services.AddScoped<ITasksService, TasksService>();

            services.AddSingleton<Form1>();
        }
    }
}
