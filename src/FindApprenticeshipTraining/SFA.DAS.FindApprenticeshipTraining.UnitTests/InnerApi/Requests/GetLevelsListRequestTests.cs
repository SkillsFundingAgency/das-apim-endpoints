using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;

public class GetLevelsListRequestTests
{
    [Test, AutoData]
    public void WhenBuildingGetLevelsListRequest_ReturnsExpectedUrl()
    {
        var actual = new GetLevelsListRequest();

        actual.GetUrl.Should().Be("api/courses/levels");

    }

}