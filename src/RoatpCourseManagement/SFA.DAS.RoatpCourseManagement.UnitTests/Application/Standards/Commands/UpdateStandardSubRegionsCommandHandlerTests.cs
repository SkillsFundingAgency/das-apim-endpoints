﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Commands.UpdateStandardSubRegions;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAllProviderLocations;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Commands
{
    [TestFixture]
    public class UpdateStandardSubRegionsCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_CallsApiClient(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            UpdateStandardSubRegionsCommandHandler sut,
            UpdateStandardSubRegionsCommand command,
            CancellationToken cancellationToken,
            List<ProviderLocationModel> apiResponse)
        {
            apiClientMock.Setup(a => a.Get<List<ProviderLocationModel>>(It.IsAny<GetAllProviderLocationsQuery>())).ReturnsAsync(apiResponse);
            apiClientMock.Setup(a => a.PostWithResponseCode<ProviderLocationsBulkInsertRequest>(It.IsAny<ProviderLocationsBulkInsertRequest>(), false)).ReturnsAsync(new ApiResponse<ProviderLocationsBulkInsertRequest>(null, System.Net.HttpStatusCode.OK, null));
            apiClientMock.Setup(a => a.PostWithResponseCode<ProviderCourseLocationBulkInsertRequest>(It.IsAny<ProviderCourseLocationBulkInsertRequest>(), false)).ReturnsAsync(new ApiResponse<ProviderCourseLocationBulkInsertRequest>(null, System.Net.HttpStatusCode.OK, null));

            var result = await sut.Handle(command, cancellationToken);

            apiClientMock.Verify(a => a.Get<List<ProviderLocationModel>>(It.IsAny<GetAllProviderLocationsQuery>()), Times.Once);
            apiClientMock.Verify(a => a.PostWithResponseCode<ProviderLocationsBulkInsertRequest>(It.IsAny<ProviderLocationsBulkInsertRequest>(), false), Times.Once);
            apiClientMock.Verify(a => a.Delete(It.IsAny<ProviderCourseLocationsBulkDeleteRequest>()), Times.Once);
            apiClientMock.Verify(a => a.PostWithResponseCode<ProviderCourseLocationBulkInsertRequest>(It.IsAny<ProviderCourseLocationBulkInsertRequest>(), false), Times.Once);
            apiClientMock.Verify(a => a.Delete(It.IsAny<ProviderLocationBulkDeleteRequest>()), Times.Once);

            Assert.That(result, Is.Not.Null);
        }

        [Test, MoqAutoData]
        public void Handle_CallsApiClient_ReturnException(
           [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
           UpdateStandardSubRegionsCommandHandler sut,
           UpdateStandardSubRegionsCommand command,
           CancellationToken cancellationToken,
           HttpRequestContentException expectedException)
        {
            apiClientMock.Setup(a => a.Get<List<ProviderLocationModel>>(It.IsAny<GetAllProviderLocationsQuery>())).Throws(expectedException);

            var actualException = Assert.ThrowsAsync<HttpRequestContentException>(() => sut.Handle(command, cancellationToken));

            actualException.Should().Be(expectedException);
        }
    }
}
