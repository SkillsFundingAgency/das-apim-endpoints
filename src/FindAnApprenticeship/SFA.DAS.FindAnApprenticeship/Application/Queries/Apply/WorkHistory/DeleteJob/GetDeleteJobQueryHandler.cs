using MediatR;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetJob;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory.DeleteJob
{
    public class GetDeleteJobQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<GetDeleteJobQuery, GetDeleteJobQueryResult>
    {
        public async Task<GetDeleteJobQueryResult> Handle(GetDeleteJobQuery request, CancellationToken cancellationToken)
        {
            return await candidateApiClient.Get<GetDeleteJobApiResponse>(new GetDeleteJobApiRequest(request.ApplicationId, request.CandidateId, request.JobId, WorkHistoryType.Job));
        }
    }
}
