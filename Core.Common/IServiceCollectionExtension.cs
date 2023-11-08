using Core.Common.FileManager;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddCommonHelpers(this IServiceCollection services)
        {
            registerHelpers(services);

            return services;
        }

        private static void registerHelpers(IServiceCollection services)
        {
            services.AddScoped<IFileManager, FileManager.FileManager>();
        }
    }
}
