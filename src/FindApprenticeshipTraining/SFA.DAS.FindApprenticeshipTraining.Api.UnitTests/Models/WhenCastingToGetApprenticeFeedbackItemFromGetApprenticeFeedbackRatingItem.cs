using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingToGetApprenticeFeedbackItemFromGetApprenticeFeedbackRatingItem
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetApprenticeFeedbackRatingItem source)
        {
            var actual = (GetApprenticeFeedbackItem)source;
            
            actual.Should().BeEquivalentTo(source);
        }
    }
}