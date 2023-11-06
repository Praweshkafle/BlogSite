namespace Blog.Helpers
{
    public class Alert
    {
        public string message { get; set; }
        public messageType message_type { get; set; }

    }
    public enum messageType
    {
        success,
        error
    }
}
