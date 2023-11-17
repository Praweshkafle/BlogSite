using Blog.Entities;
using Blog.Helpers;
using Blog.Services.Dto;
using Blog.Services.Repository.Interface;
using Blog.ViewModels;
using Core.Common.FileManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Blog.Controllers
{
    [Authorize]
    [Route("blog")]
    public class BlogController : BaseController
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IFileManager _fileManager;
        private readonly IUserRepository _userRepository;

        public BlogController(IBlogPostRepository blogPostRepository, IFileManager fileManager, IUserRepository userRepository)
        {
            _blogPostRepository = blogPostRepository;
            _fileManager = fileManager;
            this._userRepository = userRepository;
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> create(string blogPostDto, IFormFile file)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(blogPostDto))
                {
                    string imagePath = "";

                    var blogPost = JsonConvert.DeserializeObject<BlogPostDto>(blogPostDto);
                    if (file != null)
                    {
                        var prefix = blogPost.Title;
                        imagePath = _fileManager.saveImageAndGetFileName(file, prefix);
                    }
                    var blog = new BlogPost
                    {
                        PublicationDate = DateTime.Now,
                        AuthorId = getLoggedInUserId(),
                        Content = blogPost.Content,
                        Title = blogPost.Title,
                        Image = imagePath,
                    };
                    var result = await _blogPostRepository.AddAsync(blog);
                    AlertHelper.setMessage(this, "Post Added Successfully!!!", messageType.success);
                    return Json(new { success = true, message = "Post Added Successfully!!!" });
                }
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, messageType.error);
                throw;
            }
            return Json(new { success = false, message = "Failed To Post!" });
        }


        [Route("detail/{Id}")]
        [HttpGet]
        public async Task<IActionResult> detail(int Id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(getLoggedInUserId());
                var blogDetail = await _blogPostRepository.GetByIdAsync(Id);
                var blogModal = new BlogModal
                {
                    PublicationDate = blogDetail.PublicationDate,
                    AuthorName = user.Username,
                    Content = blogDetail.Content,
                    Id = Id,
                    Title = blogDetail.Title,
                    Image = blogDetail.Image,
                };
                if (blogModal != null)
                {
                    return View(blogModal);
                }
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, messageType.error);
            }
            return View();
        }


        [Route("edit/{Id}")]
        [HttpGet]
        public async Task<IActionResult> edit(int Id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(getLoggedInUserId());
                var blogDetail = await _blogPostRepository.GetByIdAsync(Id);
                var blogModal = new BlogModal
                {
                    PublicationDate = blogDetail.PublicationDate,
                    AuthorName = user.Username,
                    Content = blogDetail.Content,
                    Id = Id,
                    Title = blogDetail.Title,
                    Image = blogDetail.Image,
                };
                if (blogModal != null)
                {
                    return Json(new
                    {
                        Id = Id,
                        PublicationDate = blogDetail.PublicationDate,
                        AuthorName = user.Username,
                        Content = blogDetail.Content,
                        Title = blogDetail.Title,
                        Image = blogDetail.Image,
                    });
                }
            }
            catch (Exception ex)
            {
                AlertHelper.setMessage(this, ex.Message, messageType.error);
            }
            return Json(null);
        }

        [Route("myblog")]
        [HttpGet]
        public async Task<IActionResult> myblog()
        {
            try
            {
                var blogs = await _blogPostRepository.GetUserPosts(getLoggedInUserId());
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
                        AuthorName = authname.Username
                    });
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
