using System.ComponentModel.DataAnnotations;

namespace RefreshToken.Model
{
    public class Roles
    {
        [Key]
        public int IdRoles { get; set; }
        [Required]
        public string RoleName { get; set; }
        public string Status { get; set; }
        public string UserCreated { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserModified { get; set; }
        public DateTime DateModified { get; set; }
    }
}
