using SFA.DAS.AdminRoatp.Application.Commands.PatchProviderAllowedCourse;
using SFA.DAS.AdminRoatp.InnerApi.Requests;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class PatchProviderAllowedCourseRequestTests
{
    [Test]
    public void WhenBuildingRequest_ThenPatchUrlIsSetCorrectly()
    {
        // Arrange
        var command = new PatchProviderAllowedCourseCommand
        {
            UserId = "user-1",
            UserDisplayName = "John Smith",
            Ukprn = 12345678,
            LarsCode = "ABC123",
            LastDateStarts = new DateTime(2026, 8, 6, 15, 59, 51, DateTimeKind.Utc)
        };

        // Act
        var request = new PatchProviderAllowedCourseRequest(command);

        // Assert
        Assert.That(
            request.PatchUrl,
            Is.EqualTo("providers/12345678/allowed-courses/ABC123?userId=user-1&userDisplayName=John%20Smith"));
    }

    [Test]
    public void WhenBuildingRequest_ThenPatchDocumentIsCreated_WithLastDateStartsReplaceOperation()
    {
        // Arrange
        var lastDateStarts = new DateTime(2026, 8, 6, 15, 59, 51, DateTimeKind.Utc);

        var command = new PatchProviderAllowedCourseCommand
        {
            UserId = "user-1",
            UserDisplayName = "John Smith",
            Ukprn = 12345678,
            LarsCode = "ABC123",
            LastDateStarts = lastDateStarts
        };

        // Act
        var request = new PatchProviderAllowedCourseRequest(command);

        // Assert
        Assert.That(request.Data, Is.Not.Null);
        Assert.That(request.Data.Operations, Has.Count.EqualTo(1));

        var operation = request.Data.Operations.Single();

        Assert.Multiple(() =>
        {
            Assert.That(operation.op, Is.EqualTo("replace"));
            Assert.That(operation.path, Is.EqualTo("/LastDateStarts"));
            Assert.That(operation.value, Is.EqualTo(lastDateStarts));
        });
    }

    [Test]
    public void WhenBuildingRequest_ThenPatchDocumentIsCreated_WithNullLastDateStarts()
    {
        // Arrange
        var command = new PatchProviderAllowedCourseCommand
        {
            UserId = "user-1",
            UserDisplayName = "John Smith",
            Ukprn = 12345678,
            LarsCode = "ABC123",
            LastDateStarts = null
        };

        // Act
        var request = new PatchProviderAllowedCourseRequest(command);

        // Assert
        Assert.That(request.Data, Is.Not.Null);
        Assert.That(request.Data.Operations, Has.Count.EqualTo(1));

        var operation = request.Data.Operations.Single();

        Assert.Multiple(() =>
        {
            Assert.That(operation.op, Is.EqualTo("replace"));
            Assert.That(operation.path, Is.EqualTo("/LastDateStarts"));
            Assert.That(operation.value, Is.Null);
        });
    }
}
