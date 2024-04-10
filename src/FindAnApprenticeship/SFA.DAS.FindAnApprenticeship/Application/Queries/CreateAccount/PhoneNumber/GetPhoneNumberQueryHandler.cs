using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.PhoneNumber;

public class GetPhoneNumberQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    : IRequestHandler<GetPhoneNumberQuery, GetPhoneNumberQueryResult>
{
    public async Task<GetPhoneNumberQueryResult> Handle(GetPhoneNumberQuery request, CancellationToken cancellationToken)
    {
        return await candidateApiClient.Get<GetCandidateApiResponse>(new GetCandidateApiRequest(request.CandidateId));
    }
}