using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.ProviderUsers.Commands
{
    public class ProviderEmailCommand : IRequest<Unit>
    {
        public long ProviderId { get; set; }
        public ProviderEmailRequest ProviderEmailRequest { get; set; }
    }
}
