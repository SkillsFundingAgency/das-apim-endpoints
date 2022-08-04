using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCoursesForProvider;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Queries.GetAvailableCoursesForProvider
{
    [TestFixture]
    public class GetAvailableCoursesForProviderQueryHandlerTests
    {
        private GetAvailableCoursesForProviderQueryHandler _sut;
        private Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> _apiClientMock;

        [SetUp]
        public void Before_Each_Test()
        {
            _apiClientMock = new Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>>();
            _sut = new GetAvailableCoursesForProviderQueryHandler(Mock.Of<ILogger<GetAvailableCoursesForProviderQueryHandler>>(), _apiClientMock.Object);
        }


        [Test, AutoData]
        public async Task Handle_EmptyProviderCourses_ReturnsAllStandards(GetAllStandardsResponse getAllStandardsResponse, GetAvailableCoursesForProviderQuery request)
        {
            _apiClientMock.Setup(a => a.Get<GetAllStandardsResponse>(It.IsAny<GetAllStandardsRequest>())).ReturnsAsync(getAllStandardsResponse);
            _apiClientMock.Setup(a => a.Get<List<GetAllProviderCoursesResponse>>(It.IsAny<GetAllCoursesRequest>())).ReturnsAsync(new List<GetAllProviderCoursesResponse>());

            var result = await _sut.Handle(request, new CancellationToken());

            result.AvailableCourses.Count.Should().Be(getAllStandardsResponse.Standards.Count);
        }

        [Test, AutoData]
        public async Task Handle_HasProviderCourses_ReturnsFilteredList(GetAllStandardsResponse getAllStandardsResponse, GetAvailableCoursesForProviderQuery request)
        {
            var larsCode = getAllStandardsResponse.Standards.First().LarsCode;
            _apiClientMock.Setup(a => a.Get<GetAllStandardsResponse>(It.IsAny<GetAllStandardsRequest>())).ReturnsAsync(getAllStandardsResponse);
            _apiClientMock.Setup(a => a.Get<List<GetAllProviderCoursesResponse>>(It.IsAny<GetAllCoursesRequest>())).ReturnsAsync(new List<GetAllProviderCoursesResponse>() { new GetAllProviderCoursesResponse {  LarsCode = larsCode } });

            var result = await _sut.Handle(request, new CancellationToken());

            result.AvailableCourses.Count.Should().Be(getAllStandardsResponse.Standards.Count - 1);
            result.AvailableCourses.Any(c => c.LarsCode == larsCode).Should().BeFalse();
        }
    }
}
