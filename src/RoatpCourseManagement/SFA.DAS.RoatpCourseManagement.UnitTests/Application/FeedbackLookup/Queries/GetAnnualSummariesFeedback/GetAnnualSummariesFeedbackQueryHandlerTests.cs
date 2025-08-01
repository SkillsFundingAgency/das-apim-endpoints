using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.FeedbackLookup.Queries.GetAnnualSummariesFeedback;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ApprenticeFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ApprenticeFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFeedback;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.FeedbackLookup.Queries.GetAnnualSummariesFeedback;

public class GetAnnualSummariesFeedbackQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handler_AddsEmployerFeedbackToResponse(
        [Frozen] Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> employerFeedbackApiClientMock,
        GetAnnualSummariesFeedbackQueryHandler sut,
        EmployerFeedbackStarsSummary[] expected,
        string timePeriod)
    {
        employerFeedbackApiClientMock
            .Setup(a => a.Get<IEnumerable<EmployerFeedbackStarsSummary>>(It.IsAny<GetEmployerFeedbackAnnualReviewsRequest>()))
            .ReturnsAsync(expected);
        var actual = await sut.Handle(new GetAnnualSummariesFeedbackQuery(timePeriod), default);
        employerFeedbackApiClientMock
            .Verify(a => a.Get<IEnumerable<EmployerFeedbackStarsSummary>>(It.IsAny<GetEmployerFeedbackAnnualReviewsRequest>()), Times.Once);
        Assert.That(actual.EmployersFeedback, Is.EqualTo(expected));
    }

    [Test, MoqAutoData]
    public async Task Handler_AddsApprenticesFeedbackToResponse(
        [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> apprenticeFeedbackApiClientMock,
        GetAnnualSummariesFeedbackQueryHandler sut,
        ApprenticeFeedbackStarsSummary[] expected,
        string timePeriod)
    {
        apprenticeFeedbackApiClientMock
            .Setup(a => a.Get<IEnumerable<ApprenticeFeedbackStarsSummary>>(It.IsAny<GetApprenticeFeedbackAnnualReviewsRequest>()))
            .ReturnsAsync(expected);
        var actual = await sut.Handle(new GetAnnualSummariesFeedbackQuery(timePeriod), default);
        Assert.That(actual.ApprenticesFeedback, Is.EqualTo(expected));
    }
}
