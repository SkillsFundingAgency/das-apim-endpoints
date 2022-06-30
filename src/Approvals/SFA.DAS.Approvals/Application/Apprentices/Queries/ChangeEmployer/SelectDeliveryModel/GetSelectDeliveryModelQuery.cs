using MediatR;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.SelectDeliveryModel
{
    public class GetSelectDeliveryModelQuery : IRequest<GetSelectDeliveryModelResult>
    {
        public long ApprenticeshipId { get; set; }
        public long ProviderId { get; set; }
    }
}
