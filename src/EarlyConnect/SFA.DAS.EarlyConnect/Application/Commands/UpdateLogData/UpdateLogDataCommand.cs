using MediatR;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.Application.Commands.UpdateLogData
{
    public class UpdateLogDataCommand : IRequest<Unit>
    {
        public LogUpdate Log { get; set; }
    }
}