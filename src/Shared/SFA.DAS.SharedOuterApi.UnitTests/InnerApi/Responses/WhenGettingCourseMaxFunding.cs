using System;
using System.Collections.Generic;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Responses;
public class WhenGettingCourseMaxFunding
{
    [Test]
    public void Then_If_There_Is_No_Available_Funding_Zero_Is_Returned_For_MaxFunding()
    {
        //Arrange/Act
        var course =
                new GetCoursesListItem
                {
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>(),
                };

        //Assert
        course.MaxFunding.Should().Be(0);
    }

    [Test]
    public void Then_If_There_Is_Null_Funding_Zero_Is_Returned()
    {
        //Arrange/Act
        var course =
            new GetCoursesListItem
            {
                ApprenticeshipFunding = null
            };

        //Assert
        course.MaxFunding.Should().Be(0);
    }

    [Test, AutoData]
    public void Then_The_ApprenticeshipFunding_Price_With_No_EffectiveTo_Date_And_Has_A_From_Date_In_The_Past_Is_Used(int fundingPrice)
    {
        //Arrange / Act
        var course = new GetCoursesListItem
        {
            ApprenticeshipFunding = new List<ApprenticeshipFunding>
            {
                new()
                {
                    EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                    EffectiveTo = null,
                    MaxEmployerLevyCap = fundingPrice
                }
            }
        };

        //Assert
        course.MaxFunding.Should().Be(fundingPrice);
    }

    [Test, AutoData]
    public void Then_The_Current_Funding_Price_Is_Used(int notFundingPrice, int fundingPrice)
    {
        //Arrange / Act
        var course = new GetCoursesListItem
        {
            ApprenticeshipFunding = new List<ApprenticeshipFunding>
            {
                new()
                {
                    EffectiveFrom = DateTime.UtcNow.AddDays(-10),
                    EffectiveTo = DateTime.UtcNow.AddDays(-9),
                    MaxEmployerLevyCap = notFundingPrice
                },
                new()
                {
                    EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                    EffectiveTo = null,
                    MaxEmployerLevyCap = fundingPrice
                }
            }
        };

        //Assert
        course.MaxFunding.Should().Be(fundingPrice);
    }



    [Test, AutoData]
    public void Then_The_Future_Price_Is_Not_Used(int fundingPrice, int notFundingPrice)
    {
        //Arrange / Act
        var course = new GetCoursesListItem
        {
            ApprenticeshipFunding = new List<ApprenticeshipFunding>
            {
                new()
                {
                    EffectiveFrom = DateTime.UtcNow.AddDays(-10),
                    EffectiveTo = DateTime.UtcNow.AddDays(9),
                    MaxEmployerLevyCap = fundingPrice
                },
                new()
                {
                    EffectiveFrom = DateTime.UtcNow.AddDays(4),
                    EffectiveTo = null,
                    MaxEmployerLevyCap = notFundingPrice
                }
            }
        };

        //Assert
        course.MaxFunding.Should().Be(fundingPrice);
    }
}