using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderRequestApprenticeTraining.Application.Queries.GetProviderPhoneNumbers;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderCoursesService;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderRequestApprenticeTraining.UnitTests.Application.Queries
{
    public class WhenHandlingGetProviderPhoneNumbers
    {
        [Test, TestCaseSource(nameof(PhoneNumberTestCases))]
        public async Task Then_Get_ProviderPhoneNumbers_From_The_Api(ProviderPhoneNumbersTestCase data)
        {
            // Arrange
            var providerCourses = data.PhoneNumbers
                .Select(phone => new ProviderCourse { ContactUsPhoneNumber = phone })
                .ToList();

            var mockRoatpCourseManagementApiClient = new Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>>();
            mockRoatpCourseManagementApiClient
                .Setup(client => client.Get<List<ProviderCourse>>(It.IsAny<GetProviderCoursesRequest>()))
                .ReturnsAsync(providerCourses);

            var sut = new GetProviderPhoneNumbersQueryHandler(mockRoatpCourseManagementApiClient.Object, 
                new Mock<ILogger<GetProviderPhoneNumbersQueryHandler>>().Object);

            // Act
            var actual = await sut.Handle(new GetProviderPhoneNumbersQuery(), CancellationToken.None);

            // Assert
            actual.PhoneNumbers.Should().BeEquivalentTo(data.UniquePhoneNumbers);
        }

        public static IEnumerable<ProviderPhoneNumbersTestCase> PhoneNumberTestCases()
        {
            yield return new ProviderPhoneNumbersTestCase { PhoneNumbers = ["1 11", "11 1", "222"], UniquePhoneNumbers = ["1 11", "222"] };
            yield return new ProviderPhoneNumbersTestCase { PhoneNumbers = ["33 3", "3 33", "444"], UniquePhoneNumbers = ["33 3", "444"] };
            yield return new ProviderPhoneNumbersTestCase { PhoneNumbers = ["5 55 555", "55 5 555", "555 55 5"], UniquePhoneNumbers = ["5 55 555"] };
            yield return new ProviderPhoneNumbersTestCase { PhoneNumbers = ["666 66 6", "6 66 666", "7 77 777", "777 77 7"], UniquePhoneNumbers = ["666 66 6", "7 77 777"] };
        }

        public class ProviderPhoneNumbersTestCase
        {
            public string[] PhoneNumbers { get; set; }
            public string[] UniquePhoneNumbers { get; set; }
        }

        [Test, MoqAutoData]
        public async Task AndNoProviderCoursesExist_Then_Get_ProviderPhoneNumbers_ReturnsEmptyList(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpCourseManagementApiClient,
            GetProviderPhoneNumbersQueryHandler handler,
            GetProviderPhoneNumbersQuery query)
        {
            // Arrange
            mockRoatpCourseManagementApiClient
                .Setup(client => client.Get<List<ProviderCourse>>(It.IsAny<GetProviderCoursesRequest>()))
                .ReturnsAsync(new List<ProviderCourse>());

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.PhoneNumbers.Should().HaveCount(0);
        }

        [Test, MoqAutoData]
        public async Task AndProviderCoursesCannotBeAccessed_Then_Get_ProviderPhoneNumbers_ReturnsEmptyList(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpCourseManagementApiClient,
            GetProviderPhoneNumbersQueryHandler handler,
            GetProviderPhoneNumbersQuery query)
        {
            // Arrange
            mockRoatpCourseManagementApiClient
                .Setup(client => client.Get<List<ProviderCourse>>(It.IsAny<GetProviderCoursesRequest>()))
                .ReturnsAsync((List<ProviderCourse>)null);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.PhoneNumbers.Should().HaveCount(0);
        }
    }
}
