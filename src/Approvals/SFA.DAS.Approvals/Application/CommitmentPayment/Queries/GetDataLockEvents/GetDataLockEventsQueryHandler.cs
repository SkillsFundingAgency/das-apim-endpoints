using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderEvent;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.CommitmentPayment.Queries.GetDataLockEvents
{
    public class GetDataLockEventsQueryHandler : IRequestHandler<GetDataLockEventsQuery, GetDataLockEventsQueryResult>
    {
        public readonly IProviderPaymentEventsApiClient<ProviderEventsConfiguration> _providerPaymentEventsApiClient;

        public GetDataLockEventsQueryHandler(IProviderPaymentEventsApiClient<ProviderEventsConfiguration> providerPaymentEventsApiClient)
        {
            _providerPaymentEventsApiClient = providerPaymentEventsApiClient;
        }

        public async Task<GetDataLockEventsQueryResult> Handle(GetDataLockEventsQuery query, CancellationToken cancellationToken)
        {
            var response = await _providerPaymentEventsApiClient.Get<PageOfResults<GetDataLockEventsResponse>>(new GetDataLockEventsRequest
            {
                SinceEventId = query.SinceEventId,
                SinceTime = query.SinceTime,
                EmployerAccountId = query.EmployerAccountId,
                Ukprn = query.Ukprn,
                PageNumber = query.PageNumber
            });

            return new GetDataLockEventsQueryResult()
            {
                PagedDataLockEvent = response
            };
        }
    }
}