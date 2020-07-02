using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Responses
{
    public class WhenBuildingTheGetStandardsListItemResponse
    {
        
        [Test]
        public void Then_If_There_Is_No_Available_Funding_Zero_Is_Returned()
        {
            var standards = new List<GetStandardsListItem>
                {
                    new GetStandardsListItem
                    {
                        Title = "Available",
                        ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                        StandardDates = new List<StandardDate>
                        {
                            new StandardDate
                            {
                                EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                                LastDateStarts = null
                            }
                        }
                    }
                
            };
            
            //Assert
            Assert.IsTrue(standards.ToList().TrueForAll(c=>c.MaxFunding.Equals(0)));
        }
        
        [Test, AutoData]
        public void Then_The_Funding_Amount_Is_Taken_From_The_Valid_ApprenticeshipFunding_List(long fundingPrice, long notFundingPrice)
        {
            var standards = new List<GetStandardsListItem>
                {
                    new GetStandardsListItem
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
                        StandardDates = new List<StandardDate>
                        {
                            new StandardDate
                            {
                                EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                                LastDateStarts = null
                            }
                        }
                    },
                    new GetStandardsListItem
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
                        StandardDates = new List<StandardDate>
                        {
                            new StandardDate
                            {
                                EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                                LastDateStarts = null
                            }
                        }
                        
                    },
                    new GetStandardsListItem
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
                        StandardDates = new List<StandardDate>
                        {
                            new StandardDate
                            {
                                EffectiveFrom = DateTime.UtcNow.AddMonths(-1),
                                LastDateStarts = null
                            }
                        }
                        
                    }
                
            };
            
            //Assert
            Assert.IsTrue(standards.ToList().TrueForAll(c=>c.MaxFunding.Equals(fundingPrice)));
        }
    }
}