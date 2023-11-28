using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateLogData
{
    public class CreateLogDataCommand :  IRequest<CreateLogDataCommandResult>
    {
        public LogCreate Log { get; set; }
    }
}