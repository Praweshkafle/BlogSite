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
            var sql = "Insert into BlogPosts (Title,Content,PublicationDate,AuthorId,Image) VALUES (@Title,@Content,@PublicationDate,@AuthorId,@Image)";
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

        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            var sql = "SELECT * FROM BlogPosts";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<BlogPost>(sql);
                return result.ToList();
            }
        }

        public async Task<BlogPost> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM BlogPosts WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<BlogPost>(sql, new { Id = id });
                return result;
            }
        }

        public async Task<IEnumerable<BlogPost>> GetUserPosts(int authorId)
        {
            var sql = "SELECT * FROM BlogPosts WHERE AuthorId = @AuthorId";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<BlogPost>(sql, new { AuthorId = authorId });
                return result.ToList();
            }
        }

        public Task<int> UpdateAsync(BlogPost entity)
        {
            throw new NotImplementedException();
        }
    }
}
