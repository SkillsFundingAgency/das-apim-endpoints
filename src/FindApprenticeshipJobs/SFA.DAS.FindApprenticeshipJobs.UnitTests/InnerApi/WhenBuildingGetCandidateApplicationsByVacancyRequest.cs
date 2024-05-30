using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.InnerApi;

public class WhenBuildingGetCandidateApplicationsByVacancyRequest
{
    [Test, AutoData]
    public void Then_The_Query_Is_Correctly_Constructed(Guid preferenceId, string vacancyReference)
    {
        var actual = new GetCandidateApplicationsByVacancyRequest(vacancyReference, preferenceId);

        actual.GetUrl.Should().Be($"api/Vacancies/{vacancyReference}/candidates?allowEmailContact=true&preferenceId={preferenceId}&applicationStatus=Draft");
    }
}