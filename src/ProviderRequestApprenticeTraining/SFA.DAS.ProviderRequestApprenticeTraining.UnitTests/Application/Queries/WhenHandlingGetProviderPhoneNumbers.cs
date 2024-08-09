using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderPhoneNumbers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetProviderPhoneNumbers
    {
        private readonly IProviderCoursesApiClient<ProviderCoursesApiConfiguration> _providerCoursesApiClient;
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient;

        [Test, MoqAutoData]
        public async Task Then_Get_ProviderPhoneNumbers_From_The_Api(
            List<ProviderCourse> providerCourseResult,
            GetProviderSummaryResponse providerSummaryResponse, 
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpCourseManagementApiClient,
            GetProviderPhoneNumbersQueryHandler handler,
            GetProviderPhoneNumbersQuery query)
        {

            // Arrange
            mockRoatpCourseManagementApiClient.Setup(client => client.Get<List<ProviderCourse>>(It.IsAny<GetProviderCoursesRequest>()))
                    .ReturnsAsync(providerCourseResult);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.PhoneNumbers.Should().Contain(providerCourseResult.Select(x => x.ContactUsPhoneNumber));

        }

        [Test, MoqAutoData]
        public async Task AndNoProviderCoursesExist_Then_Get_ProviderPhoneNumbers_ReturnsEmptyList(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpCourseManagementApiClient,
            GetProviderPhoneNumbersQueryHandler handler,
            GetProviderPhoneNumbersQuery query)
        {
            //Arrange
            mockRoatpCourseManagementApiClient.Setup(client => client.Get<List<ProviderCourse>>(It.IsAny<GetProviderCoursesRequest>()))
                        .ReturnsAsync(new List<ProviderCourse>());

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.PhoneNumbers.Should().HaveCount(0);
        }
    }
}
