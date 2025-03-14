namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetChangedQualificationsApiResponse
    {
        public int TotalRecords { get; set; }
        public int? Skip { get; set; }
        public int Take { get; set; }       
        public List<ChangedQualification> Data { get; set; } = new();       
    }   
}
