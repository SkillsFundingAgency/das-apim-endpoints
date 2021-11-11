using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Responses.StandardApiResponseBaseTests
{
    public class WhenGettingMaxFunding
    {
        [Test]
        public void Then_If_There_Is_No_Available_Funding_Zero_Is_Returned_For_MaxFunding()
        {
            //Arrange/Act
            var standard = 
                    new TestStandardResponse
                    {
                        ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    };
            
            //Assert
            standard.MaxFunding.Should().Be(0);
        }

        [Test]
        public void Then_If_There_Is_Null_Funding_Zero_Is_Returned()
        {
            //Arrange/Act
            var standard = 
                new TestStandardResponse
                {
                    ApprenticeshipFunding = null
                };
            
            //Assert
            standard.MaxFunding.Should().Be(0);
        }

        [Test, AutoData]
        public void Then_The_ApprenticeshipFunding_Price_With_No_EffectiveTo_Date_And_Has_A_From_Date_In_The_Past_Is_Used(int fundingPrice)
        {
            //Arrange / Act
            var standard = new TestStandardResponse
            {
                ApprenticeshipFunding = new List<ApprenticeshipFunding>
                {
                    new ApprenticeshipFunding
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                        EffectiveTo = null,
                        MaxEmployerLevyCap = fundingPrice
                    }
                }
            };
            
            //Assert
            standard.MaxFunding.Should().Be(fundingPrice);
        }

        [Test, AutoData]
        public void Then_The_Current_Funding_Price_Is_Used(int notFundingPrice, int fundingPrice)
        {
            //Arrange / Act
            var standard = new TestStandardResponse
            {
                ApprenticeshipFunding = new List<ApprenticeshipFunding>
                {
                    new ApprenticeshipFunding
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-10),
                        EffectiveTo = DateTime.UtcNow.AddDays(-9),
                        MaxEmployerLevyCap = notFundingPrice
                    },
                    new ApprenticeshipFunding
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                        EffectiveTo = null,
                        MaxEmployerLevyCap = fundingPrice
                    }
                }
            };
            
            //Assert
            standard.MaxFunding.Should().Be(fundingPrice);
        }

        

        [Test, AutoData]
        public void Then_The_Future_Price_Is_Not_Used(int fundingPrice, int notFundingPrice)
        {
            //Arrange / Act
            var standard = new TestStandardResponse
            {
                ApprenticeshipFunding = new List<ApprenticeshipFunding>
                {
                    new ApprenticeshipFunding
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-10),
                        EffectiveTo = DateTime.UtcNow.AddDays(9),
                        MaxEmployerLevyCap = fundingPrice
                    },
                    new ApprenticeshipFunding
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(4),
                        EffectiveTo = null,
                        MaxEmployerLevyCap = notFundingPrice
                    }
                }
            };
            
            //Assert
            standard.MaxFunding.Should().Be(fundingPrice);
        }


        [Test, AutoData]
        public void Then_If_An_Effective_Date_Is_Provided_The_Funding_Price_As_At_That_Date_Is_Used(int notFundingPrice, int fundingPrice)
        {
            var effectiveDate = DateTime.UtcNow;

            //Arrange / Act
            var standard = new TestStandardResponse
            {
                ApprenticeshipFunding = new List<ApprenticeshipFunding>
                {
                    new ApprenticeshipFunding
                    {
                        EffectiveFrom = effectiveDate.AddDays(-10),
                        EffectiveTo = effectiveDate.AddDays(-9),
                        MaxEmployerLevyCap = notFundingPrice
                    },
                    new ApprenticeshipFunding
                    {
                        EffectiveFrom = effectiveDate.AddDays(-1),
                        EffectiveTo = null,
                        MaxEmployerLevyCap = fundingPrice
                    }
                }
            };

            //Assert
            standard.MaxFundingOn(effectiveDate).Should().Be(fundingPrice);
        }
    }
}