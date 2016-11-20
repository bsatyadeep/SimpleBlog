using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleBlog.Areas.Admin.ViewModels
{
    public class UsersEdid
    {
        [Required, MaxLength(128)]
        public string Username { get; set; }
        [Required, MaxLength(256), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public IList<RoleCheckBox> Roles { get; set; }

        public UsersEdid()
        {
            Roles = new List<RoleCheckBox>();
        }
    }
}