using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime CommentDate { get; set; }

        public int AuthorId { get; set; }

        public virtual User Author { get; set; }

        public int BlogPostId { get; set; }

        public virtual BlogPost BlogPost { get; set; }
    }
}
