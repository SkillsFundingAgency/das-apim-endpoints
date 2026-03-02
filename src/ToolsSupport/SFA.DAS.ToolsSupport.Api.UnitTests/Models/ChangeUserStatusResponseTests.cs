using ChangeUserStatusResponse = SFA.DAS.ToolsSupport.Api.Models.Users.ChangeUserStatusResponse;

namespace SFA.DAS.ToolsSupport.Api.UnitTests.Models;

public class ChangeUserStatusResponseTests
{
    [Test]
    public void FromInnerResponse_ReturnsNull_When_Response_IsNull()
    {
        var result = ChangeUserStatusResponse.FromInnerResponse(null);

        result.Should().BeNull();
    }

    [Test]
    public void FromInnerResponse_Maps_Id_And_Errors()
    {
        var inner = new SFA.DAS.ToolsSupport.InnerApi.Responses.ChangeUserStatusResponse
        {
            Id = "123",
            Errors = new Dictionary<string, string> { { "error", "message" } }
        };

        var result = ChangeUserStatusResponse.FromInnerResponse(inner);

        result.Should().NotBeNull();
        result!.Id.Should().Be(inner.Id);
        result.Errors.Should().BeEquivalentTo(inner.Errors);
    }
}

