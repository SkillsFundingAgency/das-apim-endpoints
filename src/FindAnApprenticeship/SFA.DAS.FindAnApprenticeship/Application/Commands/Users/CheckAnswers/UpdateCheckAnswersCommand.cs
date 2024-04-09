using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.CheckAnswers
{
    public class UpdateCheckAnswersCommand : IRequest
    {
        public Guid CandidateId { get; set; }
    }

    public class UpdateCheckAnswersCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient) : IRequestHandler<UpdateCheckAnswersCommand>
    {
        public async Task Handle(UpdateCheckAnswersCommand request, CancellationToken cancellationToken)
        {
            var postRequest = new PutCandidateApiRequest(request.CandidateId, new PutCandidateApiRequestData
            {
                Status = 1
            });

            var response = await candidateApiClient.PutWithResponseCode<PutCandidateApiResponse>(postRequest);

            if ((int)response.StatusCode > 300)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
