using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Application.TrainingCourses.Queries;
using SFA.DAS.Vacancies.InnerApi.Requests;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingRoutes
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Sectors_Returned_And_Added_To_Cache(
            GetRoutesQuery query,
            GetRoutesListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetRoutesQueryHandler handler
        )
        {
            //Arrange
            cacheStorageService.Setup(x => x.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse)))
                .ReturnsAsync((GetRoutesListResponse)default);
            apiClient.Setup(x => x.Get<GetRoutesListResponse>(It.IsAny<GetRoutesListRequest>())).ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.Routes.Should().BeEquivalentTo(apiResponse.Routes);
            cacheStorageService.Verify(x=>x.SaveToCache(nameof(GetRoutesListResponse), apiResponse, 23));
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Routes_Are_In_The_Cache_The_Api_Is_Not_Called(
            GetRoutesQuery query,
            GetRoutesListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetRoutesQueryHandler handler)
        {
            //Arrange
            cacheStorageService.Setup(x => x.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse)))
                .ReturnsAsync(apiResponse);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.Routes.Should().BeEquivalentTo(apiResponse.Routes);
            apiClient.Verify(x => x.Get<GetRoutesListResponse>(It.IsAny<GetRoutesListRequest>()), Times.Never);
        }
    }
}