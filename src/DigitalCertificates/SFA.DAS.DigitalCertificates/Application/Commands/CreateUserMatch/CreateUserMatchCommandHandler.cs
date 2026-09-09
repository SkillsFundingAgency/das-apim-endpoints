using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.DigitalCertificates.Application.Commands.CreateUserMatch
{
    public class CreateUserMatchCommandHandler : IRequestHandler<CreateUserMatchCommand, Unit>
    {
        private readonly IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> _digitalCertificatesApiClient;

        public CreateUserMatchCommandHandler(IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration> digitalCertificatesApiClient)
        {
            _digitalCertificatesApiClient = digitalCertificatesApiClient;
        }

        public async Task<Unit> Handle(CreateUserMatchCommand command, CancellationToken cancellationToken)
        {
            var request = new PostCreateUserMatchRequest((PostCreateUserMatchRequestData)command, command.UserId);

            var identityResponse = await _digitalCertificatesApiClient
                .GetWithResponseCode<GetUserIdentityResponse>(new GetUserIdentityRequest(command.UserId));

            identityResponse.EnsureSuccessStatusCode();
            var identity = identityResponse.Body;

            var userMatchIdentity = command.UserIdentityId != null
                ? identity.Identity.FirstOrDefault(p => p.UserIdentityId == command.UserIdentityId)
                : identity.Identity.OrderByDescending(p => p.ValidSince).FirstOrDefault();

            if (userMatchIdentity == null || identity.DateOfBirth == null)
            {
                throw new InvalidOperationException("User identity details are required to submit a match attempt.");
            }

            request.Data.FamilyName = userMatchIdentity.FamilyName;
            request.Data.DateOfBirth = identity.DateOfBirth.Value;

            var response = await _digitalCertificatesApiClient.PostWithResponseCode<PostCreateUserMatchRequestData, object>(request, false);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
