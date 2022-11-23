using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderEvent;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.PaymentEvents.Queries
{
    public class GetPaymentSubmissionEventsQueryHandler : IRequestHandler<GetPaymentSubmissionEventsQuery, PageOfResults<SubmissionEvent>>
    {
        public readonly IProviderPaymentEventsApiClient<ProviderEventsConfiguration> _providerPaymentEventsApiClient;
        public GetPaymentSubmissionEventsQueryHandler(IProviderPaymentEventsApiClient<ProviderEventsConfiguration> providerPaymentEventsApiClient)
        {
            _providerPaymentEventsApiClient = providerPaymentEventsApiClient;
        }

        public async Task<PageOfResults<SubmissionEvent>> Handle(GetPaymentSubmissionEventsQuery query, CancellationToken cancellationToken)
        {
            var response = await _providerPaymentEventsApiClient.Get<PageOfResults<SubmissionEvent>>(new GetSubmissionsEventsRequest
            {
                SinceEventId = query.SinceEventId,
                SinceTime = query.SinceTime,
                Ukprn = query.Ukprn,
                PageNumber = query.Page
            });

            return response; 
        }
    }
}
