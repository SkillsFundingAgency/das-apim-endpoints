using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetApplicationsToAutoDeclineRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed()
    {
        var actual = new GetApplicationsToAutoDeclineRequest();

        actual.GetUrl.Should().Be("/applications-auto-decline");
    }
}
