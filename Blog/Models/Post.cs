using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class Post
    {
        public Guid ID { get; set; }
        public string Heading { get; set; }
        public string PageTitle { get; set; }
        public string Content { get; set; }
        public string ShortDescription { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; }
        public bool Visible { get; set; }
        public ICollection<Tag>? Tags { get; set; }
        //public User User { get; set; }

        public ICollection<Comment>? Comments { get; set; }

        public Post()
        {
            Tags = new List<Tag>();
            Comments = new List<Comment>();
        }

    }
}
