using AutoFixture.NUnit3;
using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.AdminRoatp.InnerApi.Requests;

namespace SFA.DAS.AdminRoatp.UnitTests.InnerApi.Requests;

public class PutProviderAllowedCourseRequestTests
{
    [Test, AutoData]
    public void WhenBuildingRequest_ThenPatchUrlIsSetCorrectly(int ukprn, string larsCode, PutProviderAllowedCourseRequestModel model)
    {
        var sut = new PutProviderAllowedCourseRequest(ukprn, larsCode, model);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(sut.PutUrl, Is.EqualTo($"providers/{ukprn}/allowed-courses/{larsCode}"));
            Assert.That(sut.Data, Is.EqualTo(model));
        });
    }
}
