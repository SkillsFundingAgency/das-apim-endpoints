using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidatePreferences;
public class GetCandidatePreferencesQueryHandler : IRequestHandler<GetCandidatePreferencesQuery, GetCandidatePreferencesQueryResult>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public GetCandidatePreferencesQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _candidateApiClient = candidateApiClient;
    }

    public async Task<GetCandidatePreferencesQueryResult> Handle(GetCandidatePreferencesQuery request, CancellationToken cancellationToken)
    {
        var result = await _candidateApiClient.Get<GetCandidatePreferencesApiResponse>(new GetCandidatePreferencesApiRequest(request.CandidateId));

        if (result is null)
        {
            return new GetCandidatePreferencesQueryResult
            {
                CandidatePreferences = new List<GetCandidatePreferencesQueryResult.CandidatePreference>()
            };
        }

        return new GetCandidatePreferencesQueryResult
        {
            CandidatePreferences = result.CandidatePreferences.Select(cp => new GetCandidatePreferencesQueryResult.CandidatePreference
            {
                PreferenceId = cp.PreferenceId,
                PreferenceMeaning = cp.PreferenceMeaning,
                PreferenceHint = cp.PreferenceHint,
                ContactMethodsAndStatus = cp.ContactMethodsAndStatus.Select(c => new GetCandidatePreferencesQueryResult.ContactMethodStatus
                {
                    ContactMethod = c.ContactMethod,
                    Status = c.Status
                }).ToList()
            }).ToList()
        };
    }
}
