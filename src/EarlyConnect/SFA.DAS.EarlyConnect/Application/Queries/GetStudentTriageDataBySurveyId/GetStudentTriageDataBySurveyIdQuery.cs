using MediatR;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId
{
    public class GetStudentTriageDataBySurveyIdQuery : IRequest<StudentTriageDataShared>
    {
        public Guid SurveyGuid { get; set; }
    }
}