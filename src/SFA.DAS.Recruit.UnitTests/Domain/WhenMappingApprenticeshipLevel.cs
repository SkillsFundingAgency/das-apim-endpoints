using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.UnitTests.Domain
{
    public class WhenMappingApprenticeshipLevel
    {
        // Remap

        [Test]
        public void When_Foundation5_Then_Return_Higher()
        {
            var result = ApprenticeshipLevelHelper.RemapFromInt(5);
            result.Should().Be(ApprenticeshipLevel.Higher);
        }

        [Test]
        public void When_Masters7_Then_Return_Degree()
        {
            var result = ApprenticeshipLevelHelper.RemapFromInt(7);
            result.Should().Be(ApprenticeshipLevel.Degree);
        }

        [Test]
        public void When_Int_With_Corresponding_Value_Then_Return_CorrectEnum()
        {
            var enumValues = Enum.GetValues(typeof(ApprenticeshipLevel))
                .OfType<ApprenticeshipLevel>();
            foreach (var enumValue in enumValues)
            {
                var result = ApprenticeshipLevelHelper.RemapFromInt((int)enumValue);
                result.Should().Be(enumValue);
            }
        }

        [Test]
        public void When_Passed_An_Int_With_No_Corresponding_Value_Then_Throw_ArgumentException()
        {
            var intValueToConvert = (int)ApprenticeshipLevel.Degree + 2;
            Assert.Throws<ArgumentException>(() =>
            {
                ApprenticeshipLevelHelper.RemapFromInt(intValueToConvert);
            });
        }

        //TryRemap

        [Test]
        public void When_Foundation5_Then_True_And_Return_Higher()
        {
            var success = ApprenticeshipLevelHelper.TryRemapFromInt(5, out var result);
            success.Should().Be(true);
            result.Should().Be(ApprenticeshipLevel.Higher);
        }

        [Test]
        public void When_Masters7_Then_True_And_Return_Degree()
        {
            var success = ApprenticeshipLevelHelper.TryRemapFromInt(7, out var result);
            success.Should().Be(true);
            result.Should().Be(ApprenticeshipLevel.Degree);
        }

        [Test]
        public void When_Int_With_Corresponding_Value_Then_True_And_Return_CorrectEnum()
        {
            var enumValues = Enum.GetValues(typeof(ApprenticeshipLevel))
                .OfType<ApprenticeshipLevel>();
            foreach (ApprenticeshipLevel enumValue in enumValues)
            {
                var success = ApprenticeshipLevelHelper.TryRemapFromInt((int)enumValue, out var result);
                success.Should().Be(true);
                result.Should().Be(enumValue);
            }
        }

        [Test]
        public void When_Passed_An_Int_With_No_Corresponding_Value_Then_False()
        {
            var intValueToConvert = (int)ApprenticeshipLevel.Degree + 2;
            var success = ApprenticeshipLevelHelper.TryRemapFromInt(intValueToConvert, out var result);
            success.Should().Be(false);
            result.Should().Be(ApprenticeshipLevel.Unknown);
        }
    }
}