using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData
{
    public class CreateOtherStudentTriageDataCommand : IRequest<CreateOtherStudentTriageDataCommandResult>
    {
        public OtherStudentTriageData StudentTriageData { get; set; }
    }
}