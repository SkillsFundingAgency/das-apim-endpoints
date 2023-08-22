namespace SFA.DAS.AdminAan.Application.Schools.Queries;
public class School
{
    public string Name { get; set; } = null!;
    public string? EducationalType { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? AddressLine3 { get; set; }
    public string? Town { get; set; }
    public string? County { get; set; }
    public string? PostCode { get; set; }
    public string Urn { get; set; } = null!;
}
