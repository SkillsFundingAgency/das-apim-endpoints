using System;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Responses.StandardApiResponseBaseTests
{
    public class WhenGettingIsActive
    {
        [Test, AutoData]
        public void And_StandardDates_Are_Not_Available_Then_False(TestStandardResponse standard)
        {
            standard.CourseDates = null;

            standard.IsActive.Should().BeFalse();
        }

        [Test, AutoData]
        public void And_EffectiveFrom_After_Today_Then_False(TestStandardResponse standard)
        {
            standard.CourseDates.EffectiveFrom = DateTime.UtcNow.AddDays(1);
            standard.CourseDates.EffectiveTo = null;

            standard.IsActive.Should().BeFalse();
        }

        [Test, AutoData]
        public void And_EffectiveTo_Before_Today_Then_False(TestStandardResponse standard)
        {
            standard.CourseDates.EffectiveFrom = DateTime.UtcNow;
            standard.CourseDates.EffectiveFrom = DateTime.UtcNow.AddDays(-50);
            standard.CourseDates.EffectiveTo = DateTime.UtcNow.AddDays(-1);

            standard.IsActive.Should().BeFalse();
        }

        [Test, AutoData]
        public void And_EffectiveFrom_And_To_Are_Today_Then_True(TestStandardResponse standard)
        {
            standard.CourseDates.EffectiveFrom = DateTime.UtcNow;
            standard.CourseDates.EffectiveTo = DateTime.UtcNow;
            standard.CourseDates.LastDateStarts = null;

            standard.IsActive.Should().BeTrue();
        }

        [Test, AutoData]
        public void And_EffectiveFrom_Today_And_No_EffectiveTo_Then_True(TestStandardResponse standard)
        {
            standard.CourseDates.EffectiveFrom = DateTime.UtcNow;
            standard.CourseDates.EffectiveTo = null;
            standard.CourseDates.LastDateStarts = null;

            standard.IsActive.Should().BeTrue();
        }

        [Test, AutoData]
        public void And_EffectiveFrom_Today_And_No_LastDateStarts_Then_True(TestStandardResponse standard)
        {
            standard.CourseDates.EffectiveFrom = DateTime.UtcNow;
            standard.CourseDates.EffectiveTo = null;
            standard.CourseDates.LastDateStarts = null;

            standard.IsActive.Should().BeTrue();
        }

        [Test, AutoData]
        public void And_EffectiveFrom_Today_And_No_LastDateStart_AfterToday_Then_True(TestStandardResponse standard)
        {
            standard.CourseDates.EffectiveFrom = DateTime.UtcNow.AddDays(-10);
            standard.CourseDates.EffectiveTo = null;
            standard.CourseDates.LastDateStarts = DateTime.UtcNow.AddDays(10); ;

            standard.IsActive.Should().BeTrue();
        }

        [Test, AutoData]
        public void And_EffectiveFrom_Today_And_No_LastDateStart_BeforeToday_Then_False(TestStandardResponse standard)
        {
            standard.CourseDates.EffectiveFrom = DateTime.UtcNow.AddDays(-10);
            standard.CourseDates.EffectiveTo = null;
            standard.CourseDates.LastDateStarts = DateTime.UtcNow.AddDays(-1); ;

            standard.IsActive.Should().BeFalse();
        }

        [Test, AutoData]
        public void And_EffectiveFrom_Before_Today_And_EffectiveTo_After_Today_Then_True(TestStandardResponse standard)
        {
            standard.CourseDates.LastDateStarts = null;
            standard.CourseDates.EffectiveFrom = DateTime.UtcNow.AddDays(-1);
            standard.CourseDates.EffectiveTo = DateTime.UtcNow.AddDays(1);

            standard.IsActive.Should().BeTrue();
        }
    }
}