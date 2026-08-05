using AutoFixture.NUnit3;
using SFA.DAS.AdminRoatp.Application.Commands.AddRestrictedCourse;
using SFA.DAS.AdminRoatp.InnerApi.Requests;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class AddRestrictedCourseRequestTests
{

    [Test, AutoData]
    public void WhenBuildingRequest_ThenSetsDataCorrectly(AddRestrictedCourseCommand command)
    {
        // Act
        var request = new AddRestrictedCourseRequest(command);
        var data = (AddRestrictedCourseCommand)request.Data;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(data.LarsCode, Is.EqualTo(command.LarsCode));
            Assert.That(data.UserId, Is.EqualTo(command.UserId));
            Assert.That(data.UserDisplayName, Is.EqualTo(command.UserDisplayName));
        });
    }

    [Test, AutoData]
    public void WhenBuildingRequest_ThenPostUrlIsSetCorrectly(AddRestrictedCourseCommand command)
    {
        // Act
        var request = new AddRestrictedCourseRequest(command);

        // Assert
        Assert.That(request.PostUrl, Is.EqualTo("restricted-courses"));
    }
}
