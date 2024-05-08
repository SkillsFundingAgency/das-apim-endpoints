using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Requests;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.WithdrawApplication;

public class WithdrawApplicationCommandHandler(
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient, 
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) : IRequestHandler<WithdrawApplicationCommand, bool>
{
    public async Task<bool> Handle(WithdrawApplicationCommand request, CancellationToken cancellationToken)
    {
        var application =
            await candidateApiClient.Get<GetApplicationApiResponse>(
                new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false));

        if (application == null || application.Status == "Withdrawn")
        {
            return false;
        }
        var response = await recruitApiClient.PostWithResponseCode<NullResponse>(
            new PostWithdrawApplicationRequest(request.CandidateId, Convert.ToInt64(application.VacancyReference.Replace("VAC",""))), false);

        if (response.StatusCode != HttpStatusCode.NoContent)
        {
            return false;
        }
        
        var jsonPatchDocument = new JsonPatchDocument<Models.Application>();
        
        jsonPatchDocument.Replace(x => x.Status, ApplicationStatus.Withdrawn);
        
        var patchRequest = new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, jsonPatchDocument);
        await candidateApiClient.PatchWithResponseCode(patchRequest);
        
        return true;
    }
}