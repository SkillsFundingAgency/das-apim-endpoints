using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Api.Models;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;

namespace SFA.DAS.EmployerAccounts.Api.UnitTests.Models;

public class WhenMappingGetEmployerVacanciesApiResponseFromMediatorType
{
    [Test, AutoData]
    public void Then_The_Values_Are_Mapped(VacancySummary result)
    {
        var actual = (VacancyApiResponse)result;
        
        actual.Should().BeEquivalentTo(result, options => options.Excluding(c=>c.Status));
        actual.Status.Should().Be(result.Status.ToString());
    }
}