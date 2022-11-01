
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using GetEmployerFeedbackItem = SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses.GetEmployerFeedbackItem;
using GetEmployerFeedbackSummaryItem = SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses.GetEmployerFeedbackSummaryItem;


namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingToGetEmployerFeedbackItemFromGetEmployerFeedbackSummaryItem
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetEmployerFeedbackSummaryItem source)
        {
            GetEmployerFeedbackItem actual = (GetEmployerFeedbackSummaryItem)source;

            actual.Should().NotBeNull();
            actual.FeedbackAttributes.Should().NotBeNull();
            actual.FeedbackAttributes.Should().BeEmpty();

            actual.FeedbackRatings.Should().NotBeNull();
            actual.FeedbackRatings.Any().Should().BeTrue();
            actual.FeedbackRatings.Where(x => x.FeedbackName == "ReviewCount").Count().Should().Be(1);
            actual.FeedbackRatings.Where(x => x.FeedbackName == "Stars").Count().Should().Be(1);
            actual.FeedbackRatings.FirstOrDefault(x => x.FeedbackName == "ReviewCount").FeedbackCount.Should().Be(source.ReviewCount);
            actual.FeedbackRatings.FirstOrDefault(x => x.FeedbackName == "Stars").FeedbackCount.Should().Be(source.Stars);
        }
    }
}
