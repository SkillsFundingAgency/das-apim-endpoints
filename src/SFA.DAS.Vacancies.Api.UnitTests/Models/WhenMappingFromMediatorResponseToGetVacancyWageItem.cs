using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetVacancyWageItem
    {
        [Test, AutoData]
        public void Then_The_Values_Are_Mapped(GetVacanciesListItem source)
        {
            var actual = (GetVacancyWageItem)source;
            
            actual.WageAmount.Should().Be(source.WageAmount);
            actual.WageType.Should().Be((WageType)source.WageType);
            actual.WageAdditionalInformation.Should().Be(source.WageText);
            actual.WageAmountUpperBound.Should().Be(source.WageAmountUpperBound);
            actual.WageAmountLowerBound.Should().Be(source.WageAmountLowerBound);
            actual.WorkingWeekDescription.Should().Be(source.WorkingWeek);
        }
    }
}