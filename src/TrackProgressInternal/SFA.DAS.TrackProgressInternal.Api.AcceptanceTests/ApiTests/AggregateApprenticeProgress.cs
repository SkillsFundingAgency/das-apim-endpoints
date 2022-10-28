using FluentAssertions;
using SFA.DAS.TrackProgressInternal.Api.AcceptanceTests.TestModels;
using WireMock.FluentAssertions;

namespace SFA.DAS.TrackProgressInternal.Api.AcceptanceTests.ApiTests;

public class AggregateApprenticeProgress : ApiFixture
{
    [Test]
    public async Task Accepts_aggreateprogress_for_apprenticeship()
    {
        var apprenticeship = An.Apprenticeship.WithId(2);
        var course = A.Course;

        factory
            .WithApprenticeship(apprenticeship)
            .WithCourse(course);

        var response = await client.PostAsync(
            $"/apprenticeships/{apprenticeship.CommitmentsApprenticeshipId}/snapshot", null);

        response.Should().Be200Ok();
    }

    [Test]
    public async Task Forwards_aggregate_command_to_inner_api()
    {
        factory.WithApprenticeship(An.Apprenticeship.WithId(5));

        _ = await client.PostAsync(
            $"/apprenticeships/5/snapshot", null);

        factory.TrackProgressInnerApi.Server.Should().HaveReceivedACall()
            .AtUrl(InnerApiUrl("apprenticeships/5/snapshot"));
    }

    [Test]
    public async Task Inner_api_not_found_is_returned_to_caller()
    {
        var response = await client.PostAsync(
            $"/apprenticeships/77/snapshot", null);

        response.Should().Be404NotFound();
    }
}