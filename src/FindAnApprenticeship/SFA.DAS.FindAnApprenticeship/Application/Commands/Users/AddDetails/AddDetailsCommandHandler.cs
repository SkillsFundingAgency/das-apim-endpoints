using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.AddDetails
{
    public class AddDetailsCommandHandler : IRequestHandler<AddDetailsCommand, Unit>
    {
        private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

        public AddDetailsCommandHandler(
            ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        {
            _candidateApiClient = candidateApiClient;
        }

        public async Task<Unit> Handle(AddDetailsCommand command, CancellationToken cancellationToken)
        {
            var putData = new PutCandidateApiRequestData
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email
            };

            var putRequest = new PutCandidateApiRequest(command.CandidateId, putData);

            var response = await _candidateApiClient.PutWithResponseCode<NullResponse>(putRequest);

            if ((int)response.StatusCode > 300)
            {
                throw new InvalidOperationException();
            }

            return Unit.Value;

        }

    }
}
