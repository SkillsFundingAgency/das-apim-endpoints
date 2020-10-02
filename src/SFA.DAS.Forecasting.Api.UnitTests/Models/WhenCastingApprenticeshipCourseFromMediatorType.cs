using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.InnerApi.Responses;

namespace SFA.DAS.Forecasting.Api.UnitTests.Models
{
    public class WhenCastingApprenticeshipCourseFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_GetStandardsListItem_Correctly(
            GetStandardsListItem source)
        {
            var response = (ApprenticeshipCourse)source;

            response.Should().BeEquivalentTo(source);
        }

        [Test, AutoData]
        public void Then_Maps_GetFrameworksListItem_Correctly(
            GetFrameworksListItem source)
        {
            var response = (ApprenticeshipCourse)source;

            response.Should().BeEquivalentTo(source);
        }
    }
}