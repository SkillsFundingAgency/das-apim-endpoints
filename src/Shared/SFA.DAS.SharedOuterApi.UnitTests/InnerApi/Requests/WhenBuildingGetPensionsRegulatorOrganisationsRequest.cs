using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PensionRegulator;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetPensionsRegulatorOrganisationsRequest
{
    [Test]
    public void Then_The_Request_Is_Correctly_Build()
    {
        string aorn = "A094734889001";
        string payeRef = "307/NL8800";

        string expectedPayeRef = Uri.EscapeDataString(payeRef);

        var actual = new GetPensionsRegulatorOrganisationsRequest(aorn, payeRef);

        actual.GetUrl.Should().Be($"/api/PensionsRegulator/organisations/{aorn}?payeRef={expectedPayeRef}");
    }
}