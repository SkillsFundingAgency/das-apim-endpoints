using System;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Roatp.Domain.Models;

public class Charity
{
    public string RegistrationNumber { get; set; }
    public string CharityNumber { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public DateTime? RemovalDate { get; set; }
    public List<Trustee> Trustees { get; set; }

    public static implicit operator Charity(GetCharityResponse charityResponse)
        => new Charity()
        {
            CharityNumber = charityResponse.Id.ToString(),
            RegistrationDate = charityResponse.RegistrationDate,
            RegistrationNumber = charityResponse.RegistrationNumber.ToString(),
            Name = charityResponse.Name,
            Status = charityResponse.RegistrationStatus,
            Type = charityResponse.CharityType,
            RemovalDate = charityResponse.RemovalDate,
            Trustees = charityResponse.Trustees.Select(c => (Trustee)c).ToList()
        };
}
