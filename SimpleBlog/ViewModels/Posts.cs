using SimpleBlog.Infrastructure;
using SimpleBlog.Models;

namespace SimpleBlog.ViewModels
{
    public class PostsIndex
    {
        public PagedData<Post> Posts { get; set; }
    }

    public class PostsShow
    {
        public Post Post { get; set; }
    }

    public class PostTag
    {
        public PagedData<Post> Posts { get; set; }
        public Tag Tag { get; set; }
    }
}