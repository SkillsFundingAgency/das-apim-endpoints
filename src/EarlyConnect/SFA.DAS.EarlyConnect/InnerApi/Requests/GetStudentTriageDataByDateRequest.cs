using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class GetStudentTriageDataByDateRequest : IGetApiRequest
    {
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
        
        public GetStudentTriageDataByDateRequest(DateTime toDate, DateTime fromDate)
        {
            ToDate = toDate;
            FromDate = fromDate;
        }
        public string GetUrl => $"/api/student-triage-data/resenddatatolondon?toDate={ToDate}&fromDate={FromDate}";
    }
}