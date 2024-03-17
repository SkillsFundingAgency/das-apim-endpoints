using System.Linq;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Domain;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.GetWorkExperiences;

public class GetVolunteeringAndWorkExperiencesQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    : IRequestHandler<GetVolunteeringAndWorkExperiencesQuery, GetVolunteeringAndWorkExperiencesQueryResult>
{
    public async Task<GetVolunteeringAndWorkExperiencesQueryResult> Handle(GetVolunteeringAndWorkExperiencesQuery request, CancellationToken cancellationToken)
    {
        var applicationRequest = new GetApplicationApiRequest(request.CandidateId, request.ApplicationId);
        var application = await candidateApiClient.Get<GetApplicationApiResponse>(applicationRequest);

        var workExperience = await candidateApiClient.Get<GetWorkHistoriesApiResponse>(
            new GetWorkHistoriesApiRequest(request.ApplicationId, request.CandidateId, WorkHistoryType.WorkExperience));

        bool? isCompleted = application.WorkExperienceStatus switch
        {
            Constants.SectionStatus.Incomplete => false,
            Constants.SectionStatus.Completed => true,
            _ => null
        };

        return new GetVolunteeringAndWorkExperiencesQueryResult
        {
            IsSectionCompleted = isCompleted,
            VolunteeringAndWorkExperiences = workExperience.WorkHistories
                .Select(x => (GetVolunteeringAndWorkExperiencesQueryResult.VolunteeringAndWorkExperience)x).ToList()
        };
    }
}