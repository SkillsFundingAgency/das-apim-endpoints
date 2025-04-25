using MediatR;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataByDate;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;

namespace SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataByDate
{
    public class GetStudentDataBeforeDateQuery : IRequest<List<GetStudentTriageDataBySurveyIdResult>>
    {
      
    }
}