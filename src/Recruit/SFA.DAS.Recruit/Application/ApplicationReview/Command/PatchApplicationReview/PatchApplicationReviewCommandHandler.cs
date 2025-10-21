using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Command.PatchApplicationReview
{
    public class PatchApplicationReviewCommandHandler(
        IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) : IRequestHandler<PatchApplicationReviewCommand>
    {
        public async Task Handle(PatchApplicationReviewCommand command, CancellationToken cancellationToken)
        {
            var jsonPatchApplicationReviewDocument = new JsonPatchDocument<InnerApi.Recruit.Requests.ApplicationReview>();
            jsonPatchApplicationReviewDocument.Replace(x => x.HasEverBeenEmployerInterviewing, command.HasEverBeenEmployerInterviewing);
            jsonPatchApplicationReviewDocument.Replace(x => x.StatusUpdatedDate, DateTime.UtcNow);
            jsonPatchApplicationReviewDocument.Replace(x => x.TemporaryReviewStatus, command.TemporaryReviewStatus);

            if (!string.IsNullOrEmpty(command.Status))
            {
                jsonPatchApplicationReviewDocument.Replace(x => x.Status, command.Status);
            }
            if (!string.IsNullOrEmpty(command.EmployerFeedback))
            {
                jsonPatchApplicationReviewDocument.Replace(x => x.EmployerFeedback, command.EmployerFeedback);
            }
            if(command.DateSharedWithEmployer.HasValue)
            {
                jsonPatchApplicationReviewDocument.Replace(x => x.DateSharedWithEmployer, command.DateSharedWithEmployer.Value);
            }

            var patchApplicationReviewApiRequest = new PatchRecruitApplicationReviewApiRequest(command.Id, jsonPatchApplicationReviewDocument);
            var patchResponse = await recruitApiClient.PatchWithResponseCode(patchApplicationReviewApiRequest);
            
            if (patchResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            patchResponse.EnsureSuccessStatusCode();
        }
    }
}
