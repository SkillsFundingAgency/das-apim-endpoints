using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReview;
using System.Net;
using SFA.DAS.Recruit.Contracts.ApiRequests;
using IRecruitApiClient = SFA.DAS.Recruit.Contracts.Client.IRecruitApiClient<SFA.DAS.Recruit.Contracts.Client.RecruitApiConfiguration>;

namespace SFA.DAS.RecruitQa.UnitTests.Application.Queries.VacancyReview;

public class WhenHandlingGetVacancyReviewQuery
{
    [Test, RecursiveMoqAutoData]
    public async Task Then_The_Request_Is_Handled_And_Api_Called_And_VacancyReview_Returned(
        GetVacancyReviewQuery query,
        Recruit.Contracts.ApiResponses.VacancyReview apiResponse,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        GetVacancyReviewQueryHandler handler)
    {
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<Recruit.Contracts.ApiResponses.VacancyReview>(
                It.IsAny<GetVacancyreviewsByIdApiRequest>()))
            .ReturnsAsync(new ApiResponse<Recruit.Contracts.ApiResponses.VacancyReview>(apiResponse, HttpStatusCode.OK, ""));
        
        var actual = await handler.Handle(query, CancellationToken.None);
        
        actual.VacancyReview.Should().BeEquivalentTo(apiResponse);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Request_Is_Handled_And_Api_Called_And_Null_Returned_If_VacancyReview_NotFound(
        GetVacancyReviewQuery query,
        [Frozen] Mock<IRecruitApiClient> recruitApiClient,
        GetVacancyReviewQueryHandler handler)
    {
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<Recruit.Contracts.ApiResponses.VacancyReview>(
                It.IsAny<GetVacancyreviewsByIdApiRequest>()))
            .ReturnsAsync(new ApiResponse<Recruit.Contracts.ApiResponses.VacancyReview>(null!, HttpStatusCode.NotFound, ""));
        
        var actual = await handler.Handle(query, CancellationToken.None);
        
        actual.VacancyReview.Should().BeNull();
    }
}