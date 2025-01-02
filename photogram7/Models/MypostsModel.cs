using System.Collections.Generic; // Required for List<T>

namespace photogram.Models
{
    public class MyPostsViewModel
    {
        public List<Post> UserPosts { get; set; } = new List<Post>(); // Property to hold a list of user's posts
    }
}