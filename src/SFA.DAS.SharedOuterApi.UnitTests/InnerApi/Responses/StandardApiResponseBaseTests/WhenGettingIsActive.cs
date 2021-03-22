using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Responses.StandardApiResponseBaseTests
{
    public class WhenGettingIsActive
    {
        [Test, AutoData]
        public void And_StandardDates_Are_Not_Available_Then_False(TestStandardResponse standard)
        {
            standard.StandardDates = null;

            standard.IsActive.Should().BeFalse();
        }

        [Test, AutoData]
        public void And_EffectiveFrom_After_Today_Then_False(TestStandardResponse standard)
        {
            standard.StandardDates.EffectiveFrom = DateTime.UtcNow.AddDays(1);
            standard.StandardDates.EffectiveTo = null;

            standard.IsActive.Should().BeFalse();
        }

        [Test, AutoData]
        public void And_EffectiveTo_Before_Today_Then_False(TestStandardResponse standard)
        {
            standard.StandardDates.EffectiveFrom = DateTime.UtcNow;
            standard.StandardDates.EffectiveTo = DateTime.UtcNow.AddDays(-1);

            standard.IsActive.Should().BeFalse();
        }

        [Test, AutoData]
        public void And_EffectiveFrom_And_To_Are_Today_Then_True(TestStandardResponse standard)
        {
            standard.StandardDates.EffectiveFrom = DateTime.UtcNow;
            standard.StandardDates.EffectiveTo = DateTime.UtcNow;

            standard.IsActive.Should().BeTrue();
        }

        [Test, AutoData]
        public void And_EffectiveFrom_Today_And_No_EffectiveTo_Then_True(TestStandardResponse standard)
        {
            standard.StandardDates.EffectiveFrom = DateTime.UtcNow;
            standard.StandardDates.EffectiveTo = null;

            standard.IsActive.Should().BeTrue();
        }

        [Test, AutoData]
        public void And_EffectiveFrom_Before_Today_And_EffectiveTo_After_Today_Then_True(TestStandardResponse standard)
        {
            standard.StandardDates.EffectiveFrom = DateTime.UtcNow.AddDays(-1);
            standard.StandardDates.EffectiveTo = DateTime.UtcNow.AddDays(1);

            standard.IsActive.Should().BeTrue();
        }
    }
}