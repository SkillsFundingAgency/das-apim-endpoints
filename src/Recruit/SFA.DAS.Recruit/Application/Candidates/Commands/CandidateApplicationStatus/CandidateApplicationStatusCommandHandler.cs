using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Candidates.Commands.CandidateApplicationStatus;

public class CandidateApplicationStatusCommandHandler : IRequestHandler<CandidateApplicationStatusCommand, Unit>
{
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public CandidateApplicationStatusCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _candidateApiClient = candidateApiClient;
    }
    public async Task<Unit> Handle(CandidateApplicationStatusCommand request, CancellationToken cancellationToken)
    {
        if (request.ApplicationId == Guid.Empty)
        {
            var migratedCandidateRequest = new GetCandidateByMigratedCandidateIdApiRequest(request.CandidateId);
            var candidate =
                await _candidateApiClient.GetWithResponseCode<GetCandidateApiResponse>(migratedCandidateRequest);
            if (candidate.StatusCode == HttpStatusCode.NotFound)
            {
                return new Unit();
            }
            var applicationRequest =
                new GetApplicationByReferenceApiRequest(candidate.Body.Id, request.VacancyReference);
            var application = await
                _candidateApiClient.GetWithResponseCode<GetApplicationByReferenceApiResponse>(applicationRequest);
            request.ApplicationId = application.Body.Id;
        }
        var jsonPatchDocument = new JsonPatchDocument<InnerApi.Requests.Application>();

        var applicationStatus = request.Outcome.Equals("Successful", StringComparison.CurrentCultureIgnoreCase)
            ? ApplicationStatus.Successful
            : ApplicationStatus.UnSuccessful;
        
        jsonPatchDocument.Replace(x => x.ResponseNotes, request.Feedback);
        jsonPatchDocument.Replace(x => x.Status, applicationStatus);
        
        var patchRequest = new PatchApplicationApiRequest(request.ApplicationId, request.CandidateId, jsonPatchDocument);
        
        await _candidateApiClient.PatchWithResponseCode(patchRequest);

        return new Unit();
    }
}