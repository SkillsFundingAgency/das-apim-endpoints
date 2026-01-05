using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateStudentData
{
    public class CreateStudentDataCommand : IRequest<CreateStudentDataCommandResult>
    {
        public string RequestIP { get; set; }
        public string Payload { get; set; }
        public string RequestType { get; set; }
        public string RequestSource { get; set; }
        public StudentDataList StudentDataList { get; set; }
    }
}