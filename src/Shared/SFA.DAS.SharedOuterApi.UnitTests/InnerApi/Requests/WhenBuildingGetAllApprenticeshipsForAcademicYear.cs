using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;
public class WhenBuildingGetAllApprenticeshipsForAcademicYear
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(string ukprn, string startDate, string endDate, int page, int? pageSize = 20)
    {
        var actual = new GetAllApprenticeshipsByDatesRequest(ukprn, startDate, endDate, page, pageSize);

        actual.GetUrl.Should().Be($"/{ukprn}/apprenticeships/by-dates?startDate={startDate}&endDate={endDate}&page={page}&pageSize={pageSize}");
    }
}
