using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingGetLocationsByPostBulkPostcodeRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(List<string> postCodes)
    {
        var actual = new GetLocationsByPostBulkPostcodeRequest(postCodes);

        actual.PostUrl.Should().Be("api/Postcodes/bulk");
        ((List<string>)actual.Data).Should().BeEquivalentTo(postCodes);
    }
}