using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;

public class CreateCandidateCommandHandler(
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
    IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration> legacyApiClient,
    ILegacyApplicationMigrationService legacyApplicationMigrationService)
    : IRequestHandler<CreateCandidateCommand, CreateCandidateCommandResult>
{
    public async Task<CreateCandidateCommandResult> Handle(CreateCandidateCommand request, CancellationToken cancellationToken)
    {
        var existingUser =
            await candidateApiClient.GetWithResponseCode<GetCandidateApiResponse>(
                new GetCandidateApiRequest(request.GovUkIdentifier));

        if (existingUser.StatusCode != HttpStatusCode.NotFound)
        {
            if (existingUser.Body.Email != request.Email)
            {
                var updateEmailRequest = new PutCandidateApiRequest(existingUser.Body.Id, new PutCandidateApiRequestData
                {
                    Email = request.Email
                });

                await candidateApiClient.PutWithResponseCode<PutCandidateApiResponse>(updateEmailRequest);    
            }
            
            return new CreateCandidateCommandResult
            {
                Id = existingUser.Body.Id,
                GovUkIdentifier = existingUser.Body.GovUkIdentifier,
                Email = request.Email,
                FirstName = existingUser.Body.FirstName,
                LastName = existingUser.Body.LastName,
                PhoneNumber = existingUser.Body.PhoneNumber,
                DateOfBirth = existingUser.Body.DateOfBirth,
                Status = existingUser.Body.Status
            };
        }

        if (existingUser.StatusCode == HttpStatusCode.NotFound)
        {
            existingUser =
                await candidateApiClient.GetWithResponseCode<GetCandidateApiResponse>(
                    new GetCandidateByEmailApiRequest(request.Email));
            if (existingUser.StatusCode != HttpStatusCode.NotFound)
            {
                if (existingUser.Body.GovUkIdentifier != null)
                {
                    return new CreateCandidateCommandResult
                    {
                        IsEmailAddressMigrated = true
                    };
                }
                
                var updateEmailRequest = new PutCandidateApiRequest(existingUser.Body.Id, new PutCandidateApiRequestData
                {
                    GovUkIdentifier = request.GovUkIdentifier
                });

                await candidateApiClient.PutWithResponseCode<PutCandidateApiResponse>(updateEmailRequest);  
                
                return new CreateCandidateCommandResult
                {
                    Id = existingUser.Body.Id,
                    GovUkIdentifier = request.GovUkIdentifier,
                    Email = request.Email,
                    FirstName = existingUser.Body.FirstName,
                    LastName = existingUser.Body.LastName,
                    PhoneNumber = existingUser.Body.PhoneNumber,
                    DateOfBirth = existingUser.Body.DateOfBirth,
                    Status = existingUser.Body.Status
                };
            }
        }

        var userWithMigratedEmail =
            await candidateApiClient.GetWithResponseCode<GetCandidateByMigratedEmailApiResponse>(
                new GetCandidateByMigratedEmailApiRequest(request.Email));

        if (userWithMigratedEmail.StatusCode != HttpStatusCode.NotFound)
        {
            return new CreateCandidateCommandResult
            {
                IsEmailAddressMigrated = true
            };
        }

        var userMigrationStatus = UserStatus.Incomplete;

        var userDetails =
            await legacyApiClient.Get<GetLegacyUserByEmailApiResponse>(
                new GetLegacyUserByEmailApiRequest(request.Email));

        if (userDetails != null && Guid.TryParse(userDetails.Id.ToString(), out _))
        {
            userMigrationStatus = UserStatus.InProgress;
        }
        var registrationDetailsDateOfBirth = userDetails?.RegistrationDetails?.DateOfBirth;
        if (registrationDetailsDateOfBirth == DateTime.MinValue)
        {
            registrationDetailsDateOfBirth = null;
        }

        var postData = new PostCandidateApiRequestData
        {
            Email = request.Email,
            FirstName = userDetails?.RegistrationDetails?.FirstName,
            LastName = userDetails?.RegistrationDetails?.LastName,
            DateOfBirth = registrationDetailsDateOfBirth,
            MigratedEmail = userDetails == null ? null : request.Email,
            MigratedCandidateId = userDetails?.Id,
            PhoneNumber = userDetails?.RegistrationDetails?.PhoneNumber,
        };

        var postRequest = new PostCandidateApiRequest(request.GovUkIdentifier, postData);

        var candidateResult = await candidateApiClient.PostWithResponseCode<PostCandidateApiResponse>(postRequest);

        candidateResult.EnsureSuccessStatusCode();

        if (candidateResult is null)
        {
            return null;
        }
        
        if (userDetails is {RegistrationDetails.Address: not null})
        {
            var postAddressData = new PutCandidateAddressApiRequestData
            {
                Email = request.Email,
                AddressLine1 = userDetails.RegistrationDetails?.Address?.AddressLine1,
                AddressLine2 = userDetails.RegistrationDetails?.Address?.AddressLine2,
                AddressLine3 = userDetails.RegistrationDetails?.Address?.AddressLine3,
                AddressLine4 = userDetails.RegistrationDetails?.Address?.AddressLine4,
                Latitude = userDetails.RegistrationDetails?.Address?.GeoPoint?.Latitude ?? default,
                Longitude = userDetails.RegistrationDetails?.Address?.GeoPoint?.Longitude ?? default,
                Postcode = userDetails.RegistrationDetails?.Address?.Postcode,
                Uprn = userDetails.RegistrationDetails?.Address?.Uprn
            };

            var postAddressRequest = new PutCandidateAddressApiRequest(candidateResult.Body.Id, postAddressData);

            var candidateAddressResponse = await candidateApiClient.PutWithResponseCode<PostCandidateAddressApiResponse>(postAddressRequest);

            candidateAddressResponse.EnsureSuccessStatusCode();
        }
        

        await legacyApplicationMigrationService.MigrateLegacyApplications(candidateResult.Body.Id, request.Email);

        return new CreateCandidateCommandResult
        {
            Id = candidateResult.Body.Id,
            GovUkIdentifier = candidateResult.Body.GovUkIdentifier,
            Email = candidateResult.Body.Email,
            FirstName = candidateResult.Body.FirstName,
            LastName = candidateResult.Body.LastName,
            PhoneNumber = candidateResult.Body.PhoneNumber,
            DateOfBirth = registrationDetailsDateOfBirth,
            Status = userMigrationStatus
        };
    }
}