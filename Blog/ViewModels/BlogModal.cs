namespace Blog.ViewModels
{
    public class BlogModal
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime PublicationDate { get; set; } = DateTime.Now;

        public string AuthorName { get; set; }

    }
}
