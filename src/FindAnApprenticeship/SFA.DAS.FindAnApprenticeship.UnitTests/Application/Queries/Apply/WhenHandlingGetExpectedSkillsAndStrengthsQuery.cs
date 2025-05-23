﻿using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetExpectedSkillsAndStrengths;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;
public class WhenHandlingGetExpectedSkillsAndStrengthsQuery
{
    [Test]
    [MoqInlineAutoData(false, Constants.SectionStatus.Incomplete)]
    [MoqInlineAutoData(true, Constants.SectionStatus.Completed)]
    public async Task Then_The_QueryResult_Is_Returned_As_Expected(
        bool isSectionCompleted,
        string sectionStatus,
        GetExpectedSkillsAndStrengthsQuery query,
        GetApplicationApiResponse application,
        GetApprenticeshipVacancyItemResponse vacancy,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetExpectedSkillsAndStrengthsQueryHandler handler)
    {
        application.SkillsAndStrengthStatus = sectionStatus;

        var expectedGetApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
        var expectedGetVacancyRequest = new GetVacancyRequest(application.VacancyReference.ToString());

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
            result.Should().BeOfType<GetExpectedSkillsAndStrengthsQueryResult>();
            candidateApiClient.Verify(p => p.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(x => x.GetUrl == expectedGetApplicationRequest.GetUrl)), Times.Once);
            findApprenticeshipApiClient.Verify(p => p.Get<GetApprenticeshipVacancyItemResponse>(It.Is<GetVacancyRequest>(x => x.GetUrl == expectedGetVacancyRequest.GetUrl)), Times.Once);
            result.Employer.Should().BeEquivalentTo(vacancy.EmployerName);
            result.ExpectedSkillsAndStrengths.Should().BeEquivalentTo(vacancy.Skills);
            result.ApplicationId.Should().Be(application.Id);
            result.IsSectionCompleted.Should().Be(isSectionCompleted);
        }
    }

    [Test, MoqAutoData]
    public void And_Application_Is_Null_Then_Exception_Is_Returned(
        GetExpectedSkillsAndStrengthsQuery query,
        GetApplicationApiResponse application,
        GetApprenticeshipVacancyItemResponse vacancy,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetExpectedSkillsAndStrengthsQueryHandler handler)
    {
        var expectedGetApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
        var expectedGetVacancyRequest = new GetVacancyRequest(application.VacancyReference.ToString());

        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
                .Returns(Task.FromResult<GetApplicationApiResponse>(null!));

        findApprenticeshipApiClient
            .Setup(client => client.Get<GetApprenticeshipVacancyItemResponse>(
                It.Is<GetVacancyRequest>(r => r.GetUrl == expectedGetVacancyRequest.GetUrl)))
                .ReturnsAsync(vacancy);

        Func<Task> result = () => handler.Handle(query, CancellationToken.None);

        _= result.Should().ThrowAsync<InvalidOperationException>();
    }

    [Test, MoqAutoData]
    public void And_Vacancy_Is_Null_Then_Exception_Is_Returned(
        GetExpectedSkillsAndStrengthsQuery query,
        GetApplicationApiResponse application,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetExpectedSkillsAndStrengthsQueryHandler handler)
    {
        var expectedGetApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);
        var expectedGetVacancyRequest = new GetVacancyRequest(application.VacancyReference.ToString());

        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
                .ReturnsAsync(application);

        findApprenticeshipApiClient
            .Setup(client => client.Get<GetApprenticeshipVacancyItemResponse>(
                It.Is<GetVacancyRequest>(r => r.GetUrl == expectedGetVacancyRequest.GetUrl)))
            .Returns(Task.FromResult<GetApprenticeshipVacancyItemResponse>(null!));    

        Func<Task> result = () => handler.Handle(query, CancellationToken.None);

        _= result.Should().ThrowAsync<InvalidOperationException>();
    }
}
