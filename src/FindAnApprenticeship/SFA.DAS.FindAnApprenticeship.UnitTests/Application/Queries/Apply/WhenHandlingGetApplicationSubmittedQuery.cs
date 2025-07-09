using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplicationSubmitted;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;
public class WhenHandlingGetApplicationSubmittedQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_QueryResult_Is_Returned_As_Expected(
        GetApplicationSubmittedQuery query,
        GetApplicationApiResponse application,
        GetApprenticeshipVacancyItemResponse vacancy,
        GetAboutYouItemApiResponse aboutYou,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<IVacancyService> vacancyService,
        GetApplicationSubmittedQueryHandler handler)
    {
        var expectedGetApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
        var expectedAboutYouRequest = new GetAboutYouItemApiRequest(query.CandidateId);

        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
            .ReturnsAsync(application);

        candidateApiClient
            .Setup(client => client.Get<GetAboutYouItemApiResponse>(
                It.Is<GetAboutYouItemApiRequest>(r => r.GetUrl == expectedAboutYouRequest.GetUrl)))
            .ReturnsAsync(aboutYou);

        vacancyService.Setup(x => x.GetVacancy(application.VacancyReference)).ReturnsAsync(vacancy);

        var result = await handler.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<GetApplicationSubmittedQueryResult>();
            candidateApiClient.Verify(p => p.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(x => x.GetUrl == expectedGetApplicationRequest.GetUrl)), Times.Once);
            vacancyService.Verify(p => p.GetVacancy(application.VacancyReference), Times.Once);
            result.EmployerName.Should().BeEquivalentTo(vacancy.EmployerName);
            result.VacancyTitle.Should().BeEquivalentTo(vacancy.Title);
            result.HasAnsweredEqualityQuestions.Should().Be(aboutYou.AboutYou != null);
            result.ClosedDate.Should().Be(vacancy.ClosedDate);
            result.ClosingDate.Should().Be(vacancy.ClosingDate);
        }
    }

    [Test, MoqAutoData]
    public async Task When_AboutYou_Is_Null_Then_HasCompletedEqualityQuestions_Is_False(
        GetApplicationSubmittedQuery query,
        GetApplicationApiResponse application,
        GetApprenticeshipVacancyItemResponse vacancy,
        GetAboutYouItemApiResponse aboutYou,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetApplicationSubmittedQueryHandler handler)
    {
        var expectedGetApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
        var expectedAboutYouRequest = new GetAboutYouItemApiRequest(query.CandidateId);

        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
            .ReturnsAsync(application);

        aboutYou.AboutYou = null;
        candidateApiClient
            .Setup(client => client.Get<GetAboutYouItemApiResponse>(
                It.Is<GetAboutYouItemApiRequest>(r => r.GetUrl == expectedAboutYouRequest.GetUrl)))
            .ReturnsAsync(aboutYou);

        var result = await handler.Handle(query, CancellationToken.None);

        result.HasAnsweredEqualityQuestions.Should().Be(false);
    }

    [Test, MoqAutoData]
    public void And_Application_Is_Null_Then_Exception_Is_Returned(
        GetApplicationSubmittedQuery query,
        GetApplicationApiResponse application,
        GetApprenticeshipVacancyItemResponse vacancy,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetApplicationSubmittedQueryHandler handler)
    {
        var expectedGetApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
                .Returns(Task.FromResult<GetApplicationApiResponse>(null!));

        Func<Task> result = () => handler.Handle(query, CancellationToken.None);

        _ = result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Test, MoqAutoData]
    public void And_Vacancy_Is_Null_Then_Exception_Is_Returned(
        GetApplicationSubmittedQuery query,
        GetApplicationApiResponse application,
        GetApprenticeshipVacancyItemResponse vacancy,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        [Frozen] Mock<IVacancyService> vacancyService,
        GetApplicationSubmittedQueryHandler handler)
    {
        var expectedGetApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
                .ReturnsAsync(application);

        vacancyService.Setup(x => x.GetVacancy(application.VacancyReference)).ReturnsAsync((GetApprenticeshipVacancyItemResponse)null!);

        Func<Task> result = () => handler.Handle(query, CancellationToken.None);

        _ = result.Should().ThrowAsync<InvalidOperationException>();
    }
}
