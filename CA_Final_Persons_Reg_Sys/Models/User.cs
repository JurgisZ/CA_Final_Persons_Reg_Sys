using System.ComponentModel.DataAnnotations;

namespace CA_Final_Persons_Reg_Sys.Model
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public required string UserName { get; set; }

        [Required]
        public required byte[] PasswordHash {  get; set; }

        [Required]
        public required byte[] PasswordSalt {  get; set; }
        
        [Required]
        [MaxLength(127)]
        public string Role { get; set; } = "user";      //Move to config json

        [Required]
        public long UserPersonalDataId { get; set; }

        [Required]
        public UserPersonalData UserPersonalData { get; set; }

    }
}
