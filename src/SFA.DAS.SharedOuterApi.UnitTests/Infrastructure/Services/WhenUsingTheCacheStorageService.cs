using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Infrastructure.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.SharedOuterApi.UnitTests.Infrastructure.Services
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

        public class TestObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}