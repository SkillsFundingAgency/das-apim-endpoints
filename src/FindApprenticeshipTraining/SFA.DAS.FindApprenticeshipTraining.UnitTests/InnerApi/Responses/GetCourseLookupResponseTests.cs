using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Responses
{
    public class GetCourseLookupResponseTests
    {
        [Test]
        public void IsActiveAvailable_CourseDatesAreNull_ReturnsFalse()
        {
            var standard = new GetCourseLookupResponse
            {
                CourseDates = null
            };

            standard.IsActiveAvailable.Should().BeFalse();
        }

        [Test]
        public void IsActiveAvailable_LastDateStartsIsNullAndEffectiveFromIsToday_ReturnsTrue()
        {
            var standard = new GetCourseLookupResponse
            {
                CourseDates = new()
                {
                    EffectiveFrom = DateTime.UtcNow.Date,
                    LastDateStarts = null
                }
            };

            standard.IsActiveAvailable.Should().BeTrue();
        }

        [Test]
        public void IsActiveAvailable_LastDateStartsIsInFutureAndEffectiveFromIsInPast_ReturnsTrue()
        {
            var standard = new GetCourseLookupResponse
            {
                CourseDates = new()
                {
                    EffectiveFrom = DateTime.UtcNow.Date.AddDays(-1),
                    LastDateStarts = DateTime.UtcNow.Date.AddDays(1)
                }
            };

            standard.IsActiveAvailable.Should().BeTrue();
        }

        [Test]
        public void IsActiveAvailable_LastDateStartsEqualsEffectiveFrom_ReturnsFalse()
        {
            var today = DateTime.UtcNow.Date;
            var standard = new GetCourseLookupResponse
            {
                CourseDates = new()
                {
                    EffectiveFrom = today,
                    LastDateStarts = today
                }
            };

            standard.IsActiveAvailable.Should().BeFalse();
        }

        [Test]
        public void IsActiveAvailable_LastDateStartsIsInPast_ReturnsFalse()
        {
            var standard = new GetCourseLookupResponse
            {
                CourseDates = new()
                {
                    EffectiveFrom = DateTime.UtcNow.Date.AddDays(-10),
                    LastDateStarts = DateTime.UtcNow.Date.AddDays(-1)
                }
            };

            standard.IsActiveAvailable.Should().BeFalse();
        }

        [Test]
        public void IsActiveAvailable_EffectiveFromIsInFuture_ReturnsFalse()
        {
            var standard = new GetCourseLookupResponse
            {
                CourseDates = new()
                {
                    EffectiveFrom = DateTime.UtcNow.Date.AddDays(1),
                    LastDateStarts = DateTime.UtcNow.Date.AddDays(10)
                }
            };

            standard.IsActiveAvailable.Should().BeFalse();
        }
    }
}
