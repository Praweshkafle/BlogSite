namespace Blog.ViewModels
{
    public class CommentModel
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public DateTime CommentDate { get; set; }

        public int AuthorId { get; set; }
        public string Username { get; set; }
        public bool isUser { get; set; } = false;
        public int BlogPostId { get; set; }
    }
}
