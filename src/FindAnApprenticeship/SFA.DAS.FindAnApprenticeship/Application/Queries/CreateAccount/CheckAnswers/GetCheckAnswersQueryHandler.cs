using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.CheckAnswers;

public class GetCheckAnswersQueryHandler(ICandidateApiClient<CandidateApiConfiguration> apiClient) : IRequestHandler<GetCheckAnswersQuery, GetCheckAnswersQueryResult>
{
    public async Task<GetCheckAnswersQueryResult> Handle(GetCheckAnswersQuery request, CancellationToken cancellationToken)
    {
        var candidate = await apiClient.Get<GetCandidateApiResponse>(new GetCandidateApiRequest(request.CandidateId));
        var preferences = await apiClient.Get<GetCandidatePreferencesApiResponse>(new GetCandidatePreferencesApiRequest(request.CandidateId));
        var address = await apiClient.Get<GetCandidateAddressApiResponse>(new GetCandidateAddressApiRequest(request.CandidateId));

        address ??= new GetCandidateAddressApiResponse();

        return new GetCheckAnswersQueryResult
        {
            FirstName = candidate.FirstName,
            MiddleNames = candidate.MiddleNames,
            LastName = candidate.LastName,
            DateOfBirth = candidate.DateOfBirth,
            AddressLine1 = address.AddressLine1,
            AddressLine2 = address.AddressLine2,
            Town = address.Town,
            County = address.County,
            Postcode = address.Postcode,
            CandidatePreferences = preferences.CandidatePreferences == null
                ? new List<GetCheckAnswersQueryResult.CandidatePreference>()
                : preferences.CandidatePreferences.Select(cp => new GetCheckAnswersQueryResult.CandidatePreference
                {
                    PreferenceId = cp.PreferenceId,
                    PreferenceMeaning = cp.PreferenceMeaning,
                    PreferenceHint = cp.PreferenceHint,
                    ContactMethodsAndStatus = cp.ContactMethodsAndStatus.Select(c => new GetCheckAnswersQueryResult.ContactMethodStatus
                    {
                        ContactMethod = c.ContactMethod,
                        Status = c.Status
                    }).ToList()
                }).ToList()
        };
    }
}