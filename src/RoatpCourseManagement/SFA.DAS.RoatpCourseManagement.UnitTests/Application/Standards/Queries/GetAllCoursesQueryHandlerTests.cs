
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAllCoursesQuery;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Standards.Queries
{
    [TestFixture]
    public class GetAllCoursesQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_CallsInnerApi_ReturnsResults(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            List<GetAllCoursesResponse> courses,
            GetAllCoursesQuery query,
            GetAllCoursesQueryHandler sut)
        {
            apiClientMock.Setup(c => c.Get<List<GetAllCoursesResponse>>(It.IsAny<GetAllCoursesRequest>())).ReturnsAsync(courses);
            var result = await sut.Handle(query, new CancellationToken());
            
            result.Should().BeEquivalentTo(courses, options => 
               options.Excluding(c => c.IfateReferenceNumber)
                   .Excluding(c=>c.StandardInfoUrl)
                   .Excluding(c => c.ContactUsPhoneNumber)
                   .Excluding(c => c.ContactUsEmail)
                   .Excluding(c => c.ContactUsPageUrl)
                   .Excluding(c => c.IsConfirmed)
                   .Excluding(c => c.HasNationalDeliveryOption)
                   .Excluding(c => c.DeliveryModels)
                   .Excluding(c => c.HasHundredPercentEmployerDeliveryOption)
           );
        }

        [Test, MoqAutoData]
        public async Task Handle_CallsInnerApi_ReturnsEmptySetWithNoCourses(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetAllCoursesQuery query,
            GetAllCoursesQueryHandler sut)
        {
            var courses = new List<GetAllCoursesResponse>();
           apiClientMock.Setup(c => c.Get<List<GetAllCoursesResponse>>(It.IsAny<GetAllCoursesRequest>())).ReturnsAsync(courses);
            var result = await sut.Handle(query, new CancellationToken());
        
            result.Should().BeEmpty();
        }

        [Test, MoqAutoData]
        public async Task Handle_CallsInnerApi_ReturnsNullSetWithNullCourses(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetAllCoursesQuery query,
            GetAllCoursesQueryHandler sut)
        {
            apiClientMock.Setup(c => c.Get<List<GetAllCoursesResponse>>(It.IsAny<GetAllCoursesRequest>())).ReturnsAsync((List<GetAllCoursesResponse>)null);
            var result = await sut.Handle(query, new CancellationToken());

            result.Should().BeNull();
        }

        [Test, MoqAutoData]
        public void Handle_CallsInnerApi_ThrowsException(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            GetAllCoursesQuery query,
            GetAllCoursesQueryHandler sut)
        {
            apiClientMock.Setup(c => c.Get<List<GetAllCoursesResponse>>(It.IsAny<GetAllCoursesRequest>())).ThrowsAsync(new Exception());
            Assert.ThrowsAsync<Exception>(() => sut.Handle(query, new CancellationToken()));
        }
    }
}
