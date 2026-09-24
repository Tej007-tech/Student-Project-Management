using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentProjectAPI.Models
{
    public class SPM_UserRole
    {
        [Key]
        public int RolePermissionID { get; set; }

        [ForeignKey("Role")]
        public int RoleID { get; set; }

        public SPM_Role? Role { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public SPM_User? User { get; set; }
    }
}