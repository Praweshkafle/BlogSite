using Blog.Services.Repository.Implementation;
using Blog.Services.Repository.Interface;
using Core.Common.FileManager;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection AddCommonLibrary(this IServiceCollection services)
        {
            registerRepositories(services);

            return services;
        }

        private static void registerRepositories(IServiceCollection services)
        {
            services.AddScoped<IFileManager, FileManager>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBlogPostRepository, BlogPostRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
        }
    }
}
