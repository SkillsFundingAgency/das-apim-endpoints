using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Responses
{
    public class WhenBuildingTheGetStandardsListItemResponse
    {
        
        [Test]
        public void Then_If_There_Is_No_Available_Funding_Zero_Is_Returned_For_MaxFunding()
        {
            //Arrange/Act
            var standard = 
                    new GetStandardsListItem
                    {
                        Title = "Available",
                        ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                        StandardDates = 
                            new StandardDate
                            {
                                EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                                LastDateStarts = null
                            }
                        
                    };
            
            //Assert
            standard.MaxFunding.Should().Be(0);
        }

        [Test]
        public void Then_If_There_Is_No_Available_Funding_Zero_Is_Returned_For_TypicalDuration()
        {
            //Arrange/Act
            var standard =
                new GetStandardsListItem
                {
                    Title = "Available",
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                    StandardDates =
                        new StandardDate
                        {
                            EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                            LastDateStarts = null
                        }

                };

            //Assert
            standard.TypicalDuration.Should().Be(0);
        }

        [Test, AutoData]
        public void Then_The_ApprenticeshipFunding_Price_With_No_EffectiveTo_Date_And_Has_A_From_Date_In_The_Past_Is_Used(long fundingPrice)
        {
            //Arrange / Act
            var standard = new GetStandardsListItem
            {
                Title = "Available",
                ApprenticeshipFunding = new List<ApprenticeshipFunding>
                {
                    new ApprenticeshipFunding
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                        EffectiveTo = null,
                        MaxEmployerLevyCap = fundingPrice
                    }
                },
                StandardDates = 
                    new StandardDate
                    {
                        EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                        LastDateStarts = null
                    }
                
            };
            
            //Assert
            standard.MaxFunding.Should().Be(fundingPrice);
        }

        [Test, AutoData]
        public void Then_The_Current_Funding_Price_Is_Used(long notFundingPrice, long fundingPrice)
        {
            //Arrange / Act
            var standard = new GetStandardsListItem
            {
                Title = "Available",
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
                },
                StandardDates = 
                    new StandardDate
                    {
                        EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                        LastDateStarts = null
                    }
                
            };
            
            //Assert
            standard.MaxFunding.Should().Be(fundingPrice);
        }

        [Test, AutoData]
        public void Then_The_Typical_Duration_Is_Used(int notDuration, int duration)
        {
            //Arrange / Act
            var standard = new GetStandardsListItem
            {
                Title = "Available",
                ApprenticeshipFunding = new List<ApprenticeshipFunding>
                {
                    new ApprenticeshipFunding
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-10),
                        EffectiveTo = DateTime.UtcNow.AddDays(-9),
                        Duration = notDuration
                    },
                    new ApprenticeshipFunding
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                        EffectiveTo = null,
                        Duration = duration
                    }
                },
                StandardDates =
                    new StandardDate
                    {
                        EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                        LastDateStarts = null
                    }

            };

            //Assert
            standard.TypicalDuration.Should().Be(duration);
        }

        [Test, AutoData]
        public void Then_The_Future_Price_Is_Not_Used(long fundingPrice, long notFundingPrice)
        {
            //Arrange / Act
            var standard = new GetStandardsListItem
            {
                Title = "Available",
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
                },
                StandardDates =
                    new StandardDate
                    {
                        EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                        LastDateStarts = null
                    }
                
            };
            
            //Assert
            standard.MaxFunding.Should().Be(fundingPrice);
        }
    }
}