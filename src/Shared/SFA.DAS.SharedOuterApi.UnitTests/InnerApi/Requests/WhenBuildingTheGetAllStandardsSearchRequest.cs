using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;

public class WhenBuildingTheGetAllStandardsSearchRequest
{
    [Test, AutoData]
    public void GetUrl_DefaultRequest_ReturnsActiveOrderedByScoreUrl()
    {
        var _sut = new GetActiveStandardsSearchRequest();

        Assert.That(_sut.GetUrl, Is.EqualTo("api/courses/search?filter=Active&orderby=Score"));
    }

    [Test, AutoData]
    public void GetUrl_KeywordProvided_IncludesKeywordQueryParameter()
    {
        var _sut = new GetActiveStandardsSearchRequest
        {
            Keyword = "Construction"
        };

        Assert.That(_sut.GetUrl, Is.EqualTo("api/courses/search?keyword=Construction&filter=Active&orderby=Score"));
    }

    [Test, AutoData]
    public void GetUrl_LevelsProvided_IncludesLevelsQueryParameters()
    {
        var _sut = new GetActiveStandardsSearchRequest
        {
            Levels = [1, 2]
        };

        Assert.That(_sut.GetUrl, Is.EqualTo("api/courses/search?levels=1&levels=2&filter=Active&orderby=Score"));
    }

    [Test, AutoData]
    public void GetUrl_RouteIdsProvided_IncludesRouteIdsQueryParameters()
    {
        var _sut = new GetActiveStandardsSearchRequest
        {
            RouteIds = [1, 2]
        };

        Assert.That(_sut.GetUrl, Is.EqualTo("api/courses/search?routeIds=1&routeIds=2&filter=Active&orderby=Score"));
    }

    [Test, AutoData]
    public void GetUrl_ApprenticeshipTypeProvided_IncludesLearningTypeQueryParameter(ApprenticeshipType type)
    {
        var _sut = new GetActiveStandardsSearchRequest
        {
            ApprenticeshipType = type.ToString()
        };

        Assert.That(_sut.GetUrl, Is.EqualTo($"api/courses/search?learningType={type.ToString()}&filter=Active&orderby=Score"));
    }
}