﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Forecasting.Application.Courses.Queries.GetFrameworkCoursesList;
using SFA.DAS.Forecasting.InnerApi.Requests;
using SFA.DAS.Forecasting.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Forecasting.UnitTests.Application.Courses.Queries;

public class WhenGettingFrameworks
{
    [Test, MoqAutoData]
    public void Then_Gets_All_Active_Frameworks_With_Funding_Periods_From_Courses_Api(
        GetFrameworkCoursesQuery query,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
        GetFrameworkCoursesQueryHandler handler)
    {
        //Arrange
        var fundingPeriods = new List<GetFrameworksListItem.FundingPeriod>
        {
            new() {EffectiveFrom = DateTime.Today.AddDays(-1), EffectiveTo = DateTime.Today.AddDays(1), FundingCap = 10}
        };
        
        var frameworksList = new GetFrameworksListResponse
        {
            Frameworks = new List<GetFrameworksListItem>
            {
                new() {Id = "1", IsActiveFramework = true, FundingPeriods = fundingPeriods},
                new() {Id = "2", IsActiveFramework = false, FundingPeriods = fundingPeriods},
                new() {Id = "3", IsActiveFramework = true, FundingPeriods = fundingPeriods},
                new() {Id = "4", IsActiveFramework = true, FundingPeriods = [] }
            }
        };
        mockCoursesApiClient.Setup(client =>
                client.Get<GetFrameworksListResponse>(It.IsAny<GetFrameworksRequest>()))
            .ReturnsAsync(frameworksList);

        //Act
        var actual = handler.Handle(query, CancellationToken.None);

        //Assert
        Assert.That(2, Is.EqualTo(actual.Result.Frameworks.Count()));
        Assert.That("1", Is.EquivalentTo(actual.Result.Frameworks.First().Id));
        Assert.That("3", Is.EquivalentTo(actual.Result.Frameworks.Last().Id));
    }
}