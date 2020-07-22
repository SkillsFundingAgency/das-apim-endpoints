using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetStandardsListRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(
            List<Guid> routeIds,
            List<int> levels,
            string keyword,
            string baseUrl)
        {
            var orderBy = OrderBy.Title;
            var actual = new GetStandardsListRequest
            {
                BaseUrl = baseUrl, 
                Keyword = keyword, 
                RouteIds = routeIds,
                Levels = levels,
                OrderBy = orderBy
            };

            actual.BaseUrl.Should().Be(baseUrl);
            actual.GetUrl.Should()
                .Be($"{baseUrl}api/courses/standards?keyword={keyword}&orderby={orderBy}&routeIds=" + string.Join("&routeIds=",routeIds) + "&levels=" + string.Join("&levels=", levels));
        }

        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_Without_RouteIds_And_Levels(
            string keyword,
            string baseUrl)
        {
            var actual = new GetStandardsListRequest
            {
                BaseUrl = baseUrl, 
                Keyword = keyword
            };

            actual.BaseUrl.Should().Be(baseUrl);
            actual.GetUrl.Should()
                .Be($"{baseUrl}api/courses/standards?keyword={keyword}");
        }

        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_Without_RouteIds(
            List<int> levels,
            string keyword,
            string baseUrl)
        {
            var actual = new GetStandardsListRequest
            {
                BaseUrl = baseUrl, 
                Keyword = keyword, 
                Levels = levels
            };
            
            actual.BaseUrl.Should().Be(baseUrl);
            actual.GetUrl.Should()
                .Be($"{baseUrl}api/courses/standards?keyword={keyword}&levels=" + string.Join("&levels=", levels));
        }
        
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_Without_Levels(
            List<Guid> routeIds,
            string keyword,
            string baseUrl)
        {
            var actual = new GetStandardsListRequest
            {
                BaseUrl = baseUrl, 
                Keyword = keyword, 
                RouteIds = routeIds
            };
            
            actual.BaseUrl.Should().Be(baseUrl);
            actual.GetUrl.Should()
                .Be($"{baseUrl}api/courses/standards?keyword={keyword}&routeIds=" + string.Join("&routeIds=", routeIds));
        }
    }
}
