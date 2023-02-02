using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RefreshToken.Model
{
    public class RolesDetail
    {
        [Key]
        public int IdRolesDetail { get; set; }
        [ForeignKey("User")]
        public int IdUser { get; set; }
        [ForeignKey("Roles")]
        public int IdRoles { get; set; }
        public string Status { get; set; }
        public string UserCreated { get; set; }
        public DateTime DateCreated { get; set; }
        public string UserModified { get; set; }
        public DateTime DateModified { get; set; }

        public User User { get; set; }
        public Roles Roles { get; set; }
    }
}
