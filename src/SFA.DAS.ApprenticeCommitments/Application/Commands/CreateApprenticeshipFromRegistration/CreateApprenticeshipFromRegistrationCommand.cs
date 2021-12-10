using MediatR;
using SFA.DAS.ApprenticeCommitments.Apis.ApprenticeAccountsApi;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Application.Commands
{
    public class CreateApprenticeshipFromRegistration : IRequestHandler<CreateApprenticeshipFromRegistration.Command>
    {
        public class Command : IRequest
        {
            public Guid RegistrationId { get; set; }
            public Guid ApprenticeId { get; set; }
        }

        public CreateApprenticeshipFromRegistration(
            IInternalApiClient<ApprenticeAccountsConfiguration> accounts,
            IInternalApiClient<ApprenticeCommitmentsConfiguration> cmad)
        {
            _cmad = cmad;
            _accounts = accounts;
        }

        public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var apprentice = await _accounts.Get<Apis.ApprenticeAccountsApi.Apprentice>(new GetApprenticeRequest(request.ApprenticeId));

            var response = await _cmad.PostWithResponseCode<object>(new CreateApprenticeshipRequest(
                request.RegistrationId, request.ApprenticeId, apprentice.LastName, apprentice.DateOfBirth));

            if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 300)
            {
                throw new Exception("hi");
            }

            return Unit.Value;
        }

        private readonly IInternalApiClient<ApprenticeCommitmentsConfiguration> _cmad;
        private readonly IInternalApiClient<ApprenticeAccountsConfiguration> _accounts;
    }
}
