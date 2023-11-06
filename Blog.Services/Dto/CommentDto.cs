using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Dto
{
    public class CommentDto
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CommentDate { get; set; }

        public int AuthorId { get; set; }

        public int BlogPostId { get; set; }

    }
}
