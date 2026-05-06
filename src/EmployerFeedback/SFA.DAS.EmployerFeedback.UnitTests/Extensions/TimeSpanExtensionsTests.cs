using System;
using NUnit.Framework;
using FluentAssertions;
using SFA.DAS.EmployerFeedback.Extensions;

namespace SFA.DAS.EmployerFeedback.UnitTests.Extensions
{
    [TestFixture]
    public class TimeSpanExtensionsTests
    {
        [Test]
        public void ToReadableString_ShouldCorrectlyFormatDays()
        {
            TimeSpan timeSpan = TimeSpan.FromDays(2);

            string result = timeSpan.ToReadableString();

            result.Should().Be("2 days");
        }

        [Test]
        public void ToReadableString_ShouldCorrectlyFormatHoursMinutesSeconds()
        {
            TimeSpan timeSpan = new TimeSpan(0, 2, 3, 4);

            string result = timeSpan.ToReadableString();

            result.Should().Be("2 hours, 3 minutes, 4 seconds");
        }

        [Test]
        public void ToReadableString_ShouldCorrectlyHandleSingularUnits()
        {
            TimeSpan timeSpan = new TimeSpan(1, 1, 1, 1);

            string result = timeSpan.ToReadableString();

            result.Should().Be("1 day, 1 hour, 1 minute, 1 second");
        }

        [Test]
        public void ToReadableString_ShouldReturnZeroSecondsForZeroTimeSpan()
        {
            TimeSpan timeSpan = TimeSpan.Zero;

            string result = timeSpan.ToReadableString();

            result.Should().Be("0 seconds");
        }

        [Test]
        public void ToReadableString_ShouldCorrectlyFormatComplexTimeSpan()
        {
            TimeSpan timeSpan = new TimeSpan(1, 22, 333, 44444);

            string result = timeSpan.ToReadableString();

            result.Should().Be("2 days, 15 hours, 53 minutes, 44 seconds");
        }
    }

}
