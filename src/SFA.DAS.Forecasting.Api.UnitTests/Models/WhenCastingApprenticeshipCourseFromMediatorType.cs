using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Forecasting.Api.Models;
using SFA.DAS.Forecasting.InnerApi.Responses;

namespace SFA.DAS.Forecasting.Api.UnitTests.Models
{
    public class WhenCastingApprenticeshipCourseFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_GetStandardsListItem_Correctly(
            GetStandardsListItem source)
        {
            var response = (ApprenticeshipCourse)source;

            response.Should().BeEquivalentTo(source,
                o => o.Excluding(s => s.ApprenticeshipFunding));
            
            for (int i = 0; i < response.FundingPeriods.Count; i++)
            {
                response.FundingPeriods[i].FundingCap.Should().Be(source.ApprenticeshipFunding[i].MaxEmployerLevyCap);
                response.FundingPeriods[i].EffectiveFrom.Should().Be(source.ApprenticeshipFunding[i].EffectiveFrom);
                response.FundingPeriods[i].EffectiveTo.Should().Be(source.ApprenticeshipFunding[i].EffectiveTo);
            }
        }

        [Test, AutoData]
        public void And_No_Standard_Funding_Periods_Then_Duration_Is_Zero(
            GetStandardsListItem source)
        {
            source.ApprenticeshipFunding = new List<GetStandardsListItem.FundingPeriod>();

            var response = (ApprenticeshipCourse)source;

            response.Duration.Should().Be(0);
        }

        [Test, AutoData]
        public void Then_Maps_GetFrameworksListItem_Correctly(
            GetFrameworksListItem source)
        {
            var response = (ApprenticeshipCourse)source;

            response.Should().BeEquivalentTo(source,
                o => o.Excluding(f => f.IsActiveFramework)
                    .Excluding(f => f.CurrentFundingCap));
            response.FundingCap.Should().Be(source.CurrentFundingCap);
        }
    }
}