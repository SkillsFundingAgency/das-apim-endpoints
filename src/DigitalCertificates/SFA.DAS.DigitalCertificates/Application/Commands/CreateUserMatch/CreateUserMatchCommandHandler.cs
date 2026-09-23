using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using SFA.DAS.DigitalCertificates.Extensions;

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
            var identityResponse = await _digitalCertificatesApiClient
                .GetWithResponseCode<GetUserIdentityResponse>(new GetUsersByUserIdIdentityApiRequest(command.UserId));

            identityResponse.EnsureSuccessStatusCode();

            var identity = identityResponse.Body;

            var userMatchIdentity = command.UserIdentityId != null
                ? identity.Identity?.FirstOrDefault(p => p.UserIdentityId == command.UserIdentityId)
                : identity.Identity?.OrderByDescending(p => p.ValidSince).FirstOrDefault();

            if (userMatchIdentity == null || identity.DateOfBirth == null)
            {
                throw new InvalidOperationException("User identity details are required to submit a match attempt.");
            }

            var request = new PostUsersByUserIdMatchApiRequest(new CreateUserMatchRequest
            {
                Uln = command.Uln,
                FamilyName = userMatchIdentity.FamilyName,
                DateOfBirth = identity.DateOfBirth.Value,
                CertificateType = command.CertificateType.ToCertificateType(),
                CourseCode = command.CourseCode,
                CourseName = command.CourseName,
                CourseLevel = command.CourseLevel,
                YearAwarded = command.YearAwarded,
                ProviderName = command.ProviderName,
                Ukprn = command.Ukprn,
                IsMatched = command.IsMatched,
                IsFailed = command.IsFailed
            })
            {
                UserId = command.UserId
            };

            var response = await _digitalCertificatesApiClient.PostWithResponseCode<object>(request, false);

            response.EnsureSuccessStatusCode();

            return Unit.Value;
        }
    }
}
