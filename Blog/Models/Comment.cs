namespace Blog.Models
{
    public class Comment
    {

        public Guid ID { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime Posted { get; set; }
        public Post Post { get; set; }

    }
}
