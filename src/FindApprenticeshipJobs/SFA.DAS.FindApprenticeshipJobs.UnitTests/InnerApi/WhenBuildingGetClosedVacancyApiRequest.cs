using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.InnerApi;

public class WhenBuildingGetClosedVacancyApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(int vacancyReference)
    {
        var actual = new GetClosedVacancyApiRequest(vacancyReference);

        actual.GetUrl.Should().Be($"api/closedvacancies/{vacancyReference}");
    }
}