using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services
{
    public class WhenCallingLearnerNotificationsInnerApiClient
    {
        [Test, MoqAutoData]
        public async Task Then_Get_Is_Delegated_To_InternalApiClient(
            [Frozen] Mock<IInternalApiClient<LearnerNotificationsApiConfiguration>> internalApiClientMock,
            LearnerNotificationsInnerApiClient sut)
        {
            //Arrange
            var request = new Mock<IGetApiRequest>();
            internalApiClientMock
                .Setup(x => x.Get<string>(It.IsAny<IGetApiRequest>()))
                .ReturnsAsync("response");

            //Act
            var result = await sut.Get<string>(request.Object);

            //Assert
            result.Should().Be("response");
            internalApiClientMock.Verify(x => x.Get<string>(request.Object), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_GetResponseCode_Is_Delegated_To_InternalApiClient(
            [Frozen] Mock<IInternalApiClient<LearnerNotificationsApiConfiguration>> internalApiClientMock,
            LearnerNotificationsInnerApiClient sut)
        {
            //Arrange
            var request = new Mock<IGetApiRequest>();
            internalApiClientMock
                .Setup(x => x.GetResponseCode(It.IsAny<IGetApiRequest>()))
                .ReturnsAsync(HttpStatusCode.OK);

            //Act
            var result = await sut.GetResponseCode(request.Object);

            //Assert
            result.Should().Be(HttpStatusCode.OK);
            internalApiClientMock.Verify(x => x.GetResponseCode(request.Object), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_PutWithData_Is_Delegated_To_InternalApiClient(
            [Frozen] Mock<IInternalApiClient<LearnerNotificationsApiConfiguration>> internalApiClientMock,
            LearnerNotificationsInnerApiClient sut)
        {
            //Arrange
            var request = new Mock<IPutApiRequest<string>>();
            internalApiClientMock
                .Setup(x => x.Put(It.IsAny<IPutApiRequest<string>>()))
                .Returns(Task.CompletedTask);

            //Act
            await sut.Put(request.Object);

            //Assert
            internalApiClientMock.Verify(x => x.Put(request.Object), Times.Once);
        }

        [Test, MoqAutoData]
        public void Then_Unused_Methods_Throw_NotImplementedException(
            LearnerNotificationsInnerApiClient sut)
        {
            //Assert
            Assert.ThrowsAsync<NotImplementedException>(() => sut.GetWithResponseCode<string>(new Mock<IGetApiRequest>().Object));
            Assert.ThrowsAsync<NotImplementedException>(() => sut.GetAll<string>(new Mock<IGetAllApiRequest>().Object));
            Assert.ThrowsAsync<NotImplementedException>(() => sut.GetPaged<string>(new Mock<IGetPagedApiRequest>().Object));
            Assert.ThrowsAsync<NotImplementedException>(() => sut.Post<string>(new Mock<IPostApiRequest>().Object));
            Assert.ThrowsAsync<NotImplementedException>(() => sut.Post(new Mock<IPostApiRequest<string>>().Object));
            Assert.ThrowsAsync<NotImplementedException>(() => sut.PostWithResponseCode<string>(new Mock<IPostApiRequest>().Object));
            Assert.ThrowsAsync<NotImplementedException>(() => sut.Delete(new Mock<IDeleteApiRequest>().Object));
            Assert.ThrowsAsync<NotImplementedException>(() => sut.DeleteWithResponseCode<string>(new Mock<IDeleteApiRequest>().Object));
            Assert.ThrowsAsync<NotImplementedException>(() => sut.Patch(new Mock<IPatchApiRequest<string>>().Object));
            Assert.ThrowsAsync<NotImplementedException>(() => sut.PatchWithResponseCode(new Mock<IPatchApiRequest<string>>().Object));
            Assert.ThrowsAsync<NotImplementedException>(() => sut.PatchWithResponseCode<string, string>(new Mock<IPatchApiRequest<string>>().Object));
            Assert.ThrowsAsync<NotImplementedException>(() => sut.Put(new Mock<IPutApiRequest>().Object));
            Assert.ThrowsAsync<NotImplementedException>(() => sut.PutWithResponseCode<string>(new Mock<IPutApiRequest>().Object));
            Assert.ThrowsAsync<NotImplementedException>(() => sut.PutWithResponseCode<string, string>(new Mock<IPutApiRequest<string>>().Object));
        }
    }
}