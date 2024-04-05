using SFA.DAS.EarlyConnect.Web.Infrastructure;

namespace SFA.DAS.EarlyConnect.Services
{
    public interface ISendStudentDataToLepsService
    {
        Task<SendStudentDataToLepsServiceResponse> SendStudentDataToLeps(Guid SurveyGuid);
    }
}
