using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Api.Models;

namespace SFA.DAS.EmployerDemand.Api.UnitTests.Models
{
    public class WhenCastingGetAggregatedCourseDemandSummaryFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields(InnerApi.Responses.GetAggreatedCourseDemandSummaryResponse source)
        {
            var result = (GetAggregatedCourseDemandSummary)source;

            result.Should().BeEquivalentTo(source, options => options.ExcludingMissingMembers());
            result.TrainingCourse.Should().BeEquivalentTo(new GetCourseListItem
            {
                Id = source.CourseId,
                Level = source.CourseLevel,
                Route = source.CourseRoute,
                Title = source.CourseTitle
            });
        }
    }
}