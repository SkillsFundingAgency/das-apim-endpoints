using MediatR;

namespace SFA.DAS.EarlyConnect.Application.Commands.CreateLog
{
    public class CreateLogCommand : IRequest<Unit>
    {
        public InnerApi.Requests.CreateLog Log { get; set; }
    }
}