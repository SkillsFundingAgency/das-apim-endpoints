using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.InnerApi;

public class WhenBuildingGetLiveVacanciesApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(int pageSize, int pageNumber)
    {
        var actual = new GetLiveVacanciesApiRequest(pageNumber,pageSize, null);

        actual.GetUrl.Should().Be($"api/livevacancies?pageSize={pageSize}&pageNo={pageNumber}");
    }
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(int pageSize, int pageNumber, DateTime closingDate)
    {
        var actual = new GetLiveVacanciesApiRequest(pageNumber,pageSize, closingDate);

        actual.GetUrl.Should().Be($"api/livevacancies?pageSize={pageSize}&pageNo={pageNumber}&closingDate={closingDate.Date}");
    }
}