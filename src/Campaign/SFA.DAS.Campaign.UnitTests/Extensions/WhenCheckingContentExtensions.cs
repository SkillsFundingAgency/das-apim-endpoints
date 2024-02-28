using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Responses;

namespace SFA.DAS.Campaign.UnitTests.Extensions
{
    public class WhenContentExtensionsMethodAreGiven
    {
        [Test]
        public void
            A_CmsContent_Object_Is_Passed_To_ContentItemsAreNullOrEmpty_And_The_Total_Is_Zero_Then_Returns_True()
        {
            var ci = new CmsContent {Total = 0};

            var actual = ci.ContentItemsAreNullOrEmpty();

            actual.Should().BeTrue();
        }

        [Test]
        public void Then_If_The_CmsContent_Is_Null_Then_True_Returned()
        {
            CmsContent ci = null;

            var actual = ci.ContentItemsAreNullOrEmpty();

            actual.Should().BeTrue();
        }

        [Test]
        public void
            A_CmsContent_Object_Is_Passed_To_ContentItemsAreNullOrEmpty_And_The_Total_Is_Greater_Than_Zero_Then_Returns_False()
        {
            var ci = new CmsContent
            {
                Total = 1, Items = new List<Item>
                {
                    new Item
                    {
                        Fields = new ItemFields
                        {
                            Summary = "summary"
                        }
                    }
                }
            };

            var actual = ci.ContentItemsAreNullOrEmpty();

            actual.Should().BeFalse();
        }

        [TestCase(ContentExtensions.ParagraphNodeTypeKey, true)]
        [TestCase(ContentExtensions.BlockQuoteNodeTypeKey, true)]
        [TestCase(ContentExtensions.HorizontalRuleNodeTypeKey, true)]
        [TestCase(ContentExtensions.HeadingNodeTypeKey, true)]
        [TestCase(ContentExtensions.EmbeddedEntryInlineNodeTypeKey, false)]
        public void And_A_NodeType_Satisfies_The_Condition_Then_Is_NodeTypeIsContent_Returns_True(string nodeTypeValue,
            bool expected)
        {
            var actual = nodeTypeValue.NodeTypeIsContent();

            actual.Should().Be(expected);
        }

        [TestCase(ContentExtensions.UnorderedListNodeTypeKey, true)]
        [TestCase(ContentExtensions.OrderedListNodeTypeKey, true)]
        [TestCase(ContentExtensions.EmbeddedEntryInlineNodeTypeKey, false)]
        public void And_A_NodeType_Satisfies_The_Condition_Then_NodeTypeIsList_ReturnsTrue(string nodeTypeValue,
            bool expected)
        {
            var actual = nodeTypeValue.NodeTypeIsList();

            actual.Should().Be(expected);

        }
    }
}
