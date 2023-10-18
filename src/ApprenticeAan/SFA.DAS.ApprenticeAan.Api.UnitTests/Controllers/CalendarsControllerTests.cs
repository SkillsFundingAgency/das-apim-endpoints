using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.Calendars.Queries.GetCalendars;
using SFA.DAS.ApprenticeAan.Application.Model;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers;

public class CalendarsControllerTests
{
    [Test]
    [MoqAutoData]
    public async Task And_MediatorCommandSuccessful_Then_ReturnOk(
        List<Calendar> response,
        [Frozen] Mock<IMediator> mockMediator,
        CancellationToken cancellationToken)
    {
        mockMediator.Setup(m => m.Send(It.IsAny<GetCalendarsQuery>(), cancellationToken)).ReturnsAsync(response);
        var controller = new CalendarsController(mockMediator.Object);

        var result = await controller.GeCalendars(cancellationToken);

        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }
}