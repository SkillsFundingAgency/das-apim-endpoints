using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingToGetApprenticeFeedbackAttributeItemFromGetApprenticeFeedbackAttributeItem
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(InnerApi.Responses.GetApprenticeFeedbackAttributeItem source)
        {
            var actual = (Api.Models.GetApprenticeFeedbackAttributeItem) source;
            
            actual.Should().BeEquivalentTo(source);
        }
        [Test, AutoData]
        public void Then_The_Totals_Are_Added(InnerApi.Responses.GetApprenticeFeedbackAttributeItem source)
        {
            var actual = (Api.Models.GetApprenticeFeedbackAttributeItem) source;
            
            actual.TotalVotes.Should().Be(source.Disagree + source.Agree);
            actual.Rating.Should().Be(source.Agree - source.Disagree);
        }
    }
}