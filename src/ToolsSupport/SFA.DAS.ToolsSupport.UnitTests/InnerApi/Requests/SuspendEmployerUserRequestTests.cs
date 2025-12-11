using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ToolsSupport.InnerApi.Requests;

namespace SFA.DAS.ToolsSupport.UnitTests.InnerApi.Requests;

public class SuspendEmployerUserRequestTests
{
    [Test]
    public void Suspend_Request_Sets_PostUrl_And_Data()
    {
        var data = new ChangeUserStatusRequestData { ChangedByEmail = "test@test.com", ChangedByUserId = "user" };

        var request = new SuspendEmployerUserRequest("ABC123", data);

        request.PostUrl.Should().Be("api/users/ABC123/suspend");
        request.Data.Should().Be(data);
    }

    [Test]
    public void Resume_Request_Sets_PostUrl_And_Data()
    {
        var data = new ChangeUserStatusRequestData { ChangedByEmail = "resume@test.com", ChangedByUserId = "user" };

        var request = new ResumeEmployerUserRequest("ABC123", data);

        request.PostUrl.Should().Be("api/users/ABC123/resume");
        request.Data.Should().Be(data);
    }
}

