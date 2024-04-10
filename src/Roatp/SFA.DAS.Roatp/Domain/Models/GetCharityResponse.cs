using System;
using System.Collections.Generic;

namespace SFA.DAS.Roatp.Domain.Models;
public class GetCharityResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string CompaniesHouseNumber { get; set; }
    public int RegistrationNumber { get; set; }
    public int LinkedCharityId { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string RegistrationStatus { get; set; }
    public string CharityType { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string AddressLine5 { get; set; }
    public string Postcode { get; set; }
    public bool IsInsolvent { get; set; }
    public bool IsInAdministration { get; set; }
    public bool? WasPreviouslyExcepted { get; set; }
    public DateTime? RemovalDate { get; set; }
    public List<CharityTrustee> Trustees { get; set; } = new List<CharityTrustee>();
}

public class CharityTrustee
{
    public int Id { get; set; }
    public int CharityId { get; set; }
    public int RegistrationNumber { get; set; }
    public int TrusteeId { get; set; }
    public string Name { get; set; }
    public bool IsChair { get; set; }
    public string TrusteeType { get; set; }
    public DateTime? AppointmentDate { get; set; }
}

