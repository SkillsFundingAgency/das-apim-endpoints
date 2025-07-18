using MediatR;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataByDate
{
    public class GetStudentTriageDataByDateQuery : IRequest<List<StudentTriageDataShared>>
    {
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
    }
}