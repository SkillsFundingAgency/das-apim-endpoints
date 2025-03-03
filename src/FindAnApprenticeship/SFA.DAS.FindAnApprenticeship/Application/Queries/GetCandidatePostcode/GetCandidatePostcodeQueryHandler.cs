using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidatePostcode;

public class GetCandidatePostcodeQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<GetCandidatePostcodeQuery, GetCandidatePostcodeQueryResult>
{
    public async Task<GetCandidatePostcodeQueryResult> Handle(GetCandidatePostcodeQuery request, CancellationToken cancellationToken)
    {
        var result =
            await candidateApiClient.GetWithResponseCode<GetCandidateAddressApiResponse>(
                new GetCandidateAddressApiRequest(request.CandidateId));

        if (result.StatusCode == HttpStatusCode.NotFound)
        {
            return new GetCandidatePostcodeQueryResult
            {
                Postcode = null
            };
        }
        
        return new GetCandidatePostcodeQueryResult
        {
            Postcode = result.Body.Postcode 
        };
    }
}