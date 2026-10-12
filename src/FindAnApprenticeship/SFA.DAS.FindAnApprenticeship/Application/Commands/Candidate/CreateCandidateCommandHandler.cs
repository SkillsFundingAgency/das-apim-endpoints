using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.JsonPatch.SystemTextJson;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;

public class CreateCandidateCommandHandler(
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    : IRequestHandler<CreateCandidateCommand, CreateCandidateCommandResult>
{
    public async Task<CreateCandidateCommandResult> Handle(CreateCandidateCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await LookupCandidateAsync(request.GovUkIdentifier, request.Email);

        if (existingUser.StatusCode != HttpStatusCode.NotFound)
        {
            if (existingUser.Body.Email != request.Email || existingUser.Body.Status == UserStatus.Dormant)
            {
                var updateEmailRequest = new PutCandidateApiRequest(existingUser.Body.Id, new PutCandidateApiRequestData
                {
                    Email = request.Email,
                    Status = existingUser.Body.Status == UserStatus.Dormant
                        ? UserStatus.Completed
                        : existingUser.Body.Status
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

        var postData = new PostCandidateApiRequestData
        {
            Email = request.Email
        };

        var postRequest = new PostCandidateApiRequest(request.GovUkIdentifier, postData);

        var candidateResult = await candidateApiClient.PostWithResponseCode<PostCandidateApiResponse>(postRequest);

        candidateResult.EnsureSuccessStatusCode();

        if (candidateResult is null)
        {
            return null;
        }
        
        return new CreateCandidateCommandResult
        {
            Id = candidateResult.Body.Id,
            GovUkIdentifier = candidateResult.Body.GovUkIdentifier,
            Email = candidateResult.Body.Email,
            FirstName = candidateResult.Body.FirstName,
            LastName = candidateResult.Body.LastName,
            PhoneNumber = candidateResult.Body.PhoneNumber,
            Status = UserStatus.Incomplete
        };
    }

    private async Task<ApiResponse<GetCandidateApiResponse>> LookupCandidateAsync(string govUkIdentifier, string emailAddress)
    {
        var existingUser = await candidateApiClient.GetWithResponseCode<GetCandidateApiResponse>(new GetCandidateApiRequest(govUkIdentifier));
        if (existingUser.StatusCode == HttpStatusCode.NotFound)
        {
            existingUser = await LookupCandidateByEmailAddressAsync(emailAddress, govUkIdentifier);
        }
        
        return existingUser;
    }

    private async Task<ApiResponse<GetCandidateApiResponse>> LookupCandidateByEmailAddressAsync(string emailAddress, string govUkIdentifier)
    {
        var existingUser = await candidateApiClient.GetWithResponseCode<GetCandidateApiResponse>(new GetCandidateByEmailAddressApiRequest(emailAddress));
        switch (existingUser.StatusCode)
        {
            case HttpStatusCode.NotFound:
                break;
            case HttpStatusCode.OK:
                if (existingUser.Body.GovUkIdentifier is not null && existingUser.Body.GovUkIdentifier != govUkIdentifier)
                {
                    throw new NotSupportedException("GovUkIdentifier is already set. If it requires updating, support need to set the existing value to null.");
                }
                    
                if (existingUser.Body.GovUkIdentifier is null)
                {
                    // patch the user
                    var patchDocument = new JsonPatchDocument<PatchableCandidate>();
                    patchDocument.Replace(x => x.GovUkIdentifier, govUkIdentifier);
                    patchDocument.Replace(x => x.UpdatedOn, DateTime.UtcNow);
                    var patchUserRequest = new PatchCandidateApiRequest(existingUser.Body.Id, patchDocument);
                    var patchResponse = await candidateApiClient.PatchWithResponseCode(patchUserRequest);
                    patchResponse.EnsureSuccessStatusCode();
                    existingUser.Body.GovUkIdentifier = govUkIdentifier;
                }
                break;
            default:
                throw new NotSupportedException("There was a problem retrieving the candidate by their email address. This could possibly be due to there being multiple accounts with the same email address.");
        }

        return existingUser;
    }
}