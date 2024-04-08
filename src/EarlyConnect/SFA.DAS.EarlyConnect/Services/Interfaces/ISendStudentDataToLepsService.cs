
namespace SFA.DAS.EarlyConnect.Services.Interfaces
{
    public interface ISendStudentDataToLepsService
    {
        Task<SendStudentDataToLepsServiceResponse> SendStudentDataToLeps(Guid SurveyGuid);
    }
}
