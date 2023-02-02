using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace RefreshToken.Dto.RolesDetailDto
{
    public class ReviewRolesDetail
    {
        [Display(Name = "Id Roles Detail")]
        public int IdRolesDetail { get; set; }
        [Display(Name = "Id User")]
        public int IdUser { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; }
        [Display(Name = "Id Roles")]
        public int IdRoles { get; set; }
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        [Display(Name = "Status")]
        public string Status { get; set; }
        [Display(Name = "User Created")]
        public string UserCreated { get; set; }
        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }
        [Display(Name = "User Modified")]
        public string UserModified { get; set; }
        [Display(Name = "Date Modified")]
        public DateTime DateModified { get; set; }
    }
}
