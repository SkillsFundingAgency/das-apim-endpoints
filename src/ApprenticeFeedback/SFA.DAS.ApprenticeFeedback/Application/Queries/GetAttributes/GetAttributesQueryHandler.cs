using MediatR;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetAttributes
{
    public class GetAttributesQueryHandler : IRequestHandler<GetAttributesQuery, GetAttributesResult>
    {
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _providerAttributesApiClient;

        public GetAttributesQueryHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> providerAttributesApiClient)
        {
            _providerAttributesApiClient = providerAttributesApiClient;
        }

        public async Task<GetAttributesResult> Handle(GetAttributesQuery request, CancellationToken cancellationToken)
        {
            var apiResponse =
                await _providerAttributesApiClient.Get<List<Attribute>>(
                    new GetAttributesRequest()); 

            return new GetAttributesResult
            {
                Attributes = apiResponse
            };

    }
    }
}
