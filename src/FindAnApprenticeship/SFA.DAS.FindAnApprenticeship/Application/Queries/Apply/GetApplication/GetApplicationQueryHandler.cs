using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication
{
    public class GetApplicationQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        : IRequestHandler<GetApplicationQuery, GetApplicationQueryResult>
    {
        public async Task<GetApplicationQueryResult> Handle(
            GetApplicationQuery request,
            CancellationToken cancellationToken)
        {
            var candidateTask =
                candidateApiClient.Get<GetCandidateApiResponse>(
                    new GetCandidateApiRequest(request.CandidateId.ToString()));
            var addressTask =
                candidateApiClient.Get<GetAddressApiResponse>(
                    new GetCandidateAddressApiRequest(request.CandidateId));

            await Task.WhenAll(candidateTask, addressTask);

            var candidate = candidateTask.Result;
            var address = addressTask.Result;

            return new GetApplicationQueryResult
            {
                CandidateDetails = new GetApplicationQueryResult.Candidate
                {
                    Id = candidate.Id,
                    GovUkIdentifier = candidate.GovUkIdentifier,
                    Email = candidate.Email,
                    FirstName = candidate.FirstName,
                    LastName = candidate.LastName,
                    MiddleName = candidate.MiddleName,
                    PhoneNumber = candidate.PhoneNumber,
                    DateOfBirth = candidate.DateOfBirth,
                    Address = address
                }
            };
        }
    }
}
