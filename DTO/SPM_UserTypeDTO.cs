using System.ComponentModel.DataAnnotations;

namespace StudentProjectAPI.DTO
{
    public class SPM_UserTypeDTO
    {
        public int UserTypeID { get; set; }

        [Required]
        public string UserTypeName { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}
