using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;
public class WhenBuildingGetAllApprenticeshipsForAcademicYear
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Build(string ukprn, int academicyear, int page, int? pageSize = 20)
    {
        var actual = new GetAllApprenticeshipsForAcademicYearRequest(ukprn, academicyear, page, pageSize);

        actual.GetUrl.Should().Be($"/{ukprn}/academicyears/{academicyear}/apprenticeships?page={page}&pageSize={pageSize}");
    }
}
