using MediatR;

namespace SFA.DAS.EarlyConnect.Application.Commands.UpdateLog
{
    public class UpdateLogCommand : IRequest<Unit>
    {
        public InnerApi.Requests.UpdateLog Log { get; set; }
    }
}