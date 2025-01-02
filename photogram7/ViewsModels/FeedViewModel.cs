using System.Collections.Generic; // Required for List<T>

namespace photogram.Models
{
    public class FeedViewModel
    {
        public List<Post> Posts { get; set; } = new List<Post>();// Property to hold a list of posts
    }
}
