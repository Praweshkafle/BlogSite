using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Services.Dto
{
    public class BlogPostDto
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Image { get; set; } = string.Empty;

        public string Content { get; set; }

        public DateTime PublicationDate { get; set; }= DateTime.Now;

        public int AuthorId { get; set; } = 0;

    }
}
