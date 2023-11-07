using Blog.Entities;
using Blog.Services.Dto;
using Blog.Services.Repository.Interface;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Repository.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {

        private readonly IConfiguration configuration;
        public BlogPostRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<int> AddAsync(BlogPost entity)
        {
            var sql = "Insert into BlogPosts (Title,Content,PublicationDate,AuthorId) VALUES (@Title,@Content,@PublicationDate,@AuthorId)";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BlogPost> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(BlogPost entity)
        {
            throw new NotImplementedException();
        }
    }
}
