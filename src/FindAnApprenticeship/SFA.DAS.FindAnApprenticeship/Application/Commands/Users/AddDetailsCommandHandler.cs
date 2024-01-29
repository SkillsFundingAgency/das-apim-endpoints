using Azure.Core;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.PatchApplication;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users
{
    public class AddDetailsCommandHandler : IRequestHandler<AddDetailsCommand, Unit>
    {
        private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

        public AddDetailsCommandHandler(
            ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        {
            _candidateApiClient = candidateApiClient;
        }

        public async Task<Unit> Handle (AddDetailsCommand command, CancellationToken cancellationToken)
        {
            var putData = new PutCandidateApiRequest.PutCandidateApiRequestData
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email
            };

            var putRequest = new PutCandidateApiRequest(command.GovUkIdentifier, putData);

            var response = await _candidateApiClient.PutWithResponseCode<NullResponse>(putRequest);

            if ((int)response.StatusCode > 300)
            {
                throw new InvalidOperationException();
            }

            return Unit.Value;

        }
        
    }
}
