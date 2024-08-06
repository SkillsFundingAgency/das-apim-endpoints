﻿using AutoFixture.NUnit3;
using Azure.Core;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderEmailAddresses;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetSelectEmployerRequests;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Requests;
using SFA.DAS.ProviderRequestApprenticeTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
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
    public class WhenHandlingGetProviderEmails
    {
        private readonly IProviderCoursesApiClient<ProviderCoursesApiConfiguration> _providerCoursesApiClient;
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient;

        [Test, MoqAutoData]
        public async Task Then_Get_ProviderEmails_From_The_Api(
            List<ProviderCourse> providerCourseResult,
            GetProviderSummaryResponse providerSummaryResponse, 
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> mockProviderCoursesApiClient,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpCourseManagementApiClient,
            GetProviderEmailAddressesQueryHandler handler,
            GetProviderEmailAddressesQuery query)
        {

            // Arrange
            mockProviderCoursesApiClient.Setup(client => client.Get<List<ProviderCourse>>(It.IsAny<GetProviderCoursesRequest>()))
                    .ReturnsAsync(providerCourseResult);

            mockRoatpCourseManagementApiClient.Setup(client => client.Get<GetProviderSummaryResponse>(It.IsAny<GetRoatpProviderRequest>()))
                    .ReturnsAsync(providerSummaryResponse);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.EmailAddresses.Should().Contain(providerSummaryResponse.Email);
            actual.EmailAddresses.Should().Contain(providerCourseResult.Select(x => x.ContactUsEmail));

        }

        [Test, MoqAutoData]
        public async Task AndNoProviderCoursesExist_Then_Get_ProviderEmails_ReturnsEmailFromProviderSummary(
            List<ProviderCourse> providerCourseResult,
            GetProviderSummaryResponse providerSummaryResponse,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> mockProviderCoursesApiClient,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpCourseManagementApiClient,
            GetProviderEmailAddressesQueryHandler handler,
            GetProviderEmailAddressesQuery query)
        {
            //Arrange
                mockProviderCoursesApiClient.Setup(client => client.Get<List<ProviderCourse>>(It.IsAny<GetProviderCoursesRequest>()))
                        .ReturnsAsync(new List<ProviderCourse>());

            mockRoatpCourseManagementApiClient.Setup(client => client.Get<GetProviderSummaryResponse>(It.IsAny<GetRoatpProviderRequest>()))
                    .ReturnsAsync(providerSummaryResponse);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.EmailAddresses.Should().HaveCount(1);
            actual.EmailAddresses.Should().Contain(providerSummaryResponse.Email);
        }
    }
}
