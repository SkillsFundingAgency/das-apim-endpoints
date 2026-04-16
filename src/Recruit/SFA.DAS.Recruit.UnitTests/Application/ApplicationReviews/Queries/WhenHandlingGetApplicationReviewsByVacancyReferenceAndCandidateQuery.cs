using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewById;
using SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewByVacancyReferenceAndCandidateId;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.ApplicationReviews.Queries;
[TestFixture]
public class WhenHandlingGetApplicationReviewsByVacancyReferenceAndCandidateQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        Guid applicationId,
        Guid candidateId,
        GetApplicationReviewByVacancyReferenceAndCandidateIdQuery query,
            Recruit.Domain.ApplicationReview apiResponse,
            GetApplicationsByVacancyReferenceApiResponse candidateApiResponse,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetApplicationReviewByVacancyReferenceAndCandidateIdQueryHandler handler)
    {
        //Arrange
        apiResponse.ApplicationId = applicationId;
        apiResponse.CandidateId = candidateId;
        var expectedGetUrl = new GetApplicationReviewByVacancyReferenceAndCandidateIdApiRequest(query.VacancyReference, query.CandidateId);
        recruitApiClient
            .Setup(x => x.Get<Recruit.Domain.ApplicationReview>(
                It.Is<GetApplicationReviewByVacancyReferenceAndCandidateIdApiRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
            .ReturnsAsync(apiResponse);

        var expectedCandidateGetUrl = new GetApplicationsByVacancyReferenceApiRequest(query.VacancyReference);
        candidateApiClient
            .Setup(x => x.Get<GetApplicationsByVacancyReferenceApiResponse>(
                It.Is<GetApplicationsByVacancyReferenceApiRequest>(r =>
                    r.GetUrl == expectedCandidateGetUrl.GetUrl)))
            .ReturnsAsync(candidateApiResponse);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.ApplicationReview.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        recruitApiClient.Verify(x => x.Get<Recruit.Domain.ApplicationReview>(It.IsAny<GetApplicationReviewByVacancyReferenceAndCandidateIdApiRequest>()), Times.Once);
        candidateApiClient.Verify(x => x.Get<GetApplicationsByVacancyReferenceApiResponse>(It.IsAny<GetApplicationsByVacancyReferenceApiRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Null_Returned(
        Guid applicationId,
        Guid candidateId,
        GetApplicationReviewByVacancyReferenceAndCandidateIdQuery query,
        Recruit.Domain.ApplicationReview apiResponse,
        [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetApplicationReviewByVacancyReferenceAndCandidateIdQueryHandler handler)
    {
        //Arrange
        apiResponse.ApplicationId = applicationId;
        apiResponse.CandidateId = candidateId;
        var expectedGetUrl = new GetApplicationReviewByVacancyReferenceAndCandidateIdApiRequest(query.VacancyReference, query.CandidateId);
        recruitApiClient
            .Setup(x => x.Get<Recruit.Domain.ApplicationReview>(
                It.Is<GetApplicationReviewByVacancyReferenceAndCandidateIdApiRequest>(r => r.GetUrl == expectedGetUrl.GetUrl)))
            .ReturnsAsync((Recruit.Domain.ApplicationReview)null!);

        //Act
        var actual = await handler.Handle(query, CancellationToken.None);

        //Assert
        actual.Should().BeEquivalentTo(new GetApplicationReviewByIdQueryResult());
        recruitApiClient.Verify(x => x.Get<Recruit.Domain.ApplicationReview>(It.IsAny<GetApplicationReviewByVacancyReferenceAndCandidateIdApiRequest>()), Times.Once);
        candidateApiClient.Verify(x => x.Get<Recruit.Domain.Application>(It.IsAny<GetApplicationsByVacancyReferenceApiRequest>()), Times.Never);
    }
}