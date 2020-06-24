using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Application.TrainingCourses.Queries.GetTrainingCoursesList;
using SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Application.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingTrainingCourseList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_And_Sectors_From_Courses_Api(
            GetTrainingCoursesListQuery query,
            GetStandardsListResponse apiResponse,
            GetSectorsListResponse sectorsApiResponse,
            [Frozen] Mock<IApiClient> mockApiClient,
            GetTrainingCoursesListQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.Is<GetStandardsListRequest>(c=>c.Keyword.Equals(query.Keyword))))
                .ReturnsAsync(apiResponse);
            mockApiClient
                .Setup(client => client.Get<GetSectorsListResponse>(It.IsAny<GetSectorsListRequest>()))
                .ReturnsAsync(sectorsApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Courses.Should().BeEquivalentTo(apiResponse.Standards);
            result.Sectors.Should().BeEquivalentTo(sectorsApiResponse.Sectors);
            result.Total.Should().Be(apiResponse.Total);
            result.TotalFiltered.Should().Be(apiResponse.TotalFiltered);
        }
    }
}

