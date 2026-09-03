using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.AdminRoatp.InnerApi.Requests;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class GetStandardByLarsCodeRequestTests
{
    [Test, AutoData]
    public void WhenCreatingRequest_ThenSetsCorrectUrl(string larsCode)
    {
        //Act
        var sut = new GetStandardByLarsCodeRequest(larsCode);

        // Assert
        sut.GetUrl.Should().Be($"standards/{larsCode}");
    }
}
