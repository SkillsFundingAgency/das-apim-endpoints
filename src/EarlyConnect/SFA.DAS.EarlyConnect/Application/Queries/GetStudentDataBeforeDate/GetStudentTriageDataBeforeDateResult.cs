using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.EarlyConnect.Models;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataByDate
{
    public class GetStudentTriageDataByDateResult
    {
       public List<GetStudentTriageDataBySurveyIdResult> StudentTriageDataResults { get; set; } // get a list 
    }
}