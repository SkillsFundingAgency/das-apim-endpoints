using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Application.TrainingCourses.Queries;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenHandlingGetTrainingCourses
    {
        [Test, MoqAutoData]
        public async Task And_Courses_Are_Taken_From_Service_And_Returned(
            GetTrainingCoursesQuery query,
            GetStandardsListResponse coursesFromCache,
            [Frozen] Mock<ICourseService> standardsService,
            GetTrainingCoursesQueryHandler handler)
        {
            standardsService
                .Setup(service => service.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse)))
                .ReturnsAsync(coursesFromCache);

            var result = await handler.Handle(query, CancellationToken.None);

            result.TrainingCourses.Should().BeEquivalentTo(coursesFromCache.Standards);
        }

    }
}