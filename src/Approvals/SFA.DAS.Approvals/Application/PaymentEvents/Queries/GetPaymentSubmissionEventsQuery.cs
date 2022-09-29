using System;
using MediatR;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderEvent;

namespace SFA.DAS.Approvals.Application.PaymentEvents.Queries
{
    public class GetPaymentSubmissionEventsQuery : IRequest<PageOfResults<SubmissionEvent>>
    {
        public long SinceEventId { get; set; }
        public DateTime? SinceTime { get; set; }
        public long Ukprn { get; set; }
        public int Page { get; set; }
    }
}
