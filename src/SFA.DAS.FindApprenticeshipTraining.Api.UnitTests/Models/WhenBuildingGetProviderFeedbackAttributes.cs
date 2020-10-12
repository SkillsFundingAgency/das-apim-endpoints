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
                    Strength = 10,
                    Weakness = 10
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Second Attribute",
                    Strength = 12,
                    Weakness = 12
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Third Attribute",
                    Strength = 13,
                    Weakness = 13
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(),new List<FeedbackRatingType>());

            response.Feedback.FeedbackAttributes.Strengths.Should().BeEmpty();
            response.Feedback.FeedbackAttributes.Weaknesses.Should().BeEmpty();
        }

        [Test, AutoData]
        public void Then_No_Strengths_Returns_Empty_List(GetProvidersListItem source, string sectorSubjectArea)
        {
            source.FeedbackAttributes = new List<GetFeedbackAttributeItem>
            {
                new GetFeedbackAttributeItem
                {
                    AttributeName = "First Attribute",
                    Strength = 10,
                    Weakness = 12
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Second Attribute",
                    Strength = 10,
                    Weakness = 13
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Third Attribute",
                    Strength = 10,
                    Weakness = 14
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(),new List<FeedbackRatingType>());

            response.Feedback.FeedbackAttributes.Strengths.Should().BeEmpty();
            response.Feedback.FeedbackAttributes.Weaknesses.Should().Contain(source.FeedbackAttributes.Select(c => c.AttributeName).ToList());
            response.Feedback.FeedbackAttributes.Weaknesses.Should().NotBeEmpty();
        }

        [Test, AutoData]
        public void Then_No_Weaknesses_Returns_Empty_List(GetProvidersListItem source, string sectorSubjectArea)
        {
            source.FeedbackAttributes = new List<GetFeedbackAttributeItem>
            {
                new GetFeedbackAttributeItem
                {
                    AttributeName = "First Attribute",
                    Strength = 12,
                    Weakness = 10
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Second Attribute",
                    Strength = 13,
                    Weakness = 12
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Third Attribute",
                    Strength = 14,
                    Weakness = 13
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(),new List<FeedbackRatingType>());

            response.Feedback.FeedbackAttributes.Strengths.Should().NotBeEmpty();
            response.Feedback.FeedbackAttributes.Strengths.Should().Contain(source.FeedbackAttributes.Select(c => c.AttributeName).ToList());
            response.Feedback.FeedbackAttributes.Weaknesses.Should().BeEmpty();
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
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(),new List<FeedbackRatingType>());
            
            response.Feedback.FeedbackAttributes.Weaknesses.Should().ContainInOrder(new List<string>{"Yattribute", "Zattribute", "Attribute"});
        }
        
        [Test, AutoData]
        public void Then_If_Same_Feedback_Attribute_Score_And_Same_Responses_Then_The_Attribute_Is_Returned_In_Alphabetical_Order(GetProvidersListItem source, string sectorSubjectArea)
        {
            source.FeedbackAttributes = new List<GetFeedbackAttributeItem>
            {
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Sixth Attribute",
                    Strength = 14,
                    Weakness = 13
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Seventh Attribute",
                    Strength = 15,
                    Weakness = 14
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(),new List<FeedbackRatingType>());
            
            response.Feedback.FeedbackAttributes.Strengths.Should().ContainInOrder(new List<string>{"Seventh Attribute"});
        }
        

        [Test, AutoData]
        public void Then_Returns_The_Top_Three_Feedback_Attribute_Strengths_And_Weaknesses(GetProvidersListItem source, string sectorSubjectArea)
        {
            source.FeedbackAttributes = new List<GetFeedbackAttributeItem>
            {
                new GetFeedbackAttributeItem
                {
                    AttributeName = "First Attribute",
                    Strength = 11,
                    Weakness = 10
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Second Attribute",
                    Strength = 11,
                    Weakness = 10
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Third Attribute",
                    Strength = 13,
                    Weakness = 10
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Fourth Attribute",
                    Strength = 14,
                    Weakness = 10
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Fifth Attribute",
                    Strength = 11,
                    Weakness = 11
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Sixth Attribute",
                    Strength = 13,
                    Weakness = 14
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Seventh Attribute",
                    Strength = 14,
                    Weakness = 15
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Eighth Attribute",
                    Strength = 14,
                    Weakness = 20
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Ninth Attribute",
                    Strength = 140,
                    Weakness = 144
                },
                new GetFeedbackAttributeItem
                {
                    AttributeName = "Tenth Attribute",
                    Strength = 10,
                    Weakness = 14
                }
            };
            
            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea,1,new List<DeliveryModeType>(),new List<FeedbackRatingType>());
            
            response.Feedback.FeedbackAttributes.Strengths.Should().ContainInOrder(new List<string>{"Fourth Attribute", "Third Attribute", "First Attribute"});
            response.Feedback.FeedbackAttributes.Weaknesses.Should().ContainInOrder(new List<string>{"Eighth Attribute", "Ninth Attribute", "Tenth Attribute"});
        }

    }
}