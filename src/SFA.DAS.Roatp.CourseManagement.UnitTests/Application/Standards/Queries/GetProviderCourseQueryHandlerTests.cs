
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetProviderCourse;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Roatp.CourseManagement.UnitTests.Application.Standards.Queries
{
    [TestFixture]
    public class GetProviderCourseQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_CallsInnerApi_ReturnsResults(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiClient,
            GetStandardResponse standard,
            GetProviderCourseResponse course,
            GetProviderCourseQuery query,
            GetProviderCourseQueryHandler sut)
        {
            coursesApiClient.Setup(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardRequest>())).ReturnsAsync(standard);

            courseManagementApiClient.Setup(c => c.Get<GetProviderCourseResponse>(It.IsAny<GetProviderCourseRequest>())).ReturnsAsync(course);

            var result = await sut.Handle(query, new CancellationToken());

            result.Should().NotBeNull();
  
        }

        [Test, MoqAutoData]
        public async Task Handle_CallsInnerApi_ReturnsEmptyData(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            GetProviderCourseQuery query,
            GetProviderCourseQueryHandler sut)
        {
            coursesApiClient.Setup(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardRequest>())).ReturnsAsync((GetStandardResponse)null);

            var result = await sut.Handle(query, new CancellationToken());
        
            result.Should().BeNull();
        }


        [Test, MoqAutoData]
        public async Task Handle_CallsInnerApi_ReturnsEmptyData(
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiClient,
            GetProviderCourseQuery query,
            GetProviderCourseQueryHandler sut)
        {
            coursesApiClient.Setup(c => c.Get<GetStandardResponse>(It.IsAny<GetStandardRequest>())).ReturnsAsync((GetStandardResponse)null);
            courseManagementApiClient.Setup(c => c.Get<GetProviderCourseResponse>(It.IsAny<GetProviderCourseRequest>())).ReturnsAsync((GetProviderCourseResponse)null);
            var result = await sut.Handle(query, new CancellationToken());

            result.Should().BeNull();
        }
    }
}
