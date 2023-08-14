using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerAan.Application.Calendars.Queries.GetCalendars;
using SFA.DAS.EmployerAan.Entities;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.Calendars.Queries.GetCalendars;
public class GeCalendarsQueryHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_ReturnAllCalendars(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetCalendarsQueryHandler handler,
        GetCalendarsQuery query,
        List<Calendar> expected,
        CancellationToken cancellationToken)
    {
        apiClient.Setup(x => x.GetCalendars(cancellationToken)).ReturnsAsync(expected);

        var actual = await handler.Handle(query, cancellationToken);

        actual.ToList().Should().BeEquivalentTo(expected);
    }
}
