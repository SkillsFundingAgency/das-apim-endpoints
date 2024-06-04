using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.MigrateData
{
    public record MigrateDataCommandHandler(
        ICandidateApiClient<CandidateApiConfiguration> CandidateApiClient,
        IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration> LegacyApiClient,
        ILegacyApplicationMigrationService LegacyApplicationMigrationService)
        : IRequestHandler<MigrateDataCommand, Unit>
    {
        public async Task<Unit> Handle(MigrateDataCommand command, CancellationToken cancellationToken)
        {
            var userDetails =
                await LegacyApiClient.Get<GetLegacyUserByEmailApiResponse>(
                    new GetLegacyUserByEmailApiRequest(command.EmailAddress));

            var registrationDetailsDateOfBirth = userDetails?.RegistrationDetails?.DateOfBirth;
            if (registrationDetailsDateOfBirth == DateTime.MinValue)
            {
                registrationDetailsDateOfBirth = null;
            }

            var putRequest = new PutCandidateApiRequest(command.CandidateId, new PutCandidateApiRequestData
            {
                Email = command.EmailAddress,
                FirstName = userDetails?.RegistrationDetails?.FirstName,
                LastName = userDetails?.RegistrationDetails?.LastName,
                DateOfBirth = registrationDetailsDateOfBirth,
            });

            var candidateResult = await CandidateApiClient.PutWithResponseCode<PutCandidateApiResponse>(putRequest);

            candidateResult.EnsureSuccessStatusCode();

            await LegacyApplicationMigrationService.MigrateLegacyApplications(command.CandidateId, command.EmailAddress);

            return Unit.Value;
        }
    }
}