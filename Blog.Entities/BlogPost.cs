using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Entities
{
    public class BlogPost
    {
        //h
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
        public string Image { get; set; }

        public DateTime PublicationDate { get; set; }

        public int AuthorId { get; set; }

        public virtual User Author { get; set; }
    }
}
