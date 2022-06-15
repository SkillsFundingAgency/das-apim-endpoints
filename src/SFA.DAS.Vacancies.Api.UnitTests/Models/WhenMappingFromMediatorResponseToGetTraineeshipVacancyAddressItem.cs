using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.UnitTests.Models
{
    public class WhenMappingFromMediatorResponseToGetTraineeshipVacancyAddressItem
    {
        [Test, AutoData]
        public void Then_The_Values_Are_Mapped(GetTraineeshipVacanciesListItem source)
        {
            var actual = (GetTraineeshipVacancyAddressItem)source;

            actual.Should().BeEquivalentTo(source.Address);
        }

        [Test, AutoData]
        public void Then_Empty_Object_Returned_If_Null(GetTraineeshipVacanciesListItem source)
        {
            source.Address = null;

            var actual = (GetTraineeshipVacancyAddressItem)source;

            actual.Should().BeEquivalentTo(new GetVacancyAddressItem());
        }
    }
}