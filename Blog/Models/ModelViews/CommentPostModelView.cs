namespace Blog.Models.ModelViews
{
    public class CommentPostModelView
    {
        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }
        public string Content { get; set; }
        public CommentPostModelView()
        {
        }
    }
}
