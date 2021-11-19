using MediatR;

namespace SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Commands.RenewSubscriptionKey
{
    public class RenewSubscriptionKeyCommand : IRequest<Unit>
    {
        public string AccountIdentifier { get; set; }
        public string ProductId { get; set; }
    }
}