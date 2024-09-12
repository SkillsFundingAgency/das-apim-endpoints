﻿namespace SFA.DAS.ProviderPR.InnerApi.Responses;
public class GetPensionRegulatorOrganisationResponse
{
    public string? Name { get; set; }
    public string? Status { get; set; }
    public long UniqueIdentity { get; set; }
    public Address? Address { get; set; }
}

public class Address
{
    public string? Line1 { get; set; }
    public string? Line2 { get; set; }
    public string? Line3 { get; set; }
    public string? Line4 { get; set; }
    public string? Line5 { get; set; }
    public string? Postcode { get; set; }
}