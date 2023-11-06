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
    public class CommentRepository : ICommentRepository
    {
        public Task<int> AddAsync(CommentDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CommentDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CommentDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(CommentDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
