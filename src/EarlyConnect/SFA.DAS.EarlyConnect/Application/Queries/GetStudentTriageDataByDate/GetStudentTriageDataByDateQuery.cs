using MediatR;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataByDate
{
    public class GetStudentTriageDataByDateQuery : IRequest<List<GetStudentTriageDataResult>>
    {
        public DateTime ToDate { get; set; }
        public DateTime FromDate { get; set; }
    }
}