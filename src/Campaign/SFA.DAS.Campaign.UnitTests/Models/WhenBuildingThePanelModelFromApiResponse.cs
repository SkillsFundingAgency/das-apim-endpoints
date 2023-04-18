using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Campaign.UnitTests.Models
{
    public class WhenBuildingThePanelModelFromApiResponse
    {
        [Test]
        public void Then_If_No_Items_Returned_Then_Null_Returned()
        {
            var source = new CmsContent { Items = new List<Item>(), Total = 1 };

            var actual = new PanelModel().Build(source);

            actual.Should().BeNull();
        }
        [Test, RecursiveMoqAutoData]
        public void Then_If_Total_Is_Zero_Items_Returned_Then_Null_Returned(CmsContent source)
        {
            source.Total = 0;

            var actual = new PanelModel().Build(source);

            actual.Should().BeNull();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Panel_Is_Built(CmsContent source, string contentValue, string linkTitle)
        {
            source.Items[0].Fields.LinkTitle = linkTitle;

            foreach (var subContentItems in source.Items.FirstOrDefault().Fields.Content.Content)
            {
                subContentItems.NodeType = "paragraph";
                subContentItems.Content = new List<ContentDefinition>
                {
                    new ContentDefinition
                    {
                        NodeType = "text",
                        Value = contentValue
                    }
                };
            }

            var actual = new PanelModel().Build(source);

            actual.MainContent.Should().NotBeNull();
            actual.MainContent.Title.Should().NotBeNullOrWhiteSpace();
            actual.MainContent.Slug.Should().NotBeNullOrWhiteSpace();
            actual.MainContent.Items.Any().Should().BeTrue();
            actual.MainContent.Image.Should().NotBeNull();
            actual.MainContent.Button.Should().NotBeNull();
            actual.MainContent.LinkTitle.Should().NotBeNullOrWhiteSpace();
            actual.MainContent.Id.Should().NotBe(null);
        }
    }
}
