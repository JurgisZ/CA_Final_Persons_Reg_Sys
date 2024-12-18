using System.ComponentModel.DataAnnotations;

namespace CA_Final_Persons_Reg_Sys.Dtos
{
    public class UserPersonalDataResult
    {
        [Required]
        public long Id { get; set; }

        [Required]
        [MaxLength(255)]    //1 byte vs 256 - 2 bytes
        public required string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public required string LastName { get; set; }

        [Required]
        [MaxLength(20)] //Lietuva - 11 symbols
        public required string PersonalCode { get; set; }

        [Required]
        [MaxLength(15)] //The maximum length for mobile phone numbers can go up to 15 digits, as per the International Telecommunication Union (ITU) recommendation. This limit includes the country code and the national number.
        public required string PhoneNumber { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Email { get; set; }

        [Required]
        [MaxLength(255)]    //Microsoft Windows has a MAX_PATH limit of ~256 characters
        public required string ProfilePicture { get; set; }  //200 x 200 px scale to fit

        [Required]
        [MaxLength(255)]
        public required string CityName { get; set; }

        [Required]
        [MaxLength(255)]
        public required string StreetName { get; set; }

        [Required]
        [MaxLength(20)]
        public required string HouseNumber { get; set; }

        [Required]
        [MaxLength(20)]
        public required string ApartmentNumber { get; set; }

    }
}
