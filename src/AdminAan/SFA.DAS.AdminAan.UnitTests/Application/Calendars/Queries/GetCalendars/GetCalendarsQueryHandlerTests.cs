using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminAan.Application.Calendars.Queries.GetCalendars;
using SFA.DAS.AdminAan.Application.Entities;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.UnitTests.Application.Calendars.Queries.GetCalendars;
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
