using Blog.Entities;
using Blog.Services.Dto;
using Blog.Services.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Repository.Implementation
{
    public class BlogPostRepository : IBlogPostRepository
    {
        public Task<int> AddAsync(BlogPostDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BlogPostDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BlogPostDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(BlogPostDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
