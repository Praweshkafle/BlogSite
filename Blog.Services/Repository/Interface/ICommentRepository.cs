using Blog.Entities;
using Blog.Services.Dto;
using Core.Common.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Repository.Interface
{
    public interface ICommentRepository:IBaseRepository<CommentDto>
    {
    }
}
