using MediatR;

namespace SFA.DAS.ApimDeveloper.Application.ApiSubscriptions.Commands.DeleteSubscriptionKey
{
    public class DeleteSubscriptionKeyCommand : IRequest<Unit>
    {
        public string AccountIdentifier { get; set; }
        public string ProductId { get; set; }
    }
}
