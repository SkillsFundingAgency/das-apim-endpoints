using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Infrastructure.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.SharedOuterApi.UnitTests.Infrastructure.Services
{
    public class WhenUsingTheCacheStorageService
    {
        [Test, MoqAutoData]
        public async Task Then_The_Item_Is_Saved_To_The_Cache_Using_Key_And_ConfigName_And_Expiry_Passed(
            string keyName,
            int expiryInHours,
            string appName,
            TestObject test,
            [Frozen] Mock<IDistributedCache> distributedCache,
            [Frozen] Mock<IConfiguration> configuration,
            CacheStorageService service)
        {
            //Arrange
            configuration.SetupGet(x => x[It.Is<string>(s => s.Equals("ConfigNames"))]).Returns(appName);
            
            //Act
            await service.SaveToCache(keyName, test, expiryInHours);
            
            //Assert
            distributedCache.Verify(x=>
                x.SetAsync(
                    $"{appName}_{keyName}",
                    It.Is<byte[]>(c=>Encoding.UTF8.GetString(c).Equals(JsonSerializer.Serialize(test,new JsonSerializerOptions()))), 
                    It.Is<DistributedCacheEntryOptions>(c
                        => c.AbsoluteExpirationRelativeToNow.Value.Hours == TimeSpan.FromHours(expiryInHours).Hours),
                    It.IsAny<CancellationToken>()), 
                Times.Once);
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Item_Is_Saved_To_The_Cache_Using_Key_And_First_ConfigName_And_Expiry_Passed(
            string keyName,
            int expiryInHours,
            string appName,
            string appName2,
            TestObject test,
            [Frozen] Mock<IDistributedCache> distributedCache,
            [Frozen] Mock<IConfiguration> configuration,
            CacheStorageService service)
        {
            //Arrange
            configuration.SetupGet(x => x[It.Is<string>(s => s.Equals("ConfigNames"))]).Returns($"{appName},{appName2}");
            
            //Act
            await service.SaveToCache(keyName, test, expiryInHours);
            
            //Assert
            distributedCache.Verify(x=>
                    x.SetAsync(
                        $"{appName}_{keyName}",
                        It.Is<byte[]>(c=>Encoding.UTF8.GetString(c).Equals(JsonSerializer.Serialize(test,new JsonSerializerOptions()))), 
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
            string appName,
            [Frozen] Mock<IDistributedCache> distributedCache,
            [Frozen] Mock<IConfiguration> configuration,
            CacheStorageService service)
        {
            //Arrange
            configuration.SetupGet(x => x[It.Is<string>(s => s.Equals("ConfigNames"))]).Returns(appName);
            distributedCache.Setup(x => x.GetAsync($"{appName}_{keyName}", It.IsAny<CancellationToken>()))
                .ReturnsAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(test,new JsonSerializerOptions())));

            //Act
            var item = await service.RetrieveFromCache<TestObject>(keyName);
            
            //Assert
            Assert.That(item, Is.Not.Null);
            item.Should().BeEquivalentTo(test);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Item_Is_Retrieved_From_The_Cache_By_Key_For_Multiple_ConfigNames(
            string keyName,
            int expiryInHours,
            TestObject test,
            string appName,
            string appName2,
            [Frozen] Mock<IDistributedCache> distributedCache,
            [Frozen] Mock<IConfiguration> configuration,
            CacheStorageService service)
        {
            //Arrange
            configuration.SetupGet(x => x[It.Is<string>(s => s.Equals("ConfigNames"))]).Returns($"{appName},{appName2}");
            distributedCache.Setup(x => x.GetAsync($"{appName}_{keyName}", It.IsAny<CancellationToken>()))
                .ReturnsAsync(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(test,new JsonSerializerOptions())));

            //Act
            var item = await service.RetrieveFromCache<TestObject>(keyName);
            
            //Assert
            Assert.That(item, Is.Not.Null);
            item.Should().BeEquivalentTo(test);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_The_Item_Does_Not_Exist_Default_Is_Returned(
            string keyName,
            string appName,
            [Frozen] Mock<IDistributedCache> distributedCache,
            [Frozen] Mock<IConfiguration> configuration,
            CacheStorageService service)
        {
            //Arrange
            configuration.SetupGet(x => x[It.Is<string>(s => s.Equals("ConfigNames"))]).Returns(appName);
            distributedCache.Setup(x => x.GetAsync($"{appName}_{keyName}", It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[]) null);

            //Act
            var item = await service.RetrieveFromCache<TestObject>(keyName);
            
            //Assert
            Assert.That(item, Is.Null);
        }

        [Test, MoqAutoData]
        public async Task And_List_Does_Not_Exist_Then_Default_Is_Returned(
            string keyName,
            string appName,
            [Frozen] Mock<IDistributedCache> distributedCache,
            [Frozen] Mock<IConfiguration> configuration,
            CacheStorageService service)
        {
            //Arrange
            configuration.SetupGet(x => x[It.Is<string>(s => s.Equals("ConfigNames"))]).Returns(appName);
            distributedCache.Setup(x => x.GetAsync($"{appName}_{keyName}", It.IsAny<CancellationToken>()))
                .ReturnsAsync((byte[]) null);

            //Act
            var item = await service.RetrieveFromCache<List<TestObject>>(keyName);
            
            //Assert
            Assert.That(item, Is.Null);
        }
        
        [Test, MoqAutoData]
        public async Task Then_The_Item_Is_Removed_From_The_Cache(
            string keyName,
            string appName,
            [Frozen] Mock<IDistributedCache> distributedCache,
            [Frozen] Mock<IConfiguration> configuration,
            CacheStorageService service)
        {
            //Arrange
            configuration.SetupGet(x => x[It.Is<string>(s => s.Equals("ConfigNames"))]).Returns(appName);
            
            //Act
            await service.DeleteFromCache(keyName);
            
            //Assert
            distributedCache.Verify(x=>x.RemoveAsync($"{appName}_{keyName}", It.IsAny<CancellationToken>()), Times.Once);
        }

        public class TestObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}