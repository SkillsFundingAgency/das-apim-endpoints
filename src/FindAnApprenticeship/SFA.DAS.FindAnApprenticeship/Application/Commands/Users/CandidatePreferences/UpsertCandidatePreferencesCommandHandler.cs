using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.CandidatePreferences;
public class UpsertCandidatePreferencesCommandHandler : IRequestHandler<UpsertCandidatePreferencesCommand, Unit>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public UpsertCandidatePreferencesCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _candidateApiClient = candidateApiClient;
    }

    public async Task<Unit> Handle(UpsertCandidatePreferencesCommand request, CancellationToken cancellationToken)
    {
        var candidatePreferencesRequestData = new List<PutCandidatePreferencesRequestData.CandidatePreference>();
        foreach (var candidatePreference in request.CandidatePreferences)
        {
            var seperatedCandidatePreferences = candidatePreference.ContactMethodsAndStatus.Select(x => new PutCandidatePreferencesRequestData.CandidatePreference
            {
                PreferenceId = candidatePreference.PreferenceId,
                PreferenceMeaning = candidatePreference.PreferenceMeaning,
                PreferenceHint = candidatePreference.PreferenceHint,
                ContactMethod = x.ContactMethod,
                Status = x.Status
            }).ToList();

            candidatePreferencesRequestData = candidatePreferencesRequestData.Concat(seperatedCandidatePreferences).ToList();
        }

        var apiRequest = new PutCandidatePreferencesApiRequest(request.CandidateId, new PutCandidatePreferencesRequestData
        { CandidatePreferences = candidatePreferencesRequestData });

         await _candidateApiClient.PutWithResponseCode<NullResponse>(apiRequest);
        return Unit.Value;
    }
}
