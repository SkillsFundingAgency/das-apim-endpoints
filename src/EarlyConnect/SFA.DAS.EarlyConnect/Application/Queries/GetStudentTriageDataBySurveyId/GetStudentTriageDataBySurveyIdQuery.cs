using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId
{
    public class GetStudentTriageDataBySurveyIdQuery : IRequest<GetStudentTriageDataResponse>
    {
        public Guid SurveyGuid { get; set; }
    }
}