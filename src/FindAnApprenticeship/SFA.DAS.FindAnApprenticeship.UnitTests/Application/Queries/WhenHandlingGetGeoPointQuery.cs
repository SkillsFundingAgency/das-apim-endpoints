﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetGeoPoint;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries
{
    public class WhenHandlingGetGeoPointQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Calls_Location_Service_To_Get_GeoPoint(
            GetGeoPointQuery query,
            GetGeoPointResponse apiResponse,
            [Frozen] Mock<ILocationLookupService> mockLocationLookupService,
            GetGeoPointQueryHandler handler)
        {
            var result = await handler.Handle(query, CancellationToken.None);
            mockLocationLookupService.Verify(p => p.GetLocationInformation(query.Postcode, default(double), default(double), false), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task When_Location_Is_Not_Found_Then_Return_Empty_Geopoint(
           GetGeoPointQuery query,
           GetGeoPointResponse apiResponse,
           [Frozen] Mock<ILocationLookupService> mockLocationLookupService,
           GetGeoPointQueryHandler handler)
        {
            mockLocationLookupService.Setup(m => m.GetLocationInformation(query.Postcode, default(double), default(double), false)).ReturnsAsync((LocationItem)null);

            var result = await handler.Handle(query, CancellationToken.None);

            result.GetPointResponse.Should().BeNull();
        }
    }
}