using ApplicationCore.Interfaces;
using ApplicationCore.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationCore.Helpers
{
    public static class DependencyInjectionHelper
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IGroupTaskService, GroupTaskService>();

            services.AddScoped<ITasksService, TasksService>();
        }
    }
}

