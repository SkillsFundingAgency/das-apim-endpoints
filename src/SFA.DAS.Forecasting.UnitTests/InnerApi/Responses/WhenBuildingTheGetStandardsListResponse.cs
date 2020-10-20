using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Forecasting.InnerApi.Responses;

namespace SFA.DAS.Forecasting.UnitTests.InnerApi.Responses
{
    public class WhenBuildingTheGetStandardsListResponse
    {
        [Test]
        public void Then_If_There_Is_No_Available_Funding_Zero_Is_Returned_For_FundingCap()
        {
            //Arrange/Act
            var standard =
                new GetStandardsListItem
                {
                    Title = "Available",
                    ApprenticeshipFunding = new List<GetStandardsListItem.FundingPeriod>()
                };

            //Assert
            standard.FundingCap.Should().Be(0);
        }
        [Test, AutoData]
        public void Then_The_ApprenticeshipFunding_Price_With_No_EffectiveTo_Date_And_Has_A_From_Date_In_The_Past_Is_Used(int fundingPrice)
        {
            //Arrange / Act
            var standard = new GetStandardsListItem
            {
                Title = "Available",
                ApprenticeshipFunding = new List<GetStandardsListItem.FundingPeriod>
                {
                    new GetStandardsListItem.FundingPeriod
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                        EffectiveTo = null,
                        MaxEmployerLevyCap = fundingPrice
                    }
                }
            };

            //Assert
            standard.FundingCap.Should().Be(fundingPrice);
        }

        [Test, AutoData]
        public void Then_The_Current_Funding_Price_Is_Used(int notFundingPrice, int fundingPrice)
        {
            //Arrange / Act
            var standard = new GetStandardsListItem
            {
                Title = "Available",
                ApprenticeshipFunding = new List<GetStandardsListItem.FundingPeriod>
                {
                    new GetStandardsListItem.FundingPeriod
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-10),
                        EffectiveTo = DateTime.UtcNow.AddDays(-9),
                        MaxEmployerLevyCap = notFundingPrice
                    },
                    new GetStandardsListItem.FundingPeriod
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                        EffectiveTo = null,
                        MaxEmployerLevyCap = fundingPrice
                    }
                }
            };

            //Assert
            standard.FundingCap.Should().Be(fundingPrice);
        }

        [Test, AutoData]
        public void Then_The_Future_Price_Is_Not_Used(int fundingPrice, int notFundingPrice)
        {
            //Arrange / Act
            var standard = new GetStandardsListItem
            {
                Title = "Available",
                ApprenticeshipFunding = new List<GetStandardsListItem.FundingPeriod>
                {
                    new GetStandardsListItem.FundingPeriod
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-10),
                        EffectiveTo = DateTime.UtcNow.AddDays(9),
                        MaxEmployerLevyCap = fundingPrice
                    },
                    new GetStandardsListItem.FundingPeriod
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(4),
                        EffectiveTo = null,
                        MaxEmployerLevyCap = notFundingPrice
                    }
                }
            };

            //Assert
            standard.FundingCap.Should().Be(fundingPrice);
        }
    }
}