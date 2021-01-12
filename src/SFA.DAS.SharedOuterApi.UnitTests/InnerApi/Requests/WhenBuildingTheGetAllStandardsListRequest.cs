using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetAllStandardsListRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(
            List<Guid> routeIds,
            List<int> levels)
        {
            
            var actual = new GetAllStandardsListRequest
            {
                RouteIds = routeIds,
                Levels = levels
            };

            actual.GetUrl.Should()
                .Be($"api/courses/standards?filterAvailableToStart=false&routeIds=" + string.Join("&routeIds=",routeIds) + "&levels=" + string.Join("&levels=", levels));
        }
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_Without_RouteIds_And_Levels(
        )
        {
            var actual = new GetAllStandardsListRequest();

            actual.GetUrl.Should()
                .Be($"api/courses/standards?filterAvailableToStart=false");
        }

        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_Without_RouteIds(
            List<int> levels)
        {
            var actual = new GetAllStandardsListRequest
            {
                Levels = levels
            };
            
            actual.GetUrl.Should()
                .Be($"api/courses/standards?filterAvailableToStart=false&levels=" + string.Join("&levels=", levels));
        }
        
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed_Without_Levels(
            List<Guid> routeIds)
        {
            var actual = new GetAllStandardsListRequest
            { 
                RouteIds = routeIds
            };
            
            actual.GetUrl.Should()
                .Be($"api/courses/standards?filterAvailableToStart=false&routeIds=" + string.Join("&routeIds=", routeIds));
        }
    }
}