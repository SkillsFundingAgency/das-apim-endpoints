using FluentAssertions;
using SFA.DAS.AdminRoatp.InnerApi.Requests;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class GetStandardsRequestTests
{
    [Test]
    public void WhenCreatingRequest_ThenSetsCorrectUrl()
    {
        var sut = new GetStandardsRequest();

        sut.GetUrl.Should().Be("standards");
    }
}
