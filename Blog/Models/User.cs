using Microsoft.AspNetCore.Identity;

namespace Blog.Models
{
    public class User : IdentityUser
    {
        public int ID { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; }

    }
}
