using System.ComponentModel.DataAnnotations;

namespace SimpleBlog.ViewModels
{
    public class AuthLogin
    {
        [Required(ErrorMessage = "Please enter valid user name"), Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please enter valid password"), DataType(DataType.Password)]
        public string Password { get; set; }
    }
}