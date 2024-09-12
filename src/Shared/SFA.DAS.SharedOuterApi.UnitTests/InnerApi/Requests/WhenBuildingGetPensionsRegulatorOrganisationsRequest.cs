using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PensionRegulator;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetPensionsRegulatorOrganisationsRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(string aorn, string payeRef)
    {
        var actual = new GetPensionsRegulatorOrganisationsRequest(aorn, payeRef);

        actual.GetUrl.Should().Be($"/api/PensionsRegulator/organisations/{aorn}?payeRef={payeRef}");
    }
}