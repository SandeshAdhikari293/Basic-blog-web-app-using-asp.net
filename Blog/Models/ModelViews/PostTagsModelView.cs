using NuGet.DependencyResolver;

namespace Blog.Models.ModelViews
{
    public class PostTagsModelView
    {
        public Post? Post { get; set; }
        public List<Tag>? Tags { get; set; }

        public PostTagsModelView()
        {
            Tags = new List<Tag>();
        }
    }
}
