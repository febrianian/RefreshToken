using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RefreshToken.Dto.RolesDetailDto
{
    public class RolesDetailCreateDto
    {
        [Display(Name = "Id User")]
        public int IdUser { get; set; }
        [Display(Name = "Id Roles")]
        public int IdRoles { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "User Created")]
        public string UserCreated { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
    }
}
