using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using SFA.DAS.EpaoRegister.Api.Infrastructure;

namespace SFA.DAS.EpaoRegister.Api.UnitTests.Models
{
    public static class MockUrlHelperExtensions
    {
        public static void SetupAllEpaoRoutes(this Mock<IUrlHelper> mockUrlHelper, string expectedGetEpaosUrl, string expectedGetEpaoUrl, string expectedGetEpaoCoursesUrl)
        {
            mockUrlHelper
                .Setup(helper => helper.RouteUrl(
                    It.Is<UrlRouteContext>(context => context.RouteName == RouteNames.GetEpaos)))
                .Returns(expectedGetEpaosUrl);
            mockUrlHelper
                .Setup(helper => helper.RouteUrl(
                    It.Is<UrlRouteContext>(context => context.RouteName == RouteNames.GetEpao)))
                .Returns(expectedGetEpaoUrl);
            mockUrlHelper
                .Setup(helper => helper.RouteUrl(
                    It.Is<UrlRouteContext>(context => context.RouteName == RouteNames.GetEpaoCourses)))
                .Returns(expectedGetEpaoCoursesUrl);
        }
    }
}