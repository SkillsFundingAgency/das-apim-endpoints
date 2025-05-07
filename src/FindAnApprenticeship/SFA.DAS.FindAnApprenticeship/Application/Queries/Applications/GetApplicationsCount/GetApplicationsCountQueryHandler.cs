using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplicationsCount
{
    public class GetApplicationsCountQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        : IRequestHandler<GetApplicationsCountQuery, GetApplicationsCountQueryResult>
    {
        public async Task<GetApplicationsCountQueryResult> Handle(GetApplicationsCountQuery request, CancellationToken cancellationToken)
        {
            var response = await candidateApiClient.PostWithResponseCode<PostApplicationsCountApiResponse>(new PostApplicationsCountApiRequest(request.CandidateId, new PostApplicationsCountApiRequest.PostApplicationsCountApiRequestData(request.Statuses)));

            return response.Body;
        }
    }
}
