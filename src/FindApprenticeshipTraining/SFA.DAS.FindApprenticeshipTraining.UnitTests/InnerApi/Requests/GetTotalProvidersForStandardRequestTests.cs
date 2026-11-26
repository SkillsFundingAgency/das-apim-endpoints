using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests;

public class WhenBuildingGetTotalProvidersForStandardRequest
{
    [Test, AutoData]
    public void WhenBuildingGetTotalProvidersForStandardRequest_ThenUrlIsCorrectlyConstructed(int standardId)
    {
        var actual = new GetTotalProvidersForStandardRequest(standardId);
        actual.GetUrl.Should().Be($"/api/courses/{standardId}/providers/count");
    }
}
