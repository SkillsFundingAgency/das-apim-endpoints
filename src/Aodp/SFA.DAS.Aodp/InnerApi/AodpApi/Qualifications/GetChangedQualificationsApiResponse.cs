using SFA.DAS.Aodp.Application.Queries.Qualifications;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications
{
    public class GetChangedQualificationsApiResponse
    {
        public int TotalRecords { get; set; }
        public int? Skip { get; set; }
        public int Take { get; set; }
        public List<ChangedQualification> Data { get; set; } = new();
    }
}
