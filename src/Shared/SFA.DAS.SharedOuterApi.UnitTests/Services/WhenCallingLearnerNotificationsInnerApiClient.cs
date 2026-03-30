using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
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
        public async Task Then_Put_Is_Delegated_To_InternalApiClient(
            [Frozen] Mock<IInternalApiClient<LearnerNotificationsApiConfiguration>> internalApiClientMock,
            LearnerNotificationsInnerApiClient sut)
        {
            //Arrange
            var request = new Mock<IPutApiRequest>();
            internalApiClientMock
                .Setup(x => x.Put(It.IsAny<IPutApiRequest>()))
                .Returns(Task.CompletedTask);

            //Act
            await sut.Put(request.Object);

            //Assert
            internalApiClientMock.Verify(x => x.Put(request.Object), Times.Once);
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
        public async Task Then_Delete_Is_Delegated_To_InternalApiClient(
            [Frozen] Mock<IInternalApiClient<LearnerNotificationsApiConfiguration>> internalApiClientMock,
            LearnerNotificationsInnerApiClient sut)
        {
            //Arrange
            var request = new Mock<IDeleteApiRequest>();
            internalApiClientMock
                .Setup(x => x.Delete(It.IsAny<IDeleteApiRequest>()))
                .Returns(Task.CompletedTask);

            //Act
            await sut.Delete(request.Object);

            //Assert
            internalApiClientMock.Verify(x => x.Delete(request.Object), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Patch_Is_Delegated_To_InternalApiClient(
            [Frozen] Mock<IInternalApiClient<LearnerNotificationsApiConfiguration>> internalApiClientMock,
            LearnerNotificationsInnerApiClient sut)
        {
            //Arrange
            var request = new Mock<IPatchApiRequest<string>>();
            internalApiClientMock
                .Setup(x => x.Patch(It.IsAny<IPatchApiRequest<string>>()))
                .Returns(Task.CompletedTask);

            //Act
            await sut.Patch(request.Object);

            //Assert
            internalApiClientMock.Verify(x => x.Patch(request.Object), Times.Once);
        }

        [Test, MoqAutoData]
        public void Then_Post_Throws_NotImplementedException(
            LearnerNotificationsInnerApiClient sut)
        {
            //Arrange
            var request = new Mock<IPostApiRequest>();

            //Act & Assert
            Assert.ThrowsAsync<NotImplementedException>(() => sut.Post<string>(request.Object));
        }

        [Test, MoqAutoData]
        public void Then_PostWithData_Throws_NotImplementedException(
            LearnerNotificationsInnerApiClient sut)
        {
            //Arrange
            var request = new Mock<IPostApiRequest<string>>();

            //Act & Assert
            Assert.ThrowsAsync<NotImplementedException>(() => sut.Post(request.Object));
        }

        [Test, MoqAutoData]
        public async Task Then_PostWithResponseCode_Is_Delegated_To_InternalApiClient(
            [Frozen] Mock<IInternalApiClient<LearnerNotificationsApiConfiguration>> internalApiClientMock,
            LearnerNotificationsInnerApiClient sut)
        {
            //Arrange
            var request = new Mock<IPostApiRequest>();
            var expectedResponse = new ApiResponse<string>("response", HttpStatusCode.OK, null);
            internalApiClientMock
                .Setup(x => x.PostWithResponseCode<string>(It.IsAny<IPostApiRequest>(), true))
                .ReturnsAsync(expectedResponse);

            //Act
            var result = await sut.PostWithResponseCode<string>(request.Object, true);

            //Assert
            result.Should().BeSameAs(expectedResponse);
            internalApiClientMock.Verify(x => x.PostWithResponseCode<string>(request.Object, true), Times.Once);
        }
    }
}