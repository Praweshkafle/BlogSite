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
using static Dapper.SqlMapper;

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
            var sql = "Insert into Comments (Text,CommentDate,AuthorId,BlogPostId) VALUES (@Text,@CommentDate,@AuthorId,@BlogPostId) SELECT CAST(SCOPE_IDENTITY() AS INT);";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleAsync<int>(sql, entity);
                return result;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Comments WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new { Id = id });
                return result;
            }
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            var sql = "SELECT * FROM Comments";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<Comment>(sql);
                return result.ToList();
            }
        }

        public Task<Comment> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

       

        public async Task<int> UpdateAsync(Comment entity)
        {
            var sql = "UPDATE Comments SET Text = @Text, CommentDate = @CommentDate, AuthorId = @AuthorId, BlogPostId = @BlogPostId WHERE Id = @Id";
            using (var connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, entity);
                return result;
            }
        }
    }
}
