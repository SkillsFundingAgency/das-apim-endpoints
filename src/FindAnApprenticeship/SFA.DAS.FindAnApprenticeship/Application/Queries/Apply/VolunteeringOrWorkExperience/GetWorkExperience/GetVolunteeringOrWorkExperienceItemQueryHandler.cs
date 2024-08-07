﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.GetWorkExperience;

public class GetVolunteeringOrWorkExperienceItemQueryHandler (ICandidateApiClient<CandidateApiConfiguration> apiClient) : IRequestHandler<GetVolunteeringOrWorkExperienceItemQuery, GetVolunteeringOrWorkExperienceItemQueryResult>
{
    public async Task<GetVolunteeringOrWorkExperienceItemQueryResult> Handle(GetVolunteeringOrWorkExperienceItemQuery request, CancellationToken cancellationToken)
    {
        var getVolunteeringOrWorkExperienceItemQueryResult = await apiClient.Get<GetWorkHistoryItemApiResponse>(new GetWorkHistoryItemApiRequest(request.ApplicationId, request.CandidateId, request.Id, WorkHistoryType.WorkExperience));
        return getVolunteeringOrWorkExperienceItemQueryResult ?? new GetVolunteeringOrWorkExperienceItemQueryResult();
    }
}