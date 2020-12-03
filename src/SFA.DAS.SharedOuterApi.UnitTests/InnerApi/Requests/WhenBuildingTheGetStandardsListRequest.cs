using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetStandardsListRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(
            List<Guid> routeIds,
            List<int> levels,
            string keyword)
        {
            var orderBy = CoursesOrderBy.Title;
            var actual = new GetStandardsListRequest
            {
                Keyword = keyword, 
                RouteIds = routeIds,
                Levels = levels,
                OrderBy = orderBy
            };

            actual.GetUrl.Should()
                .Be($"api/courses/standards?keyword={keyword}&orderby={orderBy}&routeIds=" + string.Join("&routeIds=",routeIds) + "&levels=" + string.Join("&levels=", levels));
        }

        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_Without_RouteIds_And_Levels(
            string keyword)
        {
            var orderBy = CoursesOrderBy.Score;
            var actual = new GetStandardsListRequest
            { 
                Keyword = keyword,
                OrderBy = orderBy
            };

            actual.GetUrl.Should()
                .Be($"api/courses/standards?keyword={keyword}&orderby={orderBy}");
        }

        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_Without_RouteIds(
            List<int> levels,
            string keyword)
        {
            var orderBy = CoursesOrderBy.Score;
            var actual = new GetStandardsListRequest
            {
                Keyword = keyword, 
                Levels = levels,
                OrderBy = orderBy
            };
            
            actual.GetUrl.Should()
                .Be($"api/courses/standards?keyword={keyword}&orderby={orderBy}&levels=" + string.Join("&levels=", levels));
        }
        
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_Without_Levels(
            List<Guid> routeIds,
            string keyword)
        {
            var orderBy = CoursesOrderBy.Score;
            var actual = new GetStandardsListRequest
            { 
                Keyword = keyword, 
                RouteIds = routeIds,
                OrderBy = orderBy
            };
            
            actual.GetUrl.Should()
                .Be($"api/courses/standards?keyword={keyword}&orderby={orderBy}&routeIds=" + string.Join("&routeIds=", routeIds));
        }
    }
}
