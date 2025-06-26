using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Learning;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;
public class WhenBuildingGetAllApprenticeshipsForAcademicYear
{
    [Test, AutoData]
    public void Then_The_Request_Is_Correctly_Built(string ukprn, int academicYear, int page, int? pageSize)
    {
        var actual = new GetAllLearningsRequest(ukprn, academicYear, page, pageSize);

        actual.GetUrl.Should().Be($"/{ukprn}/academicyears/{academicYear}/learnings?page={page}&pageSize={pageSize}");
    }
}