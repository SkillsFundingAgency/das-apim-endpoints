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
            var applicationRequest = new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false);
            var application = await candidateApiClient.Get<GetApplicationApiResponse>(applicationRequest);

            if (application == null)
            {
                return null;
            }

            bool? isCompleted = application.EmploymentLocationStatus switch
            {
                Domain.Constants.SectionStatus.Incomplete => false,
                Domain.Constants.SectionStatus.Completed => true,
                _ => null
            };

            return new GetEmploymentLocationsQueryResult
            {
                EmploymentLocation = application.EmploymentLocation,
                IsSectionCompleted = isCompleted
            };
        }
    }
}