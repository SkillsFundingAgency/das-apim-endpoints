using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.Approvals.InnerApi.Responses.ProviderEvent;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.CommitmentPayment.Queries.GetDataLockEvents
{
    public class GetDataLockStatusesQueryHandler : IRequestHandler<GetDataLockStatuesQuery, GetDataLockStatusesQueryResult>
    {
        public readonly IProviderPaymentEventsApiClient<ProviderEventsConfiguration> _providerPaymentEventsApiClient;

        public GetDataLockStatusesQueryHandler(IProviderPaymentEventsApiClient<ProviderEventsConfiguration> providerPaymentEventsApiClient)
        {
            _providerPaymentEventsApiClient = providerPaymentEventsApiClient;
        }

        public async Task<GetDataLockStatusesQueryResult> Handle(GetDataLockStatuesQuery query, CancellationToken cancellationToken)
        {
            var response = await _providerPaymentEventsApiClient.GetWithResponseCode<PageOfResults<DataLockStatusEvent>>(new GetDataLockEventsRequest
            {
                SinceEventId = query.SinceEventId,
                SinceTime = query.SinceTime,
                EmployerAccountId = query.EmployerAccountId,
                Ukprn = query.Ukprn,
                PageNumber = query.PageNumber
            });

            response.EnsureSuccessStatusCode();

            return new GetDataLockStatusesQueryResult()
            {
                PagedDataLockEvent = response.Body ?? new PageOfResults<DataLockStatusEvent>()
            };
        }
    }
}