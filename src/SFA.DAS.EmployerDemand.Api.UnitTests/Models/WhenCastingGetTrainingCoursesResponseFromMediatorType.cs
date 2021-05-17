using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.InnerApi.Responses;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Models
{
    public class WhenCastingGetTrainingCoursesResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            GetStandardsListItem source)
        {
            var response = (GetCourseListItem)source;

            response.Should().BeEquivalentTo(source, options => options.Excluding(c=>c.LarsCode));
            response.Id.Should().Be(source.LarsCode);
        }
    }
}