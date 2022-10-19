
using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using Castle.Core.Internal;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using GetEmployerFeedbackAttributeItem = SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses.GetEmployerFeedbackAttributeItem;
using GetEmployerFeedbackItem = SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses.GetEmployerFeedbackItem;
using GetEmployerFeedbackResponse = SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses.GetEmployerFeedbackResponse;


namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingToGetEmployerFeedbackItemFromGetEmployerFeedbackResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetEmployerFeedbackResponse source)
        {
            GetEmployerFeedbackItem actual = (GetEmployerFeedbackResponse)source;

            actual.Should().NotBeNull();
            actual.FeedbackAttributes.Should().NotBeNull();
            actual.FeedbackAttributes.Any().Should().BeTrue();
            actual.FeedbackAttributes.Where(x => x.AttributeName.IsNullOrEmpty()).Any().Should().BeFalse();

            IEnumerable<GetEmployerFeedbackAttributeItem> extra =
                actual.FeedbackAttributes.Where(e =>
                    !source.ProviderAttribute.Any(x =>
                        e.AttributeName == x.Name &&
                        e.Strength == x.Strength &&
                        e.Weakness == x.Weakness
                    )
                );

            extra.Should().BeEmpty();

            IEnumerable<GetEmployerFeedbackResponseDetailItem> missing =
                source.ProviderAttribute.Where(m =>
                    !actual.FeedbackAttributes.Any(x =>
                        x.AttributeName == m.Name &&
                        x.Strength == m.Strength &&
                        x.Weakness == m.Weakness
                    )
                );

            missing.Should().BeEmpty();

            actual.FeedbackRatings.Should().NotBeNull();
            actual.FeedbackRatings.Any().Should().BeTrue();
            actual.FeedbackRatings.Where(x => x.FeedbackName == "ReviewCount").Count().Should().Be(1);
            actual.FeedbackRatings.Where(x => x.FeedbackName == "Stars").Count().Should().Be(1);
            actual.FeedbackRatings.FirstOrDefault(x => x.FeedbackName == "ReviewCount").FeedbackCount.Should().Be(source.ReviewCount);
            actual.FeedbackRatings.FirstOrDefault(x => x.FeedbackName == "Stars").FeedbackCount.Should().Be(source.Stars);
        }
    }
}
