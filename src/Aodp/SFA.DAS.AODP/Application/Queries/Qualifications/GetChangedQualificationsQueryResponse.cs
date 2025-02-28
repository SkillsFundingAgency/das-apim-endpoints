
namespace SFA.DAS.Aodp.Application.Queries.Qualifications;

public class GetChangedQualificationsQueryResponse
{
    public List<ChangedQualification> Data { get; set; } = new();

    public class ChangedQualification
    {
        public Guid Id { get; set; }
        public Guid Qan { get; set; }
        public string QualificationName { get; set; } = string.Empty;
    }
}
