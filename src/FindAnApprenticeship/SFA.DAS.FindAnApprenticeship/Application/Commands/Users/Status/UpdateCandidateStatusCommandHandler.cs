using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.Status
{
    public class UpdateCandidateStatusCommandHandler(ICandidateApiClient<CandidateApiConfiguration> CandidateApiClient) : IRequestHandler<UpdateCandidateStatusCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateCandidateStatusCommand request,
            CancellationToken cancellationToken)
        {
            var existingUser =
                await CandidateApiClient.GetWithResponseCode<GetCandidateApiResponse>(
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

                    await CandidateApiClient.PutWithResponseCode<PutCandidateApiResponse>(updateEmailRequest);
                }
            }

            return Unit.Value;
        }
    }
}