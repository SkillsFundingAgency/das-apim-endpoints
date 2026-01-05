using SFA.DAS.EarlyConnect.Application.Commands.ManageStudentTriageData;

namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class ManageStudentTriageDataPostResponse
    {
        public string Message { get; set; }
        public static implicit operator ManageStudentTriageDataPostResponse(ManageStudentTriageDataCommandResult source)
        {
            return new ManageStudentTriageDataPostResponse
            {
                Message = source.Message,
            };
        }
    }
}
