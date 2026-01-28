using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests;

[TestFixture]
public class GetProviderCourseTypesRequestTests
{
    [Test, AutoData]
    public void Constructor_ConstructsRequest(int ukprn)
    {
        var request = new GetProviderCourseTypesRequest(ukprn);

        request.Ukprn.Should().Be(ukprn);
        request.GetUrl.Should().Be($"providers/{ukprn}/course-types");
    }
}