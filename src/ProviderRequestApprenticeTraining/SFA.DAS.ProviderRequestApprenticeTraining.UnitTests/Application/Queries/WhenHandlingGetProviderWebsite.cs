using AutoFixture.NUnit3;
using Azure.Core;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderEmailAddresses;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderWebsite;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Testing.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static NUnit.Framework.Constraints.Tolerance;

namespace SFA.DAS.ProviderRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetProviderWebsite
    {
        [Test, MoqAutoData]
        public async Task Then_Get_ProviderWebsite_From_The_Api(
            GetProviderSummaryResponse providerSummary,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpCourseManagementApiClient,
            GetProviderWebsiteQueryHandler handler,
            GetProviderWebsiteQuery query)
        {

            // Arrange
            mockRoatpCourseManagementApiClient.Setup(client => client.Get<GetProviderSummaryResponse>(It.IsAny<GetRoatpProviderRequest>()))
                    .ReturnsAsync(providerSummary);
            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.Website.Should().Be(providerSummary.ContactUrl);

        }
    }
}
