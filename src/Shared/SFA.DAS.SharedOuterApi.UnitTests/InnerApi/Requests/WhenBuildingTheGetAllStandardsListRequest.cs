using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingTheGetAllStandardsListRequest
{
    [Test, AutoData]
    public void GetUrl_DefaultParameters_ReturnsExpectedUrl()
    {
        var _sut = new GetActiveStandardsListRequest();

        Assert.That(_sut.GetUrl, Is.EqualTo("api/courses/standards?filter=Active&orderby=Score"));
    }

    [Test, AutoData]
    public void GetUrl_KeywordProvided_ReturnsExpectedUrl()
    {
        var _sut = new GetActiveStandardsListRequest
        {
            Keyword = "Construction"
        };

        Assert.That(_sut.GetUrl, Is.EqualTo("api/courses/standards?keyword=Construction&filter=Active&orderby=Score"));
    }

    [Test, AutoData]
    public void GetUrl_LevelsProvided_ReturnsExpectedUrl()
    {
        var _sut = new GetActiveStandardsListRequest
        {
            Levels = [1, 2]
        };

        Assert.That(_sut.GetUrl, Is.EqualTo("api/courses/standards?levels=1&levels=2&filter=Active&orderby=Score"));
    }

    [Test, AutoData]
    public void GetUrl_RouteIdsProvided_ReturnsExpectedUrl()
    {
        var _sut = new GetActiveStandardsListRequest
        {
            RouteIds = [1, 2]
        };

        Assert.That(_sut.GetUrl, Is.EqualTo("api/courses/standards?routeIds=1&routeIds=2&filter=Active&orderby=Score"));
    }

    [Test, AutoData]
    public void GetUrl_ApprenticeshipTypeProvided_ReturnsExpectedUrl(ApprenticeshipType type)
    {
        var _sut = new GetActiveStandardsListRequest
        {
            ApprenticeshipType = type.ToString()
        };

        Assert.That(_sut.GetUrl, Is.EqualTo($"api/courses/standards?apprenticeshipType={type.ToString()}&filter=Active&orderby=Score"));
    }
}