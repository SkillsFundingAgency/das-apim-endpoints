using System;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Command.PatchApplicationReview
{
    public class PatchApplicationReviewCommandHandler(
        IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) : IRequestHandler<PatchApplicationReviewCommand>
    {
        public async Task Handle(PatchApplicationReviewCommand request, CancellationToken cancellationToken)
        {
            var jsonPatchApplicationReviewDocument = new JsonPatchDocument<InnerApi.Requests.ApplicationReview>();
            jsonPatchApplicationReviewDocument.Replace(x => x.Status, request.Status);
            jsonPatchApplicationReviewDocument.Replace(x => x.HasEverBeenEmployerInterviewing, request.HasEverBeenEmployerInterviewing);            
            jsonPatchApplicationReviewDocument.Replace(x => x.EmployerFeedback, request.EmployerFeedback);
            jsonPatchApplicationReviewDocument.Replace(x => x.StatusUpdatedDate, DateTime.UtcNow);
            if (request.TemporaryReviewStatus != null)
            {
                jsonPatchApplicationReviewDocument.Replace(x => x.TemporaryReviewStatus, request.TemporaryReviewStatus);
            }  
            if(request.DateSharedWithEmployer.HasValue)
            {
                jsonPatchApplicationReviewDocument.Replace(x => x.DateSharedWithEmployer, request.DateSharedWithEmployer.Value);
            }


            var patchApplicationReviewApiRequest = new PatchRecruitApplicationReviewApiRequest(request.Id, jsonPatchApplicationReviewDocument);
            var patchResponse = await recruitApiClient.PatchWithResponseCode(patchApplicationReviewApiRequest);

            patchResponse.EnsureSuccessStatusCode();
        }
    }
}
