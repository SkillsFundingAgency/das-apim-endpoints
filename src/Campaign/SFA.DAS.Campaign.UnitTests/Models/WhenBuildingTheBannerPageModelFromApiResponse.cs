using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Models
{
    public class WhenBuildingTheBannerPageModelFromApiResponse
    {
        [Test]
        public void Then_If_No_Items_Returned_Then_Null_Returned()
        {
            //Arrange
            var source = new CmsContent { Items = new List<Item>(), Total = 1 };

            //Act
            var actual = new BannerPageModel().Build(source);

            //Assert
            actual.Should().BeNull();
        }
        [Test, RecursiveMoqAutoData]
        public void Then_If_Total_Is_Zero_Items_Returned_Then_Null_Returned(CmsContent source)
        {
            //Arrange
            source.Total = 0;

            //Act
            var actual = new BannerPageModel().Build(source);

            //Assert
            actual.Should().BeNull();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Banner_Is_Built(CmsContent source, string contentValue)
        {
            source.Items[0].Fields.AllowUserToHideTheBanner = true;
            source.Items[0].Fields.ShowOnTheHomepageOnly = true;

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

            var actual = new BannerPageModel().Build(source);

            actual.MainContent.Should().NotBeNullOrEmpty();
            actual.MainContent.ElementAt(0).Title.Should().NotBeNullOrWhiteSpace();
            actual.MainContent.ElementAt(0).AllowUserToHideTheBanner.Should().BeTrue();
            actual.MainContent.ElementAt(0).BackgroundColour.Should().NotBeNullOrWhiteSpace();
            actual.MainContent.ElementAt(0).ShowOnTheHomepageOnly.Should().BeTrue();
            actual.MainContent.ElementAt(0).Items.Any().Should().BeTrue();
        }
    }
}