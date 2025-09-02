using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Api.Controllers;
using SFA.DAS.RoatpCourseManagement.Application.FeedbackLookup.Queries.GetAnnualSummariesFeedback;

namespace SFA.DAS.RoatpCourseManagement.Api.UnitTests.Controllers;

public class FeedbackLookupControllerTests
{
    [Test, AutoData]
    public async Task GetAnnualSummarisedFeedback_InvokesMediator_ReturnsOk(string academicYear, GetAnnualSummariesFeedbackQueryResult expected)
    {
        // Arrange
        Mock<IMediator> mockMediator = new();
        mockMediator.Setup(m => m.Send(It.IsAny<GetAnnualSummariesFeedbackQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expected);
        FeedbackLookupController controller = new(mockMediator.Object);

        // Act
        var result = await controller.GetAnnualSummarisedFeedback(academicYear, CancellationToken.None);

        // Assert
        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(expected);
    }
}
