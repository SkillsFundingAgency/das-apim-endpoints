using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenBuildingGetApprenticeFeedbackAttributes
    {

        [Test, AutoData]
        public void Then_Returns_EmptyApprentice_Feedback_Attribute_Lists_If_Totals_Are_Zero(InnerApi.Responses.GetProvidersListItem source, string sectorSubjectArea)
        {
            source.ApprenticeFeedback.ProviderAttribute = new List<InnerApi.Responses.GetApprenticeFeedbackAttributeItem>
            {
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "First Attribute",
                    Category = "Category",
                    Agree = 0,
                    Disagree = 0
                },
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "Second Attribute",
                    Category = "Category",
                    Agree = 0,
                    Disagree = 0
                },
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "Third Attribute",
                    Category = "Category",
                    Agree = 0,
                    Disagree = 0
                }
            };

            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea, 1, new List<DeliveryModeType>(), new List<FeedbackRatingType>(), new List<FeedbackRatingType>(), true);

            response.ApprenticeFeedback.FeedbackAttributes.Should().BeEmpty();
        }

        [Test, AutoData]
        public void Then_No_Agrees_Returns_Zero_Count(InnerApi.Responses.GetProvidersListItem source, string sectorSubjectArea)
        {
            source.ApprenticeFeedback.ProviderAttribute = new List<InnerApi.Responses.GetApprenticeFeedbackAttributeItem>
            {
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "First Attribute",
                    Category = "Category",
                    Agree = 0,
                    Disagree = 12
                },
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "Second Attribute",
                    Category = "Category",
                    Agree = 0,
                    Disagree = 13
                },
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "Third Attribute",
                    Category = "Category",
                    Agree = 0,
                    Disagree = 14
                }
            };

            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea, 1, new List<DeliveryModeType>(), new List<FeedbackRatingType>(), new List<FeedbackRatingType>(), true);

            response.ApprenticeFeedback.FeedbackAttributes.Sum(x => x.Agree).Should().Be(0);
            response.ApprenticeFeedback.FeedbackAttributes.Select(x => x.Name).Should().Contain(source.ApprenticeFeedback.ProviderAttribute.Select(c => c.Name).ToList());
            response.ApprenticeFeedback.FeedbackAttributes.Sum(x => x.Disagree).Should().Be(39);
        }

        [Test, AutoData]
        public void Then_No_Disagree_Returns_Zero_Count(InnerApi.Responses.GetProvidersListItem source, string sectorSubjectArea)
        {
            source.ApprenticeFeedback.ProviderAttribute = new List<InnerApi.Responses.GetApprenticeFeedbackAttributeItem>
            {
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "First Attribute",
                    Category = "Category",
                    Agree = 12,
                    Disagree = 0
                },
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "Second Attribute",
                    Category = "Category",
                    Agree = 13,
                    Disagree = 0
                },
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "Third Attribute",
                    Category = "Category",
                    Agree = 14,
                    Disagree = 0
                }
            };

            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea, 1, new List<DeliveryModeType>(), new List<FeedbackRatingType>(), new List<FeedbackRatingType>(), true);

            response.ApprenticeFeedback.FeedbackAttributes.Sum(x => x.Disagree).Should().Be(0);
            response.ApprenticeFeedback.FeedbackAttributes.Select(x => x.Name).Should().Contain(source.ApprenticeFeedback.ProviderAttribute.Select(c => c.Name).ToList());
            response.ApprenticeFeedback.FeedbackAttributes.Sum(x => x.Agree).Should().Be(39);
        }


        [Test, AutoData]
        public void Then_Returns_All_Available_Apprentice_Feedback_Attribute_Agreess_And_Disagrees_Where_Agrees_Disagrees_Greater_Than_Zero(InnerApi.Responses.GetProvidersListItem source, string sectorSubjectArea)
        {
            source.ApprenticeFeedback.ProviderAttribute = new List<InnerApi.Responses.GetApprenticeFeedbackAttributeItem>
            {
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "First Attribute",
                    Category = "Category",
                    Agree = 1,
                    Disagree = 1
                },
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "Second Attribute",
                    Category = "Category",
                    Agree = 0,
                    Disagree = 0
                },
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "Third Attribute",
                    Category = "Category",
                    Agree = 1,
                    Disagree = 1
                },
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "Fourth Attribute",
                    Category = "Category",
                    Agree = 0,
                    Disagree = 0
                },
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "Fifth Attribute",
                    Category = "Category",
                    Agree = 1,
                    Disagree = 1
                },
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "Sixth Attribute",
                    Category = "Category",
                    Agree = 0,
                    Disagree = 0
                },
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "Seventh Attribute",
                    Category = "Category",
                    Agree = 1,
                    Disagree = 1
                },
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "Eighth Attribute",
                    Category = "Category",
                    Agree = 0,
                    Disagree = 0
                },
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "Ninth Attribute",
                    Category = "Category",
                    Agree = 0,
                    Disagree = 0
                },
                new InnerApi.Responses.GetApprenticeFeedbackAttributeItem
                {
                    Name = "Tenth Attribute",
                    Category = "Category",
                    Agree = 1,
                    Disagree = 1
                }
            };

            var response = new GetTrainingCourseProviderListItem().Map(source, sectorSubjectArea, 1, new List<DeliveryModeType>(), new List<FeedbackRatingType>(), new List<FeedbackRatingType>(), true);

            response.ApprenticeFeedback.FeedbackAttributes.Select(x => x.Name)
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