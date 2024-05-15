using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.InnerApi;

public class WhenBuildingGetLiveVacancyApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(int vacancyReference)
    {
        var actual = new GetLiveVacancyApiRequest(vacancyReference);

        actual.GetUrl.Should().Be($"api/livevacancies/{vacancyReference}");
    }
}