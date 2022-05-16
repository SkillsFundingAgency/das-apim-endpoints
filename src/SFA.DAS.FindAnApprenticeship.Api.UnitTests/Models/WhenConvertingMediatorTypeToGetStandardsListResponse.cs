using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.ApiResponses;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCourses;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models
{
    public class WhenConvertingMediatorTypeToGetStandardsListResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetCoursesQueryResult source)
        {
            var actual = (GetCoursesResponse) source;

            actual.TrainingProgrammes.Should().BeEquivalentTo(source.TrainingProgrammes);
        }
    }
}