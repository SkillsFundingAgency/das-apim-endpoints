using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Domain;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.UnitTests.Domain
{
    public class WhenMappingIsStandardActive
    {
        [Test, AutoData]
        public void And_EffectiveFrom_After_Today_Then_False(GetStandardsListItem standard)
        {
            standard.StandardDates.EffectiveFrom = DateTime.UtcNow.AddDays(1);
            standard.StandardDates.EffectiveTo = null;

            var result = IsStandardActiveMapper.IsStandardActive(standard);
            
            result.Should().BeFalse();
        }

        [Test, AutoData]
        public void And_EffectiveTo_Before_Today_Then_False(GetStandardsListItem standard)
        {
            standard.StandardDates.EffectiveFrom = DateTime.UtcNow;
            standard.StandardDates.EffectiveTo = DateTime.UtcNow.AddDays(-1);
            var result = IsStandardActiveMapper.IsStandardActive(standard);
            result.Should().BeFalse();
        }

        [Test, AutoData]
        public void And_EffectiveFrom_And_To_Are_Today_Then_True(GetStandardsListItem standard)
        {
            standard.StandardDates.EffectiveFrom = DateTime.UtcNow;
            standard.StandardDates.EffectiveTo = DateTime.UtcNow;
            var result = IsStandardActiveMapper.IsStandardActive(standard);
            result.Should().BeTrue();
        }

        [Test, AutoData]
        public void And_EffectiveFrom_Today_And_No_EffectiveTo_Then_True(GetStandardsListItem standard)
        {
            standard.StandardDates.EffectiveFrom = DateTime.UtcNow;
            standard.StandardDates.EffectiveTo = null;
            var result = IsStandardActiveMapper.IsStandardActive(standard);
            result.Should().BeTrue();
        }

        [Test, AutoData]
        public void And_EffectiveFrom_Before_Today_And_EffectiveTo_After_Today_Then_True(GetStandardsListItem standard)
        {
            standard.StandardDates.EffectiveFrom = DateTime.UtcNow.AddDays(-1);
            standard.StandardDates.EffectiveTo = DateTime.UtcNow.AddDays(1);
            var result = IsStandardActiveMapper.IsStandardActive(standard);
            result.Should().BeTrue();
        }
    }
}