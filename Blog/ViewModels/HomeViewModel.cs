using Blog.Entities;
using Blog.Services.Dto;

namespace Blog.ViewModels
{
    public class HomeViewModel
    {
        public BlogPostDto BlogPostDto { get; set; }
        public List<BlogPost> Posts { get; set; } = new List<BlogPost>(); 

    }

    
}
