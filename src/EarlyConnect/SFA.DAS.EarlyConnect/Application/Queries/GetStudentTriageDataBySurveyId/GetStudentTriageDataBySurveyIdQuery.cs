using MediatR;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId
{
    public class GetStudentTriageDataBySurveyIdQuery : IRequest<GetStudentTriageDataResult>
    {
        public Guid SurveyGuid { get; set; }
    }
}