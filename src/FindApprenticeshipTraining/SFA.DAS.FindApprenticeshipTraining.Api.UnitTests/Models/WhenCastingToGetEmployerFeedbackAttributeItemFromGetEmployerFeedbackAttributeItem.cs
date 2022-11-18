using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingToGetEmployerFeedbackAttributeItemFromGetEmployerFeedbackAttributeItem
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(InnerApi.Responses.GetEmployerFeedbackAttributeItem source)
        {
            var actual = (Api.Models.GetEmployerFeedbackAttributeItem) source;

            actual.Should().BeEquivalentTo(new
            {
                AttributeName = source.Name,
                source.Strength,
                source.Weakness
            });
        }
        [Test, AutoData]
        public void Then_The_Totals_Are_Added(InnerApi.Responses.GetEmployerFeedbackAttributeItem source)
        {
            var actual = (Api.Models.GetEmployerFeedbackAttributeItem) source;
            
            actual.TotalVotes.Should().Be(source.Weakness + source.Strength);
            actual.Rating.Should().Be(source.Strength - source.Weakness);
        }
    }
}