using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Commands.CreateProviderCourse;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Commands
{
    [TestFixture]
    public class CreateProviderCourseCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_InvokesApiCall(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            CreateProviderCourseCommandHandler sut,
            CreateProviderCourseCommand command)
        {
            var response = new ApiResponse<int>(1, HttpStatusCode.Created, string.Empty);
            apiClientMock.Setup(c => c.PostWithResponseCode<int>(It.Is<ProviderCourseCreateRequest>(r => r.Ukprn == command.Ukprn && r.Data == command), true)).ReturnsAsync(response);

            await sut.Handle(command, new CancellationToken());
            apiClientMock.Verify(c => c.PostWithResponseCode<int>(It.Is<ProviderCourseCreateRequest>(r => r.Ukprn == command.Ukprn && r.Data == command), true));
        }

        [Test, MoqAutoData]
        public async Task Handle_ResponseNotSuccessful_ThrowsException(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            CreateProviderCourseCommandHandler sut,
            CreateProviderCourseCommand command)
        {
            var response = new ApiResponse<int>(0, HttpStatusCode.InternalServerError, string.Empty);
            apiClientMock.Setup(c => c.PostWithResponseCode<int>(It.Is<ProviderCourseCreateRequest>(r => r.Ukprn == command.Ukprn && r.Data == command), true)).ReturnsAsync(response);

            Func<Task> action = () => sut.Handle(command, new CancellationToken());

            await action.Should().ThrowAsync<InvalidOperationException>();
            apiClientMock.Verify(c => c.PostWithResponseCode<int>(It.Is<ProviderCourseCreateRequest>(r => r.Ukprn == command.Ukprn && r.Data == command), true));
        }
    }
}
