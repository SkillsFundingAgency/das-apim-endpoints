﻿using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory.DeleteJob
{
    public class GetDeleteJobQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<GetDeleteJobQuery, GetDeleteJobQueryResult>
    {
        public async Task<GetDeleteJobQueryResult> Handle(GetDeleteJobQuery request, CancellationToken cancellationToken)
        {
            return await candidateApiClient.Get<GetWorkHistoryItemApiResponse>(new GetWorkHistoryItemApiRequest(request.ApplicationId, request.CandidateId, request.JobId, WorkHistoryType.Job));
        }
    }
}
