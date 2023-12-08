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
        private readonly ICommentRepository _commentRepository;

        public BlogController(IBlogPostRepository blogPostRepository, IFileManager fileManager, IUserRepository userRepository, ICommentRepository commentRepository)
        {
            _blogPostRepository = blogPostRepository;
            _fileManager = fileManager;
            this._userRepository = userRepository;
            _commentRepository = commentRepository;
        }
        [Route("comment")]
        [HttpPost]
        public async Task<IActionResult> comment(CommentDto data)
        {
            try
            {
                if (data == null)
                {
                    return Json(new { success = false, message = "Unable to post comment !!" });
                }
                if (data.Id > 0)
                {

                    var editCmt = new Comment
                    {
                        CommentDate = DateTime.Now,
                        AuthorId = getLoggedInUserId(),
                        BlogPostId = data.BlogPostId,
                        Text = data.Text,
                        Id = data.Id,
                    };
                    var editedRes = await _commentRepository.UpdateAsync(editCmt);
                    return Json(new { success = true, data = new CommentModel() });
                }
                var comment = new Comment
                {
                    CommentDate = DateTime.Now,
                    AuthorId = getLoggedInUserId(),
                    BlogPostId = data.BlogPostId,
                    Text = data.Text,
                };
                var result = await _commentRepository.AddAsync(comment);
                var user = await _userRepository.GetByIdAsync(comment.AuthorId);
                var commentModel = new CommentModel
                {
                    CommentDate = data.CommentDate,
                    AuthorId = getLoggedInUserId(),
                    BlogPostId = data.BlogPostId,
                    Username = user.Username,
                    Text = data.Text,
                    Id = result
                };
                return Json(new { success = true, data = commentModel });
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [Route("getcomment/{Id}")]
        [HttpGet]
        public async Task<IActionResult> getcomment(int Id)
        {
            try
            {
                if (Id <= 0)
                {
                    return Json(new { success = false, message = "Unable to find comment !!" });
                }
                var comments = await _commentRepository.GetAllAsync();
                var commentsAspost = comments.Where(a => a.BlogPostId == Id).ToList();
                var comentModel = new List<CommentModel>();
                foreach (var comment in commentsAspost)
                {
                    var user = await _userRepository.GetByIdAsync(comment.AuthorId);
                    comentModel.Add(new CommentModel
                    {
                        Id = comment.Id,
                        AuthorId = comment.AuthorId,
                        CommentDate = comment.CommentDate,
                        BlogPostId = comment.BlogPostId,
                        Username = user.Username.ToString(),
                        Text = comment.Text,
                        isUser = getLoggedInUserId() == comment.AuthorId ? true : false
                    });
                }
                return Json(new { success = true, data = JsonConvert.SerializeObject(comentModel) });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error Occured!!" });
            }

        }

        [Route("deletecomment/{Id}")]
        [HttpGet]
        public async Task<IActionResult> deletecomment(int Id)
        {
            try
            {
                if (Id <= 0)
                {
                    return Json(new { success = false, message = "Unable to find comment !!" });
                }
                 var result = await _commentRepository.DeleteAsync(Id);
                return Json(new { success = true, message="Deleted successfully!!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error Occured!!" });
            }

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
                    if (blogPost?.Id > 0)
                    {
                        if (file != null)
                        {
                            if (!string.IsNullOrWhiteSpace(blogPost.Image))
                            {
                                _fileManager.deleteImage(blogPost.Image, _fileManager.getPathToImageFolder());
                            }
                            var prefix = blogPost.Title;
                            imagePath = _fileManager.saveImageAndGetFileName(file, prefix);
                        }
                        var updateblog = new BlogPost
                        {
                            Id = blogPost.Id,
                            PublicationDate = DateTime.Now,
                            AuthorId = getLoggedInUserId(),
                            Content = blogPost.Content,
                            Title = blogPost.Title,
                            Image = imagePath,
                        };
                        var updateResult = await _blogPostRepository.UpdateAsync(updateblog);
                        return Json(new { success = true, message = "Post Updated Successfully!!!" });
                    }
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
