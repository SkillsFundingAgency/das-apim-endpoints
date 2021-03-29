using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Application.Queries.Sectors;
using SFA.DAS.Campaign.InnerApi.Requests;
using SFA.DAS.Campaign.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries.Sectors
{
    public class WhenGettingSectors
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Sectors_Returned_And_Added_To_Cache(
            GetSectorsQuery query,
            GetRoutesListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetSectorsQueryHandler handler
        )
        {
            //Arrange
            cacheStorageService.Setup(x => x.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse)))
                .ReturnsAsync((GetRoutesListResponse)default);
            apiClient.Setup(x => x.Get<GetRoutesListResponse>(It.IsAny<GetRoutesListRequest>())).ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.Sectors.Should().BeEquivalentTo(apiResponse.Routes);
            cacheStorageService.Verify(x=>x.SaveToCache(nameof(GetRoutesListResponse), apiResponse, 23));
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Sectors_Are_In_The_Cache_The_Api_Is_Not_Called(
            GetSectorsQuery query,
            GetRoutesListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> apiClient,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            GetSectorsQueryHandler handler)
        {
            //Arrange
            cacheStorageService.Setup(x => x.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse)))
                .ReturnsAsync(apiResponse);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.Sectors.Should().BeEquivalentTo(apiResponse.Routes);
            apiClient.Verify(x => x.Get<GetRoutesListResponse>(It.IsAny<GetRoutesListRequest>()), Times.Never);
        }
    }
}