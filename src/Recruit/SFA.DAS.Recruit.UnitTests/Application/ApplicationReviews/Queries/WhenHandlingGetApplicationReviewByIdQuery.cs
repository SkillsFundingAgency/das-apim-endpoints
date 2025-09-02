using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewById;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.ApplicationReviews.Queries;
[TestFixture]
public class WhenHandlingGetApplicationReviewByIdQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        Guid applicationId,
        Guid candidateId,
            GetApplicationReviewByIdQuery query,
            Recruit.Domain.ApplicationReview apiResponse,
            Recruit.Domain.Application candidateApiResponse,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetApplicationReviewByIdQueryHandler handler)
    {
        //Arrange
        apiResponse.ApplicationId = applicationId;
        apiResponse.CandidateId = candidateId;
        var expectedGetUrl = new GetApplicationReviewByIdApiRequest(query.ApplicationReviewId);
        recruitApiClient
            .Setup(x => x.Get<Recruit.Domain.ApplicationReview>(
                It.Is<GetApplicationReviewByIdApiRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
            .ReturnsAsync(apiResponse);

        var expectedCandidateGetUrl = new GetApplicationByIdApiRequest(applicationId, candidateId);
        candidateApiClient
            .Setup(x => x.Get<Recruit.Domain.Application>(
                It.Is<GetApplicationByIdApiRequest>(r =>
                    r.GetUrl == expectedCandidateGetUrl.GetUrl)))
            .ReturnsAsync(candidateApiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.ApplicationReview.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        recruitApiClient.Verify(x => x.Get<Recruit.Domain.ApplicationReview>(It.IsAny<GetApplicationReviewByIdApiRequest>()), Times.Once);
        candidateApiClient.Verify(x => x.Get<Recruit.Domain.Application>(It.IsAny<GetApplicationByIdApiRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_The_ApplicationId_Is_Null_Query_Is_Handled_And_Returned(
        Guid applicationId,
        Guid candidateId,
        GetApplicationReviewByIdQuery query,
        Recruit.Domain.ApplicationReview apiResponse,
        Recruit.Domain.Application candidateApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetApplicationReviewByIdQueryHandler handler)
    {
        //Arrange
        apiResponse.ApplicationId = null;
        apiResponse.CandidateId = candidateId;
        var expectedGetUrl = new GetApplicationReviewByIdApiRequest(query.ApplicationReviewId);
        recruitApiClient
            .Setup(x => x.Get<Recruit.Domain.ApplicationReview>(
                It.Is<GetApplicationReviewByIdApiRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
            .ReturnsAsync(apiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.ApplicationReview.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        recruitApiClient.Verify(x => x.Get<Recruit.Domain.ApplicationReview>(It.IsAny<GetApplicationReviewByIdApiRequest>()), Times.Once);
        candidateApiClient.Verify(x => x.Get<Recruit.Domain.Application>(It.IsAny<GetApplicationByIdApiRequest>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Null_Returned(
        Guid applicationId,
        Guid candidateId,
        GetApplicationReviewByIdQuery query,
        Recruit.Domain.ApplicationReview apiResponse,
        Recruit.Domain.Application candidateApiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetApplicationReviewByIdQueryHandler handler)
    {
        //Arrange
        apiResponse.ApplicationId = applicationId;
        apiResponse.CandidateId = candidateId;
        var expectedGetUrl = new GetApplicationReviewByIdApiRequest(query.ApplicationReviewId);
        recruitApiClient
            .Setup(x => x.Get<Recruit.Domain.ApplicationReview>(
                It.Is<GetApplicationReviewByIdApiRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
            .ReturnsAsync((Recruit.Domain.ApplicationReview)null!);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Should().BeEquivalentTo(new GetApplicationReviewByIdQueryResult());
        recruitApiClient.Verify(x => x.Get<Recruit.Domain.ApplicationReview>(It.IsAny<GetApplicationReviewByIdApiRequest>()), Times.Once);
        candidateApiClient.Verify(x => x.Get<Recruit.Domain.Application>(It.IsAny<GetApplicationByIdApiRequest>()), Times.Never);
    }
}
