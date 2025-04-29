using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenBuildingGetEmployerFeedbackAttributes
    {

        [Test, AutoData]
        public void Then_Returns_Empty_Employer_Feedback_Attribute_Lists_If_Totals_Are_Zero(InnerApi.Responses.GetProvidersListItem source, string sectorSubjectArea)
        {
            source.EmployerFeedback.ProviderAttribute = new List<InnerApi.Responses.GetEmployerFeedbackAttributeItem>
            {
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "First Attribute",
                    Strength = 0,
                    Weakness = 0
                },
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "Second Attribute",
                    Strength = 0,
                    Weakness = 0
                },
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "Third Attribute",
                    Strength = 0,
                    Weakness = 0
                }
            };

            var response = new GetTrainingCourseProviderListItem().Map(source, new List<DeliveryModeType>(), new List<FeedbackRatingType>(), new List<FeedbackRatingType>(), true);

            response.EmployerFeedback.FeedbackAttributes.Should().BeEmpty();
        }

        [Test, AutoData]
        public void Then_No_Strengths_Returns_Zero_Count(InnerApi.Responses.GetProvidersListItem source, string sectorSubjectArea)
        {
            source.EmployerFeedback.ProviderAttribute = new List<InnerApi.Responses.GetEmployerFeedbackAttributeItem>
            {
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "First Attribute",
                    Strength = 0,
                    Weakness = 12
                },
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "Second Attribute",
                    Strength = 0,
                    Weakness = 13
                },
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "Third Attribute",
                    Strength = 0,
                    Weakness = 14
                }
            };

            var response = new GetTrainingCourseProviderListItem().Map(source, new List<DeliveryModeType>(), new List<FeedbackRatingType>(), new List<FeedbackRatingType>(), true);

            response.EmployerFeedback.FeedbackAttributes.Sum(x => x.Strength).Should().Be(0);
            response.EmployerFeedback.FeedbackAttributes.Select(x => x.AttributeName).Should().Contain(source.EmployerFeedback.ProviderAttribute.Select(c => c.Name).ToList());
            response.EmployerFeedback.FeedbackAttributes.Sum(x => x.Weakness).Should().Be(39);
        }

        [Test, AutoData]
        public void Then_No_Weaknesses_Returns_Zero_Count(InnerApi.Responses.GetProvidersListItem source, string sectorSubjectArea)
        {
            source.EmployerFeedback.ProviderAttribute = new List<InnerApi.Responses.GetEmployerFeedbackAttributeItem>
            {
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "First Attribute",
                    Strength = 12,
                    Weakness = 0
                },
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "Second Attribute",
                    Strength = 13,
                    Weakness = 0
                },
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "Third Attribute",
                    Strength = 14,
                    Weakness = 0
                }
            };

            var response = new GetTrainingCourseProviderListItem().Map(source, new List<DeliveryModeType>(), new List<FeedbackRatingType>(), new List<FeedbackRatingType>(), true);

            response.EmployerFeedback.FeedbackAttributes.Sum(x => x.Weakness).Should().Be(0);
            response.EmployerFeedback.FeedbackAttributes.Select(x => x.AttributeName).Should().Contain(source.EmployerFeedback.ProviderAttribute.Select(c => c.Name).ToList());
            response.EmployerFeedback.FeedbackAttributes.Sum(x => x.Strength).Should().Be(39);
        }


        [Test, AutoData]
        public void Then_Returns_All_Available_Employer_Feedback_Attribute_Strengths_And_Weaknesses_Where_Strengths_Weaknesses_Greater_Than_Zero(InnerApi.Responses.GetProvidersListItem source, string sectorSubjectArea)
        {
            source.EmployerFeedback.ProviderAttribute = new List<InnerApi.Responses.GetEmployerFeedbackAttributeItem>
            {
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "First Attribute",
                    Strength = 1,
                    Weakness = 1
                },
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "Second Attribute",
                    Strength = 0,
                    Weakness = 0
                },
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "Third Attribute",
                    Strength = 1,
                    Weakness = 1
                },
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "Fourth Attribute",
                    Strength = 0,
                    Weakness = 0
                },
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "Fifth Attribute",
                    Strength = 1,
                    Weakness = 1
                },
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "Sixth Attribute",
                    Strength = 0,
                    Weakness = 0
                },
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "Seventh Attribute",
                    Strength = 1,
                    Weakness = 1
                },
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "Eighth Attribute",
                    Strength = 0,
                    Weakness = 0
                },
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "Ninth Attribute",
                    Strength = 0,
                    Weakness = 0
                },
                new InnerApi.Responses.GetEmployerFeedbackAttributeItem
                {
                    Name = "Tenth Attribute",
                    Strength = 1,
                    Weakness = 1
                }
            };

            var response = new GetTrainingCourseProviderListItem().Map(source, new List<DeliveryModeType>(), new List<FeedbackRatingType>(), new List<FeedbackRatingType>(), true);

            response.EmployerFeedback.FeedbackAttributes.Select(x => x.AttributeName)
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