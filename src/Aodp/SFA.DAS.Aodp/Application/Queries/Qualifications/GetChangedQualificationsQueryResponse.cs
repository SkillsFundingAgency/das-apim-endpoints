
namespace SFA.DAS.Aodp.Application.Queries.Qualifications;

public class GetChangedQualificationsQueryResponse
{

    public int TotalRecords { get; set; }
    public int? Skip { get; set; }
    public int Take { get; set; }
    public List<ChangedQualification> Data { get; set; } = new();
    public Job Job { get; set; } = new();

}
public class ChangedQualification
{
    public Guid Id { get; set; }
    public string QualificationReference { get; set; } = string.Empty;
    public string AwardingOrganisation { get; set; } = string.Empty;
    public string QualificationTitle { get; set; } = string.Empty;
    public string QualificationType { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string AgeGroup { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string SectorSubjectArea { get; set; } = string.Empty;
    public string ChangedFieldNames { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}
