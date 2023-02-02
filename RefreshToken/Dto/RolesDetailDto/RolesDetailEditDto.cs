using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RefreshToken.Dto.RolesDetailDto
{
    public class RolesDetailEditDto
    {
        public int IdRolesDetail { get; set; }
        public int IdUser { get; set; }
        public int IdRoles { get; set; }
        public string UserModified { get; set; }
        public DateTime DateModified { get; set; }
    }
}
