using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Assessor.Contracts.ApiRequests;
using SFA.DAS.Assessor.Contracts.ApiResponses;
using SFA.DAS.Assessor.Contracts.Client;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Assessor.Application.Commands.CreateOrUpdateUser
{
    public class CreateOrUpdateUserCommandHandler
        : IRequestHandler<CreateOrUpdateUserCommand, CreateOrUpdateUserResult>
    {
        private readonly IAssessorApiClient<AssessorApiConfiguration> _assessorApiClient;

        public CreateOrUpdateUserCommandHandler(
            IAssessorApiClient<AssessorApiConfiguration> assessorApiClient)
        {
            _assessorApiClient = assessorApiClient;
        }

        public async Task<CreateOrUpdateUserResult> Handle(
            CreateOrUpdateUserCommand command,
            CancellationToken cancellationToken)
        {
            var request = new PostUsersApiRequest(new CreateOrUpdateUserRequest
            {
                DfeSignInUserId = command.DfeSignInUserId,
                EmailAddress = command.EmailAddress,
                PhoneNumber = command.PhoneNumber
            });

            var response = await _assessorApiClient
                .PostWithResponseCode<CreateOrUpdateUserResponse>(request);

            response.EnsureSuccessStatusCode();

            if (response.Body == null)
            {
                throw new InvalidOperationException(
                    "The Assessor API returned a successful response without a user response body.");
            }

            return new CreateOrUpdateUserResult
            {
                UserId = response.Body.UserId
            };
        }
    }
}