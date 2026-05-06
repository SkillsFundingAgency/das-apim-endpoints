using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Responses.StandardApiResponseBaseTests
{
    public class WhenGettingTypicalDuration
    {
        [Test]
        public void Then_If_There_Is_No_Available_Funding_Zero_Is_Returned_For_TypicalDuration()
        {
            //Arrange/Act
            var standard =
                new TestStandardResponse
                {
                    ApprenticeshipFunding = new List<ApprenticeshipFunding>()
                };

            //Assert
            standard.TypicalDuration.Should().Be(0);
        }
        
        [Test]
        public void Then_If_There_Is_Null_Available_Funding_Zero_Is_Returned_For_TypicalDuration()
        {
            //Arrange/Act
            var standard =
                new TestStandardResponse
                {
                    ApprenticeshipFunding = null
                };

            //Assert
            standard.TypicalDuration.Should().Be(0);
        }

        [Test, AutoData]
        public void Then_The_Typical_Duration_Is_Used(int notDuration, int duration)
        {
            //Arrange / Act
            var standard = new TestStandardResponse
            {
                ApprenticeshipFunding = new List<ApprenticeshipFunding>
                {
                    new()
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-10),
                        EffectiveTo = DateTime.UtcNow.AddDays(-9),
                        Duration = notDuration
                    },
                    new()
                    {
                        EffectiveFrom = DateTime.UtcNow.AddDays(-1),
                        EffectiveTo = null,
                        Duration = duration
                    }
                }

            };

            //Assert
            standard.TypicalDuration.Should().Be(duration);
        }
    }
}