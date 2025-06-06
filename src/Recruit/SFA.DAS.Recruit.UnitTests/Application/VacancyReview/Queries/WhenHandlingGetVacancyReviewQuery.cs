using System.Net;
using System.Threading;
using SFA.DAS.Recruit.Application.VacancyReview.Queries.GetVacancyReview;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Recruit.UnitTests.Application.VacancyReview.Queries;

public class WhenHandlingGetVacancyReviewQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Request_Is_Handled_And_Api_Called_And_VacancyReview_Returned(
        GetVacancyReviewQuery query,
        GetVacancyReviewResponse apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetVacancyReviewQueryHandler handler)
    {
        var expectedRequest = new GetVacancyReviewByIdRequest(query.Id);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<GetVacancyReviewResponse>(
                It.Is<GetVacancyReviewByIdRequest>(c => c.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<GetVacancyReviewResponse>(apiResponse, HttpStatusCode.OK, ""));
        
        var actual = await handler.Handle(query, CancellationToken.None);
        
        actual.VacancyReview.Should().BeEquivalentTo(apiResponse);
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Request_Is_Handled_And_Api_Called_And_Null_Returned_If_VacancyReview_NotFound(
        GetVacancyReviewQuery query,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        GetVacancyReviewQueryHandler handler)
    {
        var expectedRequest = new GetVacancyReviewByIdRequest(query.Id);
        recruitApiClient
            .Setup(x => x.GetWithResponseCode<GetVacancyReviewResponse>(
                It.Is<GetVacancyReviewByIdRequest>(c => c.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(new ApiResponse<GetVacancyReviewResponse>(null!, HttpStatusCode.NotFound, ""));
        
        var actual = await handler.Handle(query, CancellationToken.None);
        
        actual.VacancyReview.Should().BeNull();
    }
}