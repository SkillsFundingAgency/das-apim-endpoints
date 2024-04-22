using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplicationSubmmitted;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplicationSubmitted;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;
public class WhenHandlingGetApplicationSubmittedQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_QueryResult_Is_Returned_As_Expected(
        GetApplicationSubmittedQuery query,
        GetApplicationApiResponse application,
        GetApprenticeshipVacancyItemResponse vacancy,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetApplicationSubmittedQueryHandler handler)
    {
        var expectedGetApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
        var expectedGetVacancyRequest = new GetVacancyRequest(application.VacancyReference);

        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
            .ReturnsAsync(application);

        findApprenticeshipApiClient
            .Setup(client => client.Get<GetApprenticeshipVacancyItemResponse>(
                It.Is<GetVacancyRequest>(r => r.GetUrl == expectedGetVacancyRequest.GetUrl)))
            .ReturnsAsync(vacancy);

        var result = await handler.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<GetApplicationSubmittedQueryResult>();
            candidateApiClient.Verify(p => p.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(x => x.GetUrl == expectedGetApplicationRequest.GetUrl)), Times.Once);
            findApprenticeshipApiClient.Verify(p => p.Get<GetApprenticeshipVacancyItemResponse>(It.Is<GetVacancyRequest>(x => x.GetUrl == expectedGetVacancyRequest.GetUrl)), Times.Once);
            result.EmployerName.Should().BeEquivalentTo(vacancy.EmployerName);
            result.VacancyTitle.Should().BeEquivalentTo(vacancy.Title);
        }
    }

    [Test, MoqAutoData]
    public void And_Application_Is_Null_Then_Exception_Is_Returned(
        GetApplicationSubmittedQuery query,
        GetApplicationApiResponse application,
        GetApprenticeshipVacancyItemResponse vacancy,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetApplicationSubmittedQueryHandler handler)
    {
        var expectedGetApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
        var expectedGetVacancyRequest = new GetVacancyRequest(application.VacancyReference);

        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
                .Returns(Task.FromResult<GetApplicationApiResponse>(null!));

        findApprenticeshipApiClient
            .Setup(client => client.Get<GetApprenticeshipVacancyItemResponse>(
                It.Is<GetVacancyRequest>(r => r.GetUrl == expectedGetVacancyRequest.GetUrl)))
                .ReturnsAsync(vacancy);

        Func<Task> result = () => handler.Handle(query, CancellationToken.None);

        _ = result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Test, MoqAutoData]
    public void And_Vacancy_Is_Null_Then_Exception_Is_Returned(
        GetApplicationSubmittedQuery query,
        GetApplicationApiResponse application,
        GetApprenticeshipVacancyItemResponse vacancy,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetApplicationSubmittedQueryHandler handler)
    {
        var expectedGetApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
        var expectedGetVacancyRequest = new GetVacancyRequest(application.VacancyReference);

        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
                .ReturnsAsync(application);

        findApprenticeshipApiClient
            .Setup(client => client.Get<GetApprenticeshipVacancyItemResponse>(
                It.Is<GetVacancyRequest>(r => r.GetUrl == expectedGetVacancyRequest.GetUrl)))
            .Returns(Task.FromResult<GetApprenticeshipVacancyItemResponse>(null!));

        Func<Task> result = () => handler.Handle(query, CancellationToken.None);

        _ = result.Should().ThrowAsync<InvalidOperationException>();
    }
}
