using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ToolsSupport.InnerApi.Requests;

namespace SFA.DAS.ToolsSupport.UnitTests.Requests;
public class WhenBuildingGetEmployerAccountByPayeRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(string payeRef)
    {
        var actual = new GetEmployerAccountByPayeRequest(payeRef);

        actual.GetUrl.Should().Be($"api/accounthistories?payeRef={WebUtility.UrlEncode(payeRef)}");
    }
}
