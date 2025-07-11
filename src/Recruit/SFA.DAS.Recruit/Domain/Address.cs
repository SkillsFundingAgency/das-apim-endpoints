namespace SFA.DAS.Recruit.Domain;
public record Address
{
    public string? AddressLine1 { get; init; }
    public string? AddressLine2 { get; init; }
    public string? Town { get; init; }
    public string? County { get; init; }
    public string? Postcode { get; init; }
}