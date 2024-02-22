using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.GetWorkExperiences;

public class GetVolunteeringAndWorkExperiencesQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    : IRequestHandler<GetVolunteeringAndWorkExperiencesQuery, GetVolunteeringAndWorkExperiencesQueryResult>
{
    public async Task<GetVolunteeringAndWorkExperiencesQueryResult> Handle(GetVolunteeringAndWorkExperiencesQuery request, CancellationToken cancellationToken)
    {
        return await candidateApiClient.Get<GetWorkHistoriesApiResponse>(new GetWorkHistoriesApiRequest(request.ApplicationId, request.CandidateId, WorkHistoryType.WorkExperience));
    }
}