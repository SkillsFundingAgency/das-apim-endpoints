using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class StudentRequestModel
    {
        [Required]
        [RegularExpression(@"^[\w\s]+$", ErrorMessage = "Invalid First Name")]
        public string FirstName { get; set; }

        [Required]
        [RegularExpression(@"^[\w\s]+$", ErrorMessage = "Invalid Last Name")]
        public string LastName { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [EmailAddressAttribute(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [RegularExpression(@"^[\w\s]+$", ErrorMessage = "Invalid Telephone")]
        public string Telephone { get; set; }

        [RegularExpression(@"^[\w\s]+$", ErrorMessage = "Invalid SchoolName")]
        public string SchoolName { get; set; }

        [Required]
        [RegularExpression(@"^[\w\s]+$", ErrorMessage = "Invalid Postcode")]
        public string Postcode { get; set; }

        [Required]
        [RegularExpression(@"^[\w\s|()]+$", ErrorMessage = "Invalid Industry")]
        public string Industry { get; set; }

        public DateTime DateOfInterest { get; set; }
    }
}