using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimpleBlog.Areas.Admin.ViewModels
{
    public class UsersNew
    {
        [Required, MaxLength(128)]
        public string Username { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required, MaxLength(256), DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public IList<RoleCheckBox> Roles { get; set; }

        public UsersNew()
        {
            Roles = new List<RoleCheckBox>();
        }
    }
}