using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Reservations.Api.Models;
using SFA.DAS.Reservations.InnerApi.Responses;

namespace SFA.DAS.Reservations.Api.UnitTests.Models
{
    public class WhenCastingGetTrainingCoursesResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            GetStandardsListItem source)
        {
            var response = (GetTrainingCoursesListItem)source;

            response.Should().BeEquivalentTo(source, options => options
                .Excluding(r => r.StandardUId)
                .Excluding(r => r.LarsCode));
            response.Id.Should().Be(source.LarsCode);
            response.ApprenticeshipType.Should().Be(source.ApprenticeshipType);
        }

        [Test, AutoData]
        public void Then_Maps_ApprenticeshipType_Values_Correctly()
        {
            var source = new GetStandardsListItem
            {
                ApprenticeshipType = "Apprenticeship"
            };
            var response = (GetTrainingCoursesListItem)source;
            response.ApprenticeshipType.Should().Be("Apprenticeship");
        }
    }
}