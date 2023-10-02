namespace Blog.Models
{
    public class Tag
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
