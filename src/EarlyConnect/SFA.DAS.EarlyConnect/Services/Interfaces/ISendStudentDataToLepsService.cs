
namespace SFA.DAS.EarlyConnect.Services.Interfaces
{
    public interface ISendStudentDataToLepsService
    {
        Task<SendStudentDataToLepsServiceResponse> SendStudentDataToNe(Guid SurveyGuid);
        Task<SendStudentDataToLepsServiceResponse> SendStudentDataToLo(Guid SurveyGuid);
    }
}
