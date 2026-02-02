using System;

namespace SFA.DAS.Campaign.Models;

public class EnquiryUserDataModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string UkEmployerSize { get; set; }
    public string PrimaryIndustry { get; set; }
    public string PrimaryLocation { get; set; }
    public DateTime AppsgovSignUpDate { get; set; }
    public string PersonOrigin { get; set; }
    public bool IncludeInUR { get; set; }
}
