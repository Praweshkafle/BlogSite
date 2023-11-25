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
    public class CommentRepository : ICommentRepository
    {
        private readonly IConfiguration configuration;
        public CommentRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<int> AddAsync(Comment entity)
        {
            var sql = "Insert into Comments (Text,CommentDate,AuthorId,BlogPostId) VALUES (@Text,@CommentDate,@AuthorId,@BlogPostId)";
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

        public Task<IEnumerable<Comment>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Comment> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

       

        public Task<int> UpdateAsync(Comment entity)
        {
            throw new NotImplementedException();
        }
    }
}
