using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderEmailAddresses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ProviderRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetProviderEmails
    {
        [Test, MoqAutoData]
        public async Task Then_Get_ProviderEmails_From_The_Api(
            List<ProviderCourse> providerCourseResult,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpCourseManagementApiClient,
            GetProviderEmailAddressesQueryHandler handler,
            GetProviderEmailAddressesQuery query)
        {

            // Arrange
            mockRoatpCourseManagementApiClient.Setup(client => client.Get<List<ProviderCourse>>(It.IsAny<GetProviderCoursesRequest>()))
                    .ReturnsAsync(providerCourseResult);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.EmailAddresses.Should().Contain(query.UserEmailAddress);
            actual.EmailAddresses.Should().Contain(providerCourseResult.Select(x => x.ContactUsEmail));
        }

        [Test, MoqAutoData]
        public async Task AndNoProviderCoursesExist_Then_Get_ProviderEmails_ReturnsEmailFromCurrentLoggedInUser(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpCourseManagementApiClient,
            GetProviderEmailAddressesQueryHandler handler,
            GetProviderEmailAddressesQuery query)
        {
            //Arrange
            mockRoatpCourseManagementApiClient.Setup(client => client.Get<List<ProviderCourse>>(It.IsAny<GetProviderCoursesRequest>()))
                        .ReturnsAsync(new List<ProviderCourse>());

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.EmailAddresses.Should().HaveCount(1);
            actual.EmailAddresses.Should().Contain(query.UserEmailAddress);
        }
    }
}
