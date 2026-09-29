using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using SFA.DAS.AdminRoatp.InnerApi.Requests;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class GetProviderCourseRequestTests
{
    [Test, AutoData]
    public void WhenCreatingRequest_ThenSetsCorrectUrl(int ukprn, string larsCode)
    {
        var sut = new GetProviderCourseRequest(ukprn, larsCode);

        using (new AssertionScope())
        {
            sut.Ukprn.Should().Be(ukprn);
            sut.LarsCode.Should().Be(larsCode);
            sut.GetUrl.Should().Be($"providers/{ukprn}/courses/{larsCode}");
        }
    }
}
