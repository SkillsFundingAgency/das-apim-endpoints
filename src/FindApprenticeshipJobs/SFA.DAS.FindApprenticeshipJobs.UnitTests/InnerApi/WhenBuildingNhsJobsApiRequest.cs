using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.InnerApi;

public class WhenBuildingNhsJobsApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(int pageNumber)
    {
        var actual = new GetNhsJobsApiRequest(pageNumber);

        actual.GetUrl.Should().Be($"?contractType=Apprenticeship&page={pageNumber}");
    }
}