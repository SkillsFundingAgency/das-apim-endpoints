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

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.MigrateData
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
                FirstName = userDetails?.RegistrationDetails?.FirstName,
                LastName = userDetails?.RegistrationDetails?.LastName,
                DateOfBirth = registrationDetailsDateOfBirth,
                PhoneNumber = userDetails?.RegistrationDetails?.PhoneNumber,
                MigratedEmail = userDetails?.RegistrationDetails?.EmailAddress
            });

            var candidateResponse = await CandidateApiClient.PutWithResponseCode<PutCandidateApiResponse>(putRequest);

            candidateResponse.EnsureSuccessStatusCode();

            if (userDetails is {RegistrationDetails.Address: not null})
            {
                var postData = new PutCandidateAddressApiRequestData
                {
                    Email = candidateResponse.Body.Email,
                    AddressLine1 = userDetails.RegistrationDetails?.Address?.AddressLine1,
                    AddressLine2 = userDetails.RegistrationDetails?.Address?.AddressLine2,
                    AddressLine3 = userDetails.RegistrationDetails?.Address?.Town,
                    AddressLine4 = userDetails.RegistrationDetails?.Address?.County,
                    Latitude = userDetails.RegistrationDetails?.Address?.GeoPoint?.Latitude ?? default,
                    Longitude = userDetails.RegistrationDetails?.Address?.GeoPoint?.Longitude ?? default,
                    Postcode = userDetails.RegistrationDetails?.Address?.Postcode,
                    Uprn = userDetails.RegistrationDetails?.Address?.Uprn
                };

                var postRequest = new PutCandidateAddressApiRequest(command.CandidateId, postData);

                var candidateAddressResponse = await CandidateApiClient.PutWithResponseCode<PostCandidateAddressApiResponse>(postRequest);

                candidateAddressResponse.EnsureSuccessStatusCode();
            }

            await LegacyApplicationMigrationService.MigrateLegacyApplications(command.CandidateId, command.EmailAddress);

            return Unit.Value;
        }
    }
}