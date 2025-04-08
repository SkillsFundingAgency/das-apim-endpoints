using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetEmploymentLocations
{
    public class GetEmploymentLocationsQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        : IRequestHandler<GetEmploymentLocationsQuery, GetEmploymentLocationsQueryResult>
    {
        public async Task<GetEmploymentLocationsQueryResult> Handle(GetEmploymentLocationsQuery request, CancellationToken cancellationToken)
        {
            var employmentLocationsApiRequest = new GetEmploymentLocationsApiRequest(request.CandidateId, request.ApplicationId);
            return await candidateApiClient.Get<GetEmploymentLocationsApiResponse>(employmentLocationsApiRequest);
        }
    }
}