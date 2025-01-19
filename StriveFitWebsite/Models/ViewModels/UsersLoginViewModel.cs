using System.ComponentModel.DataAnnotations;

namespace StriveFitWebsite.Models.ViewModels
{
    public class UsersLoginViewModel
    {

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Password is required.")]
        public string Passwordhash { get; set; } = null!;

        [Required(ErrorMessage = "Role is required.")]
        public decimal Roleid { get; set; }

        [Required(ErrorMessage = "Userid is required.")]
        public decimal Userid { get; set; }

        public DateTime? Lastlogin { get; set; }

        public string? Isactive { get; set; }
    }
}
