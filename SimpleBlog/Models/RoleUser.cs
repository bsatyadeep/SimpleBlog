namespace SimpleBlog.Models
{
    public class RoleUser
    {
        public virtual int UserId { get; set; }
        public virtual int RoleId { get; set; }

        public RoleUser()
        {
        }
    }
}