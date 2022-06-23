﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingToGetProviderFeedbackAttributeItemFromGetFeedbackAttributeItem
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetFeedbackAttributeItem source)
        {
            var actual = (GetProviderFeedbackAttributeItem) source;
            
            actual.Should().BeEquivalentTo(source);
        }
        [Test, AutoData]
        public void Then_The_Totals_Are_Added(GetFeedbackAttributeItem source)
        {
            var actual = (GetProviderFeedbackAttributeItem) source;
            
            actual.TotalVotes.Should().Be(source.Weakness + source.Strength);
            actual.Rating.Should().Be(source.Strength - source.Weakness);
        }
    }
}