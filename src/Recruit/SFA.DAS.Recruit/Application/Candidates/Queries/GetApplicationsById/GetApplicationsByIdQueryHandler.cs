using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Candidates.Queries.GetApplicationsById
{
    public class GetApplicationsByIdQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<GetApplicationsByIdQuery, GetApplicationsByIdQueryResult>
    {
        public async Task<GetApplicationsByIdQueryResult> Handle(GetApplicationsByIdQuery request, CancellationToken cancellationToken)
        {
            var response = await candidateApiClient.PostWithResponseCode<GetAllApplicationsByIdApiResponse>(new GetAllApplicationsByIdApiRequest(new GetAllApplicationsByIdApiRequestData
            {
                ApplicationIds = request.ApplicationIds,
                IncludeDetails = request.IncludeDetails
            }), true);

            response.EnsureSuccessStatusCode();

            return new GetApplicationsByIdQueryResult(response.Body.Applications);
        }
    }
}