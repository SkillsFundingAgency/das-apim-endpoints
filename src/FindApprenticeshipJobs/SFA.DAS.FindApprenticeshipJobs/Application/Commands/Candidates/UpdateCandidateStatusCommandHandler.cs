using System.Net;
using MediatR;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Commands.Candidates
{
    public class UpdateCandidateStatusCommandHandler(
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) 
        : IRequestHandler<UpdateCandidateStatusCommand>
    {
        public async Task Handle(UpdateCandidateStatusCommand request,
            CancellationToken cancellationToken)
        {
            var existingUser =
                await candidateApiClient.GetWithResponseCode<GetCandidateApiResponse>(
                    new GetCandidateApiRequest(request.GovUkIdentifier));

            if (existingUser.StatusCode != HttpStatusCode.NotFound)
            {
                if (existingUser.Body.Email == request.Email)
                {
                    var updateEmailRequest = new PutCandidateApiRequest(existingUser.Body.Id, new PutCandidateApiRequestData
                    {
                        Email = request.Email,
                        Status = request.Status
                    });

                    await candidateApiClient.PutWithResponseCode<PutCandidateApiResponse>(updateEmailRequest);
                }
            }
        }
    }
}