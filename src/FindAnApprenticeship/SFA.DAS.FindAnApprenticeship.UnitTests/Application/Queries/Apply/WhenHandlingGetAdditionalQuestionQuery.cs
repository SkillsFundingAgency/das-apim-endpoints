using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAdditionalQuestion;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply;
public class WhenHandlingGetAdditionalQuestionQuery
{
    [Test]
    [MoqInlineAutoData(1, Domain.Constants.SectionStatus.Completed, true)]
    [MoqInlineAutoData(1, Domain.Constants.SectionStatus.Incomplete, false)]
    [MoqInlineAutoData(2, Domain.Constants.SectionStatus.Completed, true)]
    [MoqInlineAutoData(2, Domain.Constants.SectionStatus.Incomplete, false)]
    public async Task Then_The_QueryResult_Is_Returned_As_Expected(
        int additionalQuestion,
        string sectionStatus,
        bool isSectionCompleted,
        GetAdditionalQuestionQuery query,
        GetAdditionalQuestionApiResponse questionResponse,
        GetApplicationApiResponse applicationApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        GetAdditionalQuestionQueryHandler handler)
    {
        query.AdditionalQuestion = additionalQuestion;
        applicationApiResponse.AdditionalQuestion1Status = sectionStatus;
        applicationApiResponse.AdditionalQuestion2Status = sectionStatus;

        var expectedRequest = new GetAdditionalQuestionApiRequest(query.ApplicationId, query.CandidateId, query.Id);

        candidateApiClient
            .Setup(client => client.Get<GetAdditionalQuestionApiResponse>(
                It.Is<GetAdditionalQuestionApiRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
            .ReturnsAsync(questionResponse);

        var expectedApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId, false);

        candidateApiClient
            .Setup(client => client.Get<GetApplicationApiResponse>(
                It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedApplicationRequest.GetUrl)))
            .ReturnsAsync(applicationApiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeOfType<GetAdditionalQuestionQueryResult>();
            candidateApiClient.Verify(p => p.Get<GetAdditionalQuestionApiResponse>(It.Is<GetAdditionalQuestionApiRequest>(x => x.GetUrl == expectedRequest.GetUrl)), Times.Once);
            result.QuestionText.Should().BeEquivalentTo(questionResponse.QuestionText);
            result.IsSectionCompleted.Should().Be(isSectionCompleted);
        }
    }
}
