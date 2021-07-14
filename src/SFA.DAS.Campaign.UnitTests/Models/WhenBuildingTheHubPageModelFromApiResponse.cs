using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Models
{
    public class WhenBuildingTheHubPageModelFromApiResponse
    {
        [Test]
        public void Then_If_No_Items_Returned_Then_Null_Returned()
        {
            //Arrange
            var source = new CmsContent { Items = new List<Item>(), Total = 1 };

            //Act
            var actual = new HubPageModel().Build(source);

            //Assert
            actual.Should().BeNull();
        }
        [Test, RecursiveMoqAutoData]
        public void Then_If_Total_Is_Zero_Items_Returned_Then_Null_Returned(CmsContent source)
        {
            //Arrange
            source.Total = 0;

            //Act
            var actual = new HubPageModel().Build(source);

            //Assert
            actual.Should().BeNull();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_No_Content_Items_Returns_Empty_Header_Image_And_Cards(CmsContent source)
        {
            //Arrange
            source.Items.FirstOrDefault().Fields.HeaderImage = null;

            //Act
            var actual = new HubPageModel().Build(source);

            //Assert
            actual.MainContent.HeaderImage.Should().BeNull();
            actual.MainContent.Cards.Should().BeEmpty();
        }

        [Test]
        [RecursiveMoqInlineAutoData("hub", PageType.Hub)]
        [RecursiveMoqInlineAutoData("test", PageType.Unknown)]
        public void Then_The_PageType_Is_Correctly_Set(string pageType, PageType type, CmsContent source)
        {
            //Arrange
            source.Items.FirstOrDefault().Sys.ContentType.Sys.Id = pageType;

            //Act
            var actual = new HubPageModel().Build(source);

            //Assert
            actual.PageAttributes.PageType.Should().Be(type);
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Page_Level_Fields_Are_Set(CmsContent source)
        {
            //Act
            var actual = new HubPageModel().Build(source);

            //Assert
            actual.PageAttributes.Title.Should().Be(source.Items.FirstOrDefault()?.Fields.Title);
            actual.PageAttributes.MetaDescription.Should().Be(source.Items.FirstOrDefault()?.Fields.MetaDescription);
            actual.PageAttributes.Slug.Should().Be(source.Items.FirstOrDefault()?.Fields.Slug);
            actual.PageAttributes.HubType.Should().Be(source.Items.FirstOrDefault()?.Fields.HubType);
            actual.PageAttributes.Summary.Should().Be(source.Items.FirstOrDefault()?.Fields.Summary);
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Header_Image_Is_Added(CmsContent source, string contentValue, AssetFields fields)
        {
            //Arrange
            fields.File.Url = $"//{fields.File.Url}";

            source.Includes.Asset = new List<Asset>()
            {
                new Asset
                {
                    Sys = new AssetSys
                    {
                        Id = source.Items[0].Fields.HeaderImage.Sys.Id
                    },
                    Fields = fields
                }
            };

            ////Act
            var actual = new HubPageModel().Build(source);

            //Assert
            actual.MainContent.HeaderImage.Should().NotBeNull();
            actual.MainContent.HeaderImage.EmbeddedResource.Id.Should().Be(source.Items[0].Fields.HeaderImage.Sys.Id);
            actual.MainContent.HeaderImage.EmbeddedResource.Title.Should().Be(fields.Title);
            actual.MainContent.HeaderImage.EmbeddedResource.FileName.Should().Be(fields.File.FileName);
            actual.MainContent.HeaderImage.EmbeddedResource.Url.Should().Be($"https:{fields.File.Url}");
            actual.MainContent.HeaderImage.EmbeddedResource.ContentType.Should().Be(fields.File.ContentType);
            actual.MainContent.HeaderImage.EmbeddedResource.Size.Should().Be(fields.File.Details.Size);
            actual.MainContent.HeaderImage.EmbeddedResource.Description.Should().Be(fields.Description);

        }


        [Test, RecursiveMoqAutoData]
        public void Then_The_Cards_Are_Built(CmsContent source, EntryFields linkedPage)
        {
            //Arrange
            source.Items[0].Fields.Cards[0].Sys.Id = "2K5MZPYdhDNyPEsDk4EgZh";
            source.Includes.Entry = new List<Entry>
            {
                new Entry
                {
                    Sys = new AssetSys
                    {
                        Id = "2K5MZPYdhDNyPEsDk4EgZh",
                        Space = new LandingPage
                        {
                            Sys = new LandingPageSys
                            {
                                Id = "123abc",
                                Type = "Link",
                                LinkType = "Space"
                            }
                        },
                        ContentType = new LandingPage
                        {
                            Sys = new LandingPageSys
                            {
                                Id = "article",
                                LinkType = "ContentType",
                                Type = "Link",
                            }
                        }
                    },
                    Fields = linkedPage
                }
            };

            //Act
            var actual = new HubPageModel().Build(source);

            //Assert
            actual.MainContent.Cards.Count.Should().Be(1);
            actual.MainContent.Cards.TrueForAll(c => c.Title.Equals(linkedPage.Title)).Should().BeTrue();
            actual.MainContent.Cards.TrueForAll(c => c.Summary.Equals(linkedPage.Summary)).Should().BeTrue();
            actual.MainContent.Cards.TrueForAll(c => c.Slug.Equals(linkedPage.Slug)).Should().BeTrue();
            actual.MainContent.Cards.TrueForAll(c => c.HubType.Equals(linkedPage.HubType)).Should().BeTrue();
            actual.MainContent.Cards.TrueForAll(c => c.MetaDescription.Equals(linkedPage.MetaDescription)).Should().BeTrue();
        }
    }
}