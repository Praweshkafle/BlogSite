using Blog.Services.Dto;
using Blog.Services.Repository.Interface;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;

namespace Blog.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IUserRepository _userRepository;
        public HomeController(IBlogPostRepository blogPostRepository, IUserRepository userRepository)
        {
            _blogPostRepository = blogPostRepository;
            _userRepository = userRepository;
        }

        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var blogs = await _blogPostRepository.GetAllAsync();
                var blogPost = new List<BlogModal>();
                foreach (var blog in blogs)
                {
                    var authname = await _userRepository.GetByIdAsync(blog.AuthorId);
                    blogPost.Add(new BlogModal
                    {
                        Title = blog.Title,
                        Content = blog.Content,
                        PublicationDate = blog.PublicationDate,
                        Id = blog.Id,
                        AuthorName = authname.Username,
                        Image=blog.Image,
                    }) ;
                }
                if (blogPost != null)
                {
                    return View(blogPost);
                }
                return View();
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
