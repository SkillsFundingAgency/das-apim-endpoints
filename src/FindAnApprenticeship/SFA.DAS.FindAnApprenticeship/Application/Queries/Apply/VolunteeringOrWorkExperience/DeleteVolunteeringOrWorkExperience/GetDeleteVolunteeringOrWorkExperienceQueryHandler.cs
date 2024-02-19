using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.DeleteVolunteeringOrWorkExperience;
public class GetDeleteVolunteeringOrWorkExperienceQueryHandler (ICandidateApiClient<CandidateApiConfiguration> ApiClient) : IRequestHandler<GetDeleteVolunteeringOrWorkExperienceQuery, GetDeleteVolunteeringOrWorkExperienceQueryResult>
{
    public async Task<GetDeleteVolunteeringOrWorkExperienceQueryResult> Handle(GetDeleteVolunteeringOrWorkExperienceQuery request, CancellationToken cancellationToken)
    {
        return await ApiClient.Get<GetDeleteVolunteeringOrWorkExperienceApiResponse>(new GetDeleteVolunteeringOrWorkExperienceApiRequest(request.ApplicationId, request.CandidateId, request.Id));
    }
}
