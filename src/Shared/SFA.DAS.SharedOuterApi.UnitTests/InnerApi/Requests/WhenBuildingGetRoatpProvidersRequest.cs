using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public sealed class WhenBuildingGetRoatpProvidersRequest
{
    [Test]
    [TestCase(true, "api/providers?Live=true", TestName = "Live_True_Should_Return_Correct_URL")]
    [TestCase(false, "api/providers?Live=false", TestName = "Live_False_Should_Return_Correct_URL")]
    [TestCase(null, "api/providers", TestName = "Live_Null_Should_Not_Include_QueryParam")]
    public void Then_GetUrl_Should_Build_Correct_QueryString(bool? live, string expectedUrl)
    {
        var request = new GetRoatpProvidersRequest { Live = live };
        var _sut = request.GetUrl;
        Assert.That(_sut, Is.EqualTo(expectedUrl));
    }
}
