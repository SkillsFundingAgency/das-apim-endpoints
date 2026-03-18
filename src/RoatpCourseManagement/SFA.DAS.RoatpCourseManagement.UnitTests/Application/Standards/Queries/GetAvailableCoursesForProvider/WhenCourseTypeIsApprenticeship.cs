using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Standards.Queries.GetAvailableCoursesForProvider;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Standards.Queries.GetAvailableCoursesForProvider;

[TestFixture]
public class WhenCourseTypeIsApprenticeship
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
    public async Task Handle_EmptyProviderCourses_ReturnsAllStandards(GetAllStandardsResponse getAllStandardsResponse, int ukprn)
    {
        GetAvailableCoursesForProviderQuery request = new(ukprn, CourseType.Apprenticeship);
        _apiClientMock.Setup(a => a.Get<GetAllStandardsResponse>(It.IsAny<GetAllCoursesRequest>())).ReturnsAsync(getAllStandardsResponse);
        _apiClientMock.Setup(a => a.Get<List<GetAllProviderCoursesResponse>>(It.IsAny<GetAllProviderCoursesRequest>())).ReturnsAsync(new List<GetAllProviderCoursesResponse>());

        var result = await _sut.Handle(request, new CancellationToken());

        result.AvailableCourses.Should().HaveCount(getAllStandardsResponse.Standards.Count);
    }

    [Test, AutoData]
    public async Task Handle_HasProviderCourses_ReturnsFilteredList(GetAllStandardsResponse getAllStandardsResponse, int ukprn)
    {
        GetAvailableCoursesForProviderQuery request = new(ukprn, CourseType.Apprenticeship);
        var larsCode = getAllStandardsResponse.Standards[0].LarsCode;
        _apiClientMock.Setup(a => a.Get<GetAllStandardsResponse>(It.IsAny<GetAllCoursesRequest>())).ReturnsAsync(getAllStandardsResponse);
        _apiClientMock.Setup(a => a.Get<List<GetAllProviderCoursesResponse>>(It.IsAny<GetAllProviderCoursesRequest>())).ReturnsAsync(new List<GetAllProviderCoursesResponse>() { new GetAllProviderCoursesResponse { LarsCode = larsCode.ToString() } });

        var result = await _sut.Handle(request, new CancellationToken());

        result.AvailableCourses.Should().HaveCount(getAllStandardsResponse.Standards.Count - 1);
        result.AvailableCourses.Any(c => c.LarsCode == larsCode.ToString()).Should().BeFalse();
    }

    [Test, AutoData]
    public async Task Handle_IfCourseTypeIsShortCourse_FilteredList(GetAllStandardsResponse getAllStandardsResponse, GetAvailableCoursesForProviderQuery request)
    {
        var larsCode = getAllStandardsResponse.Standards.First().LarsCode;
        _apiClientMock.Setup(a => a.Get<GetAllStandardsResponse>(It.IsAny<GetAllCoursesRequest>())).ReturnsAsync(getAllStandardsResponse);
        _apiClientMock.Setup(a => a.Get<List<GetAllProviderCoursesResponse>>(It.IsAny<GetAllProviderCoursesRequest>())).ReturnsAsync(new List<GetAllProviderCoursesResponse>() { new GetAllProviderCoursesResponse { LarsCode = larsCode.ToString() } });

        var result = await _sut.Handle(request, new CancellationToken());

        result.AvailableCourses.Should().HaveCount(getAllStandardsResponse.Standards.Count - 1);
        result.AvailableCourses.Any(c => c.LarsCode == larsCode.ToString()).Should().BeFalse();
    }
}

public class WhenCourseTypeIsShortCourse
{
    GetAvailableCoursesForProviderQueryResult _result;
    List<GetStandardResponse> _standards;

    [SetUp]
    public async Task Before_Each_Test()
    {
        Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock = new();

        Fixture fixture = new();
        _standards = fixture.Build<GetStandardResponse>().CreateMany(5).ToList();
        GetAllStandardsResponse getAllStandardsResponse = new() { Standards = _standards };
        apiClientMock.Setup(a => a.Get<GetAllStandardsResponse>(It.IsAny<GetAllCoursesRequest>())).ReturnsAsync(getAllStandardsResponse);

        var providerCourse = fixture.Build<GetAllProviderCoursesResponse>().With(p => p.LarsCode, _standards[0].LarsCode).Create();
        apiClientMock.Setup(a => a.Get<List<GetAllProviderCoursesResponse>>(It.IsAny<GetAllProviderCoursesRequest>())).ReturnsAsync([providerCourse]);

        IEnumerable<ProviderAllowedCourseModel> allowedCourses = _standards[1..4].Select(s => fixture.Build<ProviderAllowedCourseModel>().With(r => r.LarsCode, s.LarsCode).Create());

        apiClientMock.Setup(a => a.Get<GetAllowedCoursesForProviderResponse>(It.IsAny<GetAllowedCoursesForProviderRequest>())).ReturnsAsync(new GetAllowedCoursesForProviderResponse(allowedCourses));

        GetAvailableCoursesForProviderQueryHandler _sut = new(Mock.Of<ILogger<GetAvailableCoursesForProviderQueryHandler>>(), apiClientMock.Object);

        _result = await _sut.Handle(new GetAvailableCoursesForProviderQuery(12345678, CourseType.ShortCourse), new CancellationToken());
    }

    [Test]
    public void Then_Only_Courses_In_Allowed_Courses_Returned()
    {
        _result.AvailableCourses.Should().HaveCount(3);
        _result.AvailableCourses.Select(s => s.LarsCode).Should().BeEquivalentTo(_standards[1..4].Select(s => s.LarsCode));
    }

    [Test]
    public void Then_Courses_Already_Added_Are_Filtered_Out()
    {
        _result.AvailableCourses.Select(s => s.LarsCode).Should().NotContain(_standards[0].LarsCode);
    }

    [Test]
    public void Then_Courses_Not_Allowed_Are_Filtered_Out()
    {
        _result.AvailableCourses.Select(s => s.LarsCode).Should().NotContain(_standards[4].LarsCode);
    }
}
