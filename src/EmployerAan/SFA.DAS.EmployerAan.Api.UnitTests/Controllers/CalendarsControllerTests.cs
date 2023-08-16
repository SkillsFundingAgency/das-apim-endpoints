using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.Calendars.Queries.GetCalendars;
using SFA.DAS.EmployerAan.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers;
public class CalendarsControllerTests
{
    [Test]
    [MoqAutoData]
    public async Task And_MediatorCommandSuccessful_Then_ReturnOk(
        List<Calendar> response,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] CalendarsController sut,
        CancellationToken cancellationToken)
    {
        mockMediator.Setup(m => m.Send(It.IsAny<GetCalendarsQuery>(), cancellationToken)).ReturnsAsync(response);
        //var controller = new CalendarsController(mockMediator.Object);

        var result = await sut.GeCalendars(cancellationToken);

        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(response);
    }
}
