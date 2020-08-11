using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Infrastructure.Services;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Infrastructure.Services
{
    public class WhenUsingTheCacheStorageService
    {
        [Test, MoqAutoData]
        public async Task Then_The_Item_Is_Saved_To_The_Cache_Using_Key_And_Expiry_Passed(
            string keyName,
            int expiryInHours,
            TestObject test,
            [Frozen] Mock<IDistributedCache> distributedCache,
            CacheStorageService service)
        {
            //Act
            await service.SaveToCache(keyName, test, expiryInHours);
            
            //Assert
            distributedCache.Verify(x=>
                x.SetAsync(
                    keyName,
                    It.Is<byte[]>(c=>Encoding.UTF8.GetString(c).Equals(JsonConvert.SerializeObject(test))), 
                    It.Is<DistributedCacheEntryOptions>(c
                        => c.AbsoluteExpirationRelativeToNow.Value.Hours == TimeSpan.FromHours(expiryInHours).Hours),
                    It.IsAny<CancellationToken>()), 
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Item_Is_Retrieved_From_The_Cache_By_Key(
            string keyName,
            int expiryInHours,
            TestObject test,
            [Frozen] Mock<IDistributedCache> distributedCache,
            CacheStorageService service)
        {
            //Arrange
            distributedCache.Setup(x => x.GetAsync(keyName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(test)));

            //Act
            var item = await service.RetrieveFromCache<TestObject>(keyName);
            
            //Assert
            Assert.IsNotNull(item);
            item.Should().BeEquivalentTo(test);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Item_Does_Not_Exist_Default_Is_Returned(
            string keyName,
            [Frozen] Mock<IDistributedCache> distributedCache,
            CacheStorageService service)
        {
            //Arrange
            distributedCache.Setup(x => x.GetAsync(keyName, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new byte[0]);

            //Act
            var item = await service.RetrieveFromCache<TestObject>(keyName);
            
            //Assert
            Assert.IsNull(item);
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Item_Is_Removed_From_The_Cache(
            string keyName,
            [Frozen] Mock<IDistributedCache> distributedCache,
            CacheStorageService service)
        {
            //Act
            await service.DeleteFromCache(keyName);
            
            //Assert
            distributedCache.Verify(x=>x.RemoveAsync(keyName, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_No_Items_Are_Updated_In_Cache_If_Not_Required(
            Task<GetSectorsListResponse> sectorsTask,
            Task<GetLevelsListResponse> levelsTask,
            Task<GetStandardsListResponse> standardsTask,
            [Frozen] Mock<IDistributedCache> distributedCache,
            CacheStorageService service)
        {
            //Arrange
            var saveToCache = new SaveToCache {Levels = false, Sectors = false, Standards = false};

            //Act
            await service.UpdateCachedItems(sectorsTask, levelsTask, standardsTask, saveToCache);

            //Assert
            distributedCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_Items_Are_Updated_In_Cache_If_Required(
            Task<GetSectorsListResponse> sectorsTask,
            Task<GetLevelsListResponse> levelsTask,
            Task<GetStandardsListResponse> standardsTask,
            [Frozen] Mock<IDistributedCache> distributedCache,
            CacheStorageService service)
        {
            //Arrange
            var saveToCache = new SaveToCache { Levels = true, Sectors = true, Standards = true };

            //Act
            await service.UpdateCachedItems(sectorsTask, levelsTask, standardsTask, saveToCache);

            //Assert
            distributedCache.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
        }

        [Test, MoqAutoData, Ignore("WIP")]
        public async Task Then_Present_Items_Are_Returned_From_Cache(
            GetSectorsListRequest request,
            [Frozen] Mock<IDistributedCache> distributedCacheMock,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> clientMock,
        CacheStorageService service)
        {
            //Arrange
            //distributedCacheMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            //    .ReturnsAsync("notnull");
            var outFalse = false;

            //Act
            await service.GetRequest<GetSectorsListResponse>(clientMock.Object, request, nameof(GetSectorsListRequest),
                out outFalse);

            //Assert
            clientMock.Verify(x => x.Get<GetSectorsListResponse>(It.IsAny<GetSectorsListRequest>()), Times.Never);
        }
        public class TestObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}