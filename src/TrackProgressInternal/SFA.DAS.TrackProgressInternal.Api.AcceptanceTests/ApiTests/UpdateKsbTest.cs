using FluentAssertions;
using SFA.DAS.TrackProgressInternal.Api.AcceptanceTests.TestModels;
using SFA.DAS.TrackProgressInternal.Tests;
using WireMock.FluentAssertions;

namespace SFA.DAS.TrackProgressInternal.Api.AcceptanceTests.ApiTests;

public class UpdateKsbTest : ApiFixture
{
    [Test]
    public async Task Accepts_update_command_for_ksb()
    {
        var course = A.Course.WithStandard("the_course");

        factory.WithCourse(course);

        var response = await client.PostAsJsonAsync(
            $"/courses/the_course/ksbs", new { course.KsbIds });

        response.Should().Be200Ok();
    }

    [Test]
    public async Task Pass_all_ksb_ids_for_course_to_inner()
    {
        // Given
        var course = A.Course.WithKsbs(new("one"), new("two"), new("three"));

        factory.WithCourse(course);

        // When
        var response = await client.PostAsJsonAsync(
            $"/courses/{course.Standard}/ksbs", new { course.KsbIds });

        // Then
        factory.TrackProgressInnerApi.Server.Should().HaveReceivedACall()
            .AtUrl(InnerApiUrl($"courses/{course.Standard}/ksbs"));

        var request = factory.TrackProgressInnerApi.Server.LogEntries
            .Select(x => x.RequestMessage)
            .Where(x => x.Url.EndsWith("/ksbs"));

        request.First().Body.Should().BeValidJson<KsbHolder>()
            .Which.Should().BeEquivalentTo(
                new { Ksbs = course.Ksbs.ToArray() },
                o => o.For(x => x.Ksbs).Exclude(x => x.Description));
    }

    [Test]
    public async Task Pass_all_ksb_descriptions_for_course_to_inner()
    {
        // Given
        var course = A.Course.WithKsbs(new("one"), new("two"), new("three"));

        factory.WithCourse(course);

        // When
        var response = await client.PostAsJsonAsync(
            $"/courses/{course.Standard}/ksbs", new { course.KsbIds });

        // Then
        factory.TrackProgressInnerApi.Server.Should().HaveReceivedACall()
            .AtUrl(InnerApiUrl($"courses/{course.Standard}/ksbs"));

        var request = factory.TrackProgressInnerApi.Server.LogEntries
            .Select(x => x.RequestMessage)
            .Where(x => x.Url.EndsWith("/ksbs"));

        request.First().Body.Should().BeValidJson<KsbHolder>()
            .Which.Should().BeEquivalentTo(
                new { Ksbs = course.Ksbs.ToArray() });
    }

    public record KsbHolder(IEnumerable<Course.Ksb> Ksbs);
}