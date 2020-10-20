using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourse;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingTrainingCourse
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Courses_Api_With_Totals(
            GetTrainingCourseQuery query,
            GetStandardsListItem coursesApiResponse,
            GetUkprnsForStandardAndLocationResponse courseDirectoryApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockCourseDeliveryApiClient,
            GetTrainingCourseQueryHandler handler)
        {
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.Id.ToString())), true))
                .ReturnsAsync(coursesApiResponse);

            var url = new GetUkprnsForStandardAndLocationRequest(query.Id, query.Lat, query.Lon).GetUrl;
            mockCourseDeliveryApiClient
                .Setup(client =>
                    client.Get<GetUkprnsForStandardAndLocationResponse>(
                        It.Is<GetUkprnsForStandardAndLocationRequest>((c =>
                            c.GetUrl.Equals(url))), true))
                .ReturnsAsync(courseDirectoryApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Course.Should().BeEquivalentTo(coursesApiResponse);
            result.ProvidersCount.Should().Be(courseDirectoryApiResponse.UkprnsByStandard.ToList().Count);
            result.ProvidersCountAtLocation.Should().Be(courseDirectoryApiResponse.UkprnsByStandardAndLocation.ToList().Count);
        }
    }
}