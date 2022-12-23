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
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _attributesApiClient;

        public GetAttributesQueryHandler(IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> attributesApiClient)
        {
            _attributesApiClient = attributesApiClient;
        }

        public async Task<GetAttributesResult> Handle(GetAttributesQuery request, CancellationToken cancellationToken)
        {
            var apiResponse =
                await _attributesApiClient.Get<List<Attribute>>(
                    new GetAttributesRequest(request.AttributeType)); 

            return new GetAttributesResult
            {
                Attributes = apiResponse
            };
        }
    }
}
