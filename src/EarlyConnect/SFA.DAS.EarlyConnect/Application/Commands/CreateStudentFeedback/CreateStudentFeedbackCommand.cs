using MediatR;
using SFA.DAS.EarlyConnect.Application.Commands.CreateStudentFeedback;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateStudentFeedback
{
    public  class CreateStudentFeedbackCommand : IRequest<CreateStudentFeedbackCommandResult>
    {
        public string RequestIP { get; set; }
        public string Payload { get; set; }
        public string RequestType { get; set; }
        public string RequestSource { get; set; }
        public StudentFeedbackList StudentFeedbackList { get; set; }
    }
}
