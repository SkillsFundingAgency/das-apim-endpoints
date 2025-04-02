using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ToolsSupport.InnerApi.Requests;

namespace SFA.DAS.ToolsSupport.UnitTests.Requests;
public class WhenBuildingGetUserByIdRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(Guid id)
    {
        var actual = new GetUserByIdRequest(id);

        actual.GetUrl.Should().Be($"api/users/{id}");
    }
}
