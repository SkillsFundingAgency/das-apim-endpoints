using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;

public class CreateCandidateCommandHandler(
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
    IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration> legacyApiClient)
    : IRequestHandler<CreateCandidateCommand, CreateCandidateCommandResult>
{
    public async Task<CreateCandidateCommandResult> Handle(CreateCandidateCommand request, CancellationToken cancellationToken)
    {
        var existingUser =
            await candidateApiClient.GetWithResponseCode<GetCandidateApiResponse>(
                new GetCandidateApiRequest(Guid.Parse(request.GovUkIdentifier)));

        if (existingUser.StatusCode != HttpStatusCode.NotFound)
        {
            return new CreateCandidateCommandResult
            {
                Id = existingUser.Body.Id,
                GovUkIdentifier = existingUser.Body.GovUkIdentifier,
                Email = existingUser.Body.Email,
                FirstName = existingUser.Body.FirstName,
                LastName = existingUser.Body.LastName,
                PhoneNumber = existingUser.Body.PhoneNumber,
                DateOfBirth = existingUser.Body.DateOfBirth,
                Status = existingUser.Body.Status
            };
        }

        var userDetails =
            await legacyApiClient.Get<GetLegacyUserByEmailApiResponse>(
                new GetLegacyUserByEmailApiRequest(request.Email));

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
        };

        var postRequest = new PostCandidateApiRequest(request.GovUkIdentifier, postData);

        var candidateResult = await candidateApiClient.PostWithResponseCode<PostCandidateApiResponse>(postRequest);

        candidateResult.EnsureSuccessStatusCode();

        if (candidateResult is null) return null;

        return new CreateCandidateCommandResult
        {
            Id = candidateResult.Body.Id,
            GovUkIdentifier = candidateResult.Body.GovUkIdentifier,
            Email = candidateResult.Body.Email,
            FirstName = candidateResult.Body.FirstName,
            LastName = candidateResult.Body.LastName,
            PhoneNumber = candidateResult.Body.PhoneNumber,
            DateOfBirth = registrationDetailsDateOfBirth,
            Status = UserStatus.Incomplete
        };
    }
}