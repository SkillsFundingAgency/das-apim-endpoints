using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Campaign.Models;

[ExcludeFromCodeCoverage]
public class EnquiryUserDataModel
{
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9](?:[a-zA-Z0-9.,'_ -]*[a-zA-Z0-9])?$", ErrorMessage = "First Name must be alphanumeric.")]
    public string FirstName { get; set; }

    [Required]
    [RegularExpression(@"^[a-zA-Z0-9](?:[a-zA-Z0-9.,'_ -]*[a-zA-Z0-9])?$", ErrorMessage = "Last Name must be alphanumeric.")]
    public string LastName { get; set; }

    [EmailAddress]
    [Required]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email address.")]
    public string Email { get; set; }

    [Required]
    [RegularExpression(@"^[a-zA-Z0-9](?:[a-zA-Z0-9.,'_ -]*[a-zA-Z0-9])?$", ErrorMessage = "Uk Employer Size must be alphanumeric.")]
    public string UkEmployerSize { get; set; }

    [Required]
    [RegularExpression(@"^[a-zA-Z0-9](?:[a-zA-Z0-9.,'_ -]*[a-zA-Z0-9])?$", ErrorMessage = "Primary Industry must be alphanumeric.")]
    public string PrimaryIndustry { get; set; }

    [Required]
    [RegularExpression(@"^[a-zA-Z0-9](?:[a-zA-Z0-9.,'_ -]*[a-zA-Z0-9])?$", ErrorMessage = "Primary Location must be alphanumeric.")]
    public string PrimaryLocation { get; set; }

    [Required]
    public DateTime AppsgovSignUpDate { get; set; }

    [Required]
    [RegularExpression(@"^[a-zA-Z0-9](?:[a-zA-Z0-9.,'_ -]*[a-zA-Z0-9])?$", ErrorMessage = "Person Origin must be alphanumeric.")]
    public string PersonOrigin { get; set; }

    [Required]
    public bool IncludeInUR { get; set; }
}
