namespace SFA.DAS.Recruit.InnerApi.Models;

public record EmployerProfileAddress(int Id,
    string AddressLine1,
    string? AddressLine2,
    string? AddressLine3,
    string? AddressLine4,
    string Postcode,
    double? Latitude,
    double? Longitude);