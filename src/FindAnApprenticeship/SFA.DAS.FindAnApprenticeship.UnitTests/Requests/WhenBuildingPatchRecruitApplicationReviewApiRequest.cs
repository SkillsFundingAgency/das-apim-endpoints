using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitV2Api.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Requests;

public class WhenBuildingPatchRecruitApplicationReviewApiRequest
{
    [Test, AutoData]
    public void Then_Request_And_Data_Is_Built(ApplicationReview applicationReview, Guid applicationId)
    {
        var jsonPatchApplicationReviewDocument = new JsonPatchDocument<ApplicationReview>();
        jsonPatchApplicationReviewDocument.Replace(x => x.WithdrawnDate, applicationReview.WithdrawnDate);
        jsonPatchApplicationReviewDocument.Replace(x => x.Status, applicationReview.Status);
        jsonPatchApplicationReviewDocument.Replace(x => x.StatusUpdatedDate, applicationReview.StatusUpdatedDate);
        
        var actual = new PatchRecruitApplicationReviewApiRequest(applicationId, jsonPatchApplicationReviewDocument);
        
        actual.PatchUrl.Should().Be($"api/applicationReviews/{applicationId}");
    }
}