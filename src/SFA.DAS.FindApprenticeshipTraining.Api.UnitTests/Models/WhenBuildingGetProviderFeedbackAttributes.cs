using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenBuildingGetProviderFeedbackAttributes
    {
        
        [Test, AutoData]
        public void Then_Returns_Empty_Feedback_Attribute_Lists_If_Totals_Are_Zero(GetProvidersListItem source, string sectorSubjectArea)
        {
            source.FeedbackAttributes = new List<GetFeedbackAttributeItem>
            {
                new GetFeedbackAttributeItem
                {
                    AttributeName = "First Attribute",
                    Strength = 0,
                    Weakness = 0
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Second Attribute",
                    Strength = 0,
                    Weakness = 0
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Third Attribute",
                    Strength = 0,
                    Weakness = 0
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(), new List<FeedbackRatingType>(), true);

            response.Feedback.FeedbackAttributes.Should().BeEmpty();
        }

        [Test, AutoData]
        public void Then_No_Strengths_Returns_Zero_Count(GetProvidersListItem source, string sectorSubjectArea)
        {
            source.FeedbackAttributes = new List<GetFeedbackAttributeItem>
            {
                new GetFeedbackAttributeItem
                {
                    AttributeName = "First Attribute",
                    Strength = 0,
                    Weakness = 12
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Second Attribute",
                    Strength = 0,
                    Weakness = 13
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Third Attribute",
                    Strength = 0,
                    Weakness = 14
                }
            };

            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea, 1, new List<DeliveryModeType>(), new List<FeedbackRatingType>(), true);

            response.Feedback.FeedbackAttributes.Sum(x => x.Strength).Should().Be(0);
            response.Feedback.FeedbackAttributes.Select(x => x.AttributeName).Should().Contain(source.FeedbackAttributes.Select(c => c.AttributeName).ToList());
            response.Feedback.FeedbackAttributes.Sum(x => x.Weakness).Should().Be(39);
        }

        [Test, AutoData]
        public void Then_No_Weaknesses_Returns_Zero_Count(GetProvidersListItem source, string sectorSubjectArea)
        {
            source.FeedbackAttributes = new List<GetFeedbackAttributeItem>
            {
                new GetFeedbackAttributeItem
                {
                    AttributeName = "First Attribute",
                    Strength = 12,
                    Weakness = 0
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Second Attribute",
                    Strength = 13,
                    Weakness = 0
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Third Attribute",
                    Strength = 14,
                    Weakness = 0
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(),new List<FeedbackRatingType>(), true);

            response.Feedback.FeedbackAttributes.Sum(x => x.Weakness).Should().Be(0);
            response.Feedback.FeedbackAttributes.Select(x => x.AttributeName).Should().Contain(source.FeedbackAttributes.Select(c => c.AttributeName).ToList());
            response.Feedback.FeedbackAttributes.Sum(x => x.Strength).Should().Be(39);
        }

       
        [Test, AutoData]
        public void Then_Returns_All_Available_Feedback_Attribute_Strengths_And_Weaknesses_Where_Strengths_Weaknesses_Greater_Than_Zero(GetProvidersListItem source, string sectorSubjectArea)
        {
            source.FeedbackAttributes = new List<GetFeedbackAttributeItem>
            {
                new GetFeedbackAttributeItem
                {
                    AttributeName = "First Attribute",
                    Strength = 1,
                    Weakness = 1
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Second Attribute",
                    Strength = 0,
                    Weakness = 0
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Third Attribute",
                    Strength = 1,
                    Weakness = 1
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Fourth Attribute",
                    Strength = 0,
                    Weakness = 0
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Fifth Attribute",
                    Strength = 1,
                    Weakness = 1
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Sixth Attribute",
                    Strength = 0,
                    Weakness = 0
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Seventh Attribute",
                    Strength = 1,
                    Weakness = 1
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Eighth Attribute",
                    Strength = 0,
                    Weakness = 0
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Ninth Attribute",
                    Strength = 0,
                    Weakness = 0
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Tenth Attribute",
                    Strength = 1,
                    Weakness = 1
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(),new List<FeedbackRatingType>(), true);
            
            response.Feedback.FeedbackAttributes.Select(x => x.AttributeName)
                .Should().ContainInOrder(
                new List<string>{
                    "First Attribute",
                    "Third Attribute",
                    "Fifth Attribute",
                    "Seventh Attribute",
                    "Tenth Attribute"});
        }

    }
}