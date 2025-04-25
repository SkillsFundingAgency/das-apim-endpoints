using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class GetStudentTriageDataByDateRequest : IGetApiRequest
    {
        public GetStudentTriageDataByDateRequest() { }

        public string GetUrl => $"/api/student-triage-data/resenddatatolondon";
    }
}