using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindEpao.Application.Courses.Services;
using SFA.DAS.FindEpao.InnerApi.Responses;

namespace SFA.DAS.FindEpao.UnitTests.Application.Courses.Services
{
    public class WhenCheckingIfCourseEpaoIsValid
    {
        [Test]
        //status
        [InlineAutoData(true, "Live", DateOccurrence.Past, DateOccurrence.Past, null)]
        [InlineAutoData(true, "Live", DateOccurrence.Past, DateOccurrence.Past, DateOccurrence.Future)]
        [InlineAutoData(true, "Live", DateOccurrence.Present, DateOccurrence.Present, null)]
        [InlineAutoData(true, "Live", DateOccurrence.Present, DateOccurrence.Present, DateOccurrence.Future)]
        [InlineAutoData(false, "NotLive", DateOccurrence.Past, DateOccurrence.Past, null)]
        [InlineAutoData(false, "NotLive", DateOccurrence.Past, DateOccurrence.Past, DateOccurrence.Future)]
        [InlineAutoData(false, "NotLive", DateOccurrence.Present, DateOccurrence.Present, null)]
        [InlineAutoData(false, "NotLive", DateOccurrence.Present, DateOccurrence.Present, DateOccurrence.Future)]
        //dateStandardApprovedOnRegister
        [InlineAutoData(false, "Live", DateOccurrence.Future, DateOccurrence.Past, null)]
        [InlineAutoData(false, "Live", null, DateOccurrence.Past, null)]
        //effectiveTo
        [InlineAutoData(false, "Live", DateOccurrence.Past, DateOccurrence.Past, DateOccurrence.Past)]
        [InlineAutoData(true, "Live", DateOccurrence.Past, DateOccurrence.Past, DateOccurrence.Present)]
        //effectiveFrom
        [InlineAutoData(false, "Live", DateOccurrence.Past, null, DateOccurrence.Future)]
        [InlineAutoData(true, "Live", DateOccurrence.Past, DateOccurrence.Past, DateOccurrence.Future)]
        public void Check_Validity(
            bool isValid,
            string status,
            DateOccurrence? dateStandardApprovedOnRegister,
            DateOccurrence? effectiveFrom,
            DateOccurrence? effectiveTo,
            CourseEpaoIsValidFilterService service)
        {
            var courseEpao = new GetCourseEpaoListItem
            {
                Status = status,
                CourseEpaoDetails = new CourseEpaoDetails
                {
                    DateStandardApprovedOnRegister = dateStandardApprovedOnRegister?.ToDateTime(),
                    EffectiveFrom = effectiveFrom?.ToDateTime(),
                    EffectiveTo = effectiveTo?.ToDateTime()
                }
            };

            var result = service.IsValidCourseEpao(courseEpao);

            result.Should().Be(isValid);
        }
    }

    public enum DateOccurrence
    {
        Past,
        Present,
        Future
    }

    public static class DateOccurrenceExtension
    {
        public static DateTime ToDateTime(this DateOccurrence occurrence)
        {
            var date = DateTime.Today;
            return occurrence switch
            {
                DateOccurrence.Past => date.AddDays(-5),
                DateOccurrence.Present => date,
                DateOccurrence.Future => date.AddDays(5)
            };
        }
    }
}