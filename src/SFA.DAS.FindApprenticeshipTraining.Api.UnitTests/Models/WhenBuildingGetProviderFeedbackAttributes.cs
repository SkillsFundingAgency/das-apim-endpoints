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

            response.Feedback.FeedbackAttributes.FeedbackAttributeDetail.Should().BeEmpty();
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

            response.Feedback.FeedbackAttributes.FeedbackAttributeDetail.Sum(x => x.StrengthCount).Should().Be(0);
            response.Feedback.FeedbackAttributes.FeedbackAttributeDetail.Select(x => x.AttributeName).Should().Contain(source.FeedbackAttributes.Select(c => c.AttributeName).ToList());
            response.Feedback.FeedbackAttributes.FeedbackAttributeDetail.Sum(x => x.WeaknessCount).Should().Be(39);
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

            response.Feedback.FeedbackAttributes.FeedbackAttributeDetail.Sum(x => x.WeaknessCount).Should().Be(0);
            response.Feedback.FeedbackAttributes.FeedbackAttributeDetail.Select(x => x.AttributeName).Should().Contain(source.FeedbackAttributes.Select(c => c.AttributeName).ToList());
            response.Feedback.FeedbackAttributes.FeedbackAttributeDetail.Sum(x => x.StrengthCount).Should().Be(39);
        }

        [Test, AutoData]
        public void Then_If_Same_Feedback_Attribute_Score_Then_The_Attribute_With_Most_Response_Is_First(GetProvidersListItem source, string sectorSubjectArea)
        {
            source.FeedbackAttributes = new List<GetFeedbackAttributeItem>
            {
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Zattribute",
                    Strength = 140,
                    Weakness = 144
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Yattribute",
                    Strength = 1040,
                    Weakness = 1044
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Attribute",
                    Strength = 10,
                    Weakness = 14
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(), new List<FeedbackRatingType>(), true);
            
            response.Feedback.FeedbackAttributes.FeedbackAttributeDetail.Select(x => x.AttributeName).Should().ContainInOrder(new List<string>{"Yattribute", "Zattribute", "Attribute"});
        }
        
        [Test, AutoData]
        public void Then_Returns_All_Available_Feedback_Attribute_Strengths_And_Weaknesses(GetProvidersListItem source, string sectorSubjectArea)
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
                    Strength = 1,
                    Weakness = 1
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
                    Strength = 1,
                    Weakness = 1
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
                    Strength = 1,
                    Weakness = 1
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
                    Strength = 1,
                    Weakness = 1
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Ninth Attribute",
                    Strength = 1,
                    Weakness = 1
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Tenth Attribute",
                    Strength = 1,
                    Weakness = 1
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(),new List<FeedbackRatingType>(), true);
            
            response.Feedback.FeedbackAttributes.FeedbackAttributeDetail.Select(x => x.AttributeName)
                .Should().ContainInOrder(
                new List<string>{
                    "First Attribute",
                    "Second Attribute",
                    "Third Attribute",
                    "Fourth Attribute",
                    "Fifth Attribute",
                    "Sixth Attribute",
                    "Seventh Attribute",
                    "Eighth Attribute",
                    "Ninth Attribute", 
                    "Tenth Attribute"});
        }

    }
}