using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.DTO
{
    public class SPM_UserDTO
    {
        public int UserID { get; set; }

        public int UserTypeID { get; set; }

      
        public string FirstName { get; set; } = string.Empty;

     
        public string LastName { get; set; } = string.Empty;

       
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

      
        public string MobileNo { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; } = false;
    }
}
