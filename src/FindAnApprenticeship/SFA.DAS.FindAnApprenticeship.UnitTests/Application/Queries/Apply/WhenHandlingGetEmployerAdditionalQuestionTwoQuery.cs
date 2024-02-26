using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetEmployerAdditionalQuestionTwo;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;
public class WhenHandlingGetEmployerAdditionalQuestionTwoQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_QueryResult_Is_Returned_As_Expected(
        GetEmployerAdditionalQuestionTwoQuery query,
        GetApprenticeshipVacancyItemResponse vacancyResponse,
        GetApplicationApiResponse applicationResponse,
        [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> findApprenticeshipApiClient,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetEmployerAdditionalQuestionTwoQueryHandler handler)
    {
        var expectedGetApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId);
        var expectedGetVacancyRequest = new GetVacancyRequest(applicationResponse.VacancyReference);

        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationRequest.GetUrl)))
            .ReturnsAsync(applicationResponse);

        findApprenticeshipApiClient
            .Setup(client => client.Get<GetApprenticeshipVacancyItemResponse>(
                It.Is<GetVacancyRequest>(r => r.GetUrl == expectedGetVacancyRequest.GetUrl)))
            .ReturnsAsync(vacancyResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<GetEmployerAdditionalQuestionTwoQueryResult>();
            candidateApiClient.Verify(p => p.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(x => x.GetUrl == expectedGetApplicationRequest.GetUrl)), Times.Once);
            findApprenticeshipApiClient.Verify(p => p.Get<GetApprenticeshipVacancyItemResponse>(It.Is<GetVacancyRequest>(x => x.GetUrl == expectedGetVacancyRequest.GetUrl)), Times.Once);
            result.QuestionTwo.Should().BeEquivalentTo(vacancyResponse.AdditionalQuestion2);
        }
    }
}
