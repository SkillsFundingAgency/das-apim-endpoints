using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetStandardsListRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(
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
                .Be($"{baseUrl}api/courses/standards?keyword={keyword}&routeIds=" + string.Join("&routeIds=",routeIds));
        }

        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_Without_RouteIds(
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
    }
}
