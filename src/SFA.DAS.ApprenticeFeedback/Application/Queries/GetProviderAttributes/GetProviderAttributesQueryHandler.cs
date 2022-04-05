using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetProviderAttributes
{
    public class GetProviderAttributesQueryHandler : IRequestHandler<GetProviderAttributesQuery, GetProviderAttributesResult>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _providerAttributesApiClient;

        public GetProviderAttributesQueryHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> providerAttributesApiClient)
        {
            _providerAttributesApiClient = providerAttributesApiClient;
        }

        public async Task<GetProviderAttributesResult> Handle(GetProviderAttributesQuery request, CancellationToken cancellationToken)
        {
            var apiResponse =
                await _providerAttributesApiClient.Get<List<ProviderAttribute>>(
                    new GetProviderAttributesRequest()); 

            return new GetProviderAttributesResult
            {
                ProviderAttributes = apiResponse
            };

    }
    }
}
