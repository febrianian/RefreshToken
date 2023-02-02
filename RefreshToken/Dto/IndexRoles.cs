using System.ComponentModel.DataAnnotations;

namespace RefreshToken.Dto
{
    public class IndexRoles
    {
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
        public string search { get; set; }
        public string sortOrder { get; set; }
        public string filter { get; set; }
        public int page { get; set; }
    }
}
