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
        [Test, RecursiveMoqAutoData]
        public void Then_If_No_Items_Returned_Then_Null_Returned(MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            var source = new HubCmsContent { Items = new List<HubItem>(), Total = 1 };

            //Act
            var actual = new HubPageModel().Build(source, menuContent, bannerContent);

            //Assert
            actual.Should().BeNull();
        }
        [Test, RecursiveMoqAutoData]
        public void Then_If_Total_Is_Zero_Items_Returned_Then_Null_Returned(HubCmsContent source, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            source.Total = 0;

            //Act
            var actual = new HubPageModel().Build(source, menuContent, bannerContent);

            //Assert
            actual.Should().BeNull();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_No_Content_Items_Returns_Empty_Header_Image_And_Cards(HubCmsContent source, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            source.Items.FirstOrDefault().Fields.HeaderImage = null;

            //Act
            var actual = new HubPageModel().Build(source, menuContent, bannerContent);

            //Assert
            actual.MainContent.HeaderImage.Should().BeNull();
            actual.MainContent.Cards.Should().BeEmpty();
        }

        [Test]
        [RecursiveMoqInlineAutoData("hub", PageType.Hub)]
        [RecursiveMoqInlineAutoData("test", PageType.Unknown)]
        public void Then_The_PageType_Is_Correctly_Set(string pageType, PageType type, HubCmsContent source, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            source.Items.FirstOrDefault().Sys.ContentType.Sys.Id = pageType;

            //Act
            var actual = new HubPageModel().Build(source, menuContent, bannerContent);

            //Assert
            actual.PageAttributes.PageType.Should().Be(type);
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Page_Level_Fields_Are_Set(HubCmsContent source, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Act
            var actual = new HubPageModel().Build(source, menuContent, bannerContent);

            //Assert
            actual.PageAttributes.Title.Should().Be(source.Items.FirstOrDefault()?.Fields.Title);
            actual.PageAttributes.MetaDescription.Should().Be(source.Items.FirstOrDefault()?.Fields.MetaDescription);
            actual.PageAttributes.Slug.Should().Be(source.Items.FirstOrDefault()?.Fields.Slug);
            actual.PageAttributes.HubType.Should().Be(source.Items.FirstOrDefault()?.Fields.HubType);
            actual.PageAttributes.Summary.Should().Be(source.Items.FirstOrDefault()?.Fields.Summary);
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Header_Image_Is_Added(HubCmsContent source, string contentValue, HubAssetFields fields, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            fields.File.Url = $"//{fields.File.Url}";

            source.Includes.Asset = new List<HubAsset>()
            {
                new HubAsset
                {
                    Sys = new HubSys
                    {
                        Id = source.Items[0].Fields.HeaderImage.Sys.Id
                    },
                    Fields = fields
                }
            };

            ////Act
            var actual = new HubPageModel().Build(source, menuContent, bannerContent);

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
        public void Then_The_Cards_Are_Built(HubCmsContent source, HubEntryFields linkedPage, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            source.Items[0].Fields.Cards[0].Sys.Id = "2K5MZPYdhDNyPEsDk4EgZh";
            source.Includes.Entry = new List<HubEntry>
            {
                new HubEntry
                {
                    Sys = new HubSys
                    {
                        Id = "2K5MZPYdhDNyPEsDk4EgZh",
                        ContentType = new HubLink
                        {
                            Sys = new HubLinkSys
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
            var actual = new HubPageModel().Build(source, menuContent, bannerContent);

            //Assert
            actual.MainContent.Cards.Count.Should().Be(1);
            actual.MainContent.Cards.TrueForAll(c => c.Title.Equals(linkedPage.Title)).Should().BeTrue();
            actual.MainContent.Cards.TrueForAll(c => c.Summary.Equals(linkedPage.Summary)).Should().BeTrue();
            actual.MainContent.Cards.TrueForAll(c => c.Slug.Equals(linkedPage.Slug)).Should().BeTrue();
            actual.MainContent.Cards.TrueForAll(c => c.HubType.Equals(linkedPage.HubType)).Should().BeTrue();
            actual.MainContent.Cards.TrueForAll(c => c.MetaDescription.Equals(linkedPage.MetaDescription)).Should().BeTrue();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Cards2_Are_Built(HubCmsContent source, HubEntryFields linkedPage, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            source.Items[0].Fields.Cards2[0].Sys.Id = "2K5MZPYdhDNyPEsDk4EgZh";
            source.Includes.Entry = new List<HubEntry>
            {
                new HubEntry
                {
                    Sys = new HubSys
                    {
                        Id = "2K5MZPYdhDNyPEsDk4EgZh",
                        ContentType = new HubLink
                        {
                            Sys = new HubLinkSys
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
            var actual = new HubPageModel().Build(source, menuContent, bannerContent);

            //Assert
            actual.MainContent.Cards2.Count.Should().Be(1);
            actual.MainContent.Cards2.TrueForAll(c => c.Title.Equals(linkedPage.Title)).Should().BeTrue();
            actual.MainContent.Cards2.TrueForAll(c => c.Summary.Equals(linkedPage.Summary)).Should().BeTrue();
            actual.MainContent.Cards2.TrueForAll(c => c.Slug.Equals(linkedPage.Slug)).Should().BeTrue();
            actual.MainContent.Cards2.TrueForAll(c => c.HubType.Equals(linkedPage.HubType)).Should().BeTrue();
            actual.MainContent.Cards2.TrueForAll(c => c.MetaDescription.Equals(linkedPage.MetaDescription)).Should().BeTrue();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Cards3_Are_Built(HubCmsContent source, HubEntryFields linkedPage, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            source.Items[0].Fields.Cards3[0].Sys.Id = "2K5MZPYdhDNyPEsDk4EgZh";
            source.Includes.Entry = new List<HubEntry>
            {
                new HubEntry
                {
                    Sys = new HubSys
                    {
                        Id = "2K5MZPYdhDNyPEsDk4EgZh",
                        ContentType = new HubLink
                        {
                            Sys = new HubLinkSys
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
            var actual = new HubPageModel().Build(source, menuContent, bannerContent);

            //Assert
            actual.MainContent.Cards3.Count.Should().Be(1);
            actual.MainContent.Cards3.TrueForAll(c => c.Title.Equals(linkedPage.Title)).Should().BeTrue();
            actual.MainContent.Cards3.TrueForAll(c => c.Summary.Equals(linkedPage.Summary)).Should().BeTrue();
            actual.MainContent.Cards3.TrueForAll(c => c.Slug.Equals(linkedPage.Slug)).Should().BeTrue();
            actual.MainContent.Cards3.TrueForAll(c => c.HubType.Equals(linkedPage.HubType)).Should().BeTrue();
            actual.MainContent.Cards3.TrueForAll(c => c.MetaDescription.Equals(linkedPage.MetaDescription)).Should().BeTrue();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Cards_Titles_Are_Set(HubCmsContent source, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Act
            var actual = new HubPageModel().Build(source, menuContent, bannerContent);

            //Assert
            actual.MainContent.CardsTitle.Should().Be(source.Items.FirstOrDefault()?.Fields.CardsTitle);
            actual.MainContent.CardsTitle2.Should().Be(source.Items.FirstOrDefault()?.Fields.CardsTitle2);
            actual.MainContent.CardsTitle3.Should().Be(source.Items.FirstOrDefault()?.Fields.CardsTitle3);
        }

        [Test, RecursiveMoqAutoData]
        public void Then_Null_Card_Collections_Return_Empty_Lists(HubCmsContent source, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            source.Items[0].Fields.Cards2 = null;
            source.Items[0].Fields.Cards3 = null;

            //Act
            var actual = new HubPageModel().Build(source, menuContent, bannerContent);

            //Assert
            actual.MainContent.Cards2.Should().BeEmpty();
            actual.MainContent.Cards3.Should().BeEmpty();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Sections_Are_Built(HubCmsContent source, HubEntryFields sectionFields, HubEntryFields stepperLink, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            sectionFields.CtaPanel = null;
            sectionFields.StandardLinks = null;
            sectionFields.StepperLinks = new List<HubLink>
            {
                new HubLink { Sys = new HubLinkSys { Id = "stepper-link-id", Type = "Link", LinkType = "Entry" } }
            };
            source.Items[0].Fields.Sections = new List<HubLink>
            {
                new HubLink { Sys = new HubLinkSys { Id = "section-id", Type = "Link", LinkType = "Entry" } }
            };
            source.Includes.Entry = new List<HubEntry>
            {
                BuildEntry("section-id", "hubSection", sectionFields),
                BuildEntry("stepper-link-id", "article", stepperLink)
            };

            //Act
            var actual = new HubPageModel().Build(source, menuContent, bannerContent);

            //Assert
            actual.MainContent.Sections.Count.Should().Be(1);
            var section = actual.MainContent.Sections[0];
            section.SectionType.Should().Be(sectionFields.SectionType);
            section.Heading.Should().Be(sectionFields.Heading);
            section.Introduction.Should().Be(sectionFields.Introduction);
            section.StepperLinks.Count.Should().Be(1);
            section.StepperLinks[0].Title.Should().Be(stepperLink.Title);
            section.StepperLinks[0].Slug.Should().Be(stepperLink.Slug);
            section.StepperLinks[0].PageType.Should().Be(PageType.Article);
            section.StandardLinks.Should().BeEmpty();
            section.CtaPanel.Should().BeNull();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Section_Cta_Panel_Is_Built(HubCmsContent source, HubEntryFields sectionFields, HubEntryFields ctaPanel, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            sectionFields.StepperLinks = null;
            sectionFields.StandardLinks = null;
            sectionFields.CtaPanel = new HubLink { Sys = new HubLinkSys { Id = "cta-panel-id", Type = "Link", LinkType = "Entry" } };
            source.Items[0].Fields.Sections = new List<HubLink>
            {
                new HubLink { Sys = new HubLinkSys { Id = "section-id", Type = "Link", LinkType = "Entry" } }
            };
            source.Includes.Entry = new List<HubEntry>
            {
                BuildEntry("section-id", "hubSection", sectionFields),
                BuildEntry("cta-panel-id", "ctaPanel", ctaPanel)
            };

            //Act
            var actual = new HubPageModel().Build(source, menuContent, bannerContent);

            //Assert
            var actualPanel = actual.MainContent.Sections[0].CtaPanel;
            actualPanel.Heading.Should().Be(ctaPanel.Heading);
            actualPanel.Description.Should().Be(ctaPanel.Description);
            actualPanel.Icon.Should().Be(ctaPanel.Icon);
            actualPanel.ButtonText.Should().Be(ctaPanel.ButtonText);
            actualPanel.Url.Should().Be(ctaPanel.Url);
        }

        [Test, RecursiveMoqAutoData]
        public void Then_A_Cta_Panel_In_The_Standard_Links_Is_Built(HubCmsContent source, HubEntryFields sectionFields, HubEntryFields ctaPanel, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            sectionFields.StepperLinks = null;
            sectionFields.CtaPanel = null;
            sectionFields.StandardLinks = new List<HubLink>
            {
                new HubLink { Sys = new HubLinkSys { Id = "cta-panel-id", Type = "Link", LinkType = "Entry" } }
            };
            source.Items[0].Fields.Sections = new List<HubLink>
            {
                new HubLink { Sys = new HubLinkSys { Id = "section-id", Type = "Link", LinkType = "Entry" } }
            };
            source.Includes.Entry = new List<HubEntry>
            {
                BuildEntry("section-id", "hubSection", sectionFields),
                BuildEntry("cta-panel-id", "ctaPanel", ctaPanel)
            };

            //Act
            var actual = new HubPageModel().Build(source, menuContent, bannerContent);

            //Assert
            var actualLink = actual.MainContent.Sections[0].StandardLinks[0];
            actualLink.PageType.Should().Be(PageType.CtaPanel);
            actualLink.CtaPanel.Should().NotBeNull();
            actualLink.CtaPanel.Url.Should().Be(ctaPanel.Url);
        }

        [Test, RecursiveMoqAutoData]
        public void Then_Sections_Linking_To_Other_Content_Types_Are_Ignored(HubCmsContent source, HubEntryFields linkedPage, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            source.Items[0].Fields.Sections = new List<HubLink>
            {
                new HubLink { Sys = new HubLinkSys { Id = "section-id", Type = "Link", LinkType = "Entry" } }
            };
            source.Includes.Entry = new List<HubEntry>
            {
                BuildEntry("section-id", "article", linkedPage)
            };

            //Act
            var actual = new HubPageModel().Build(source, menuContent, bannerContent);

            //Assert
            actual.MainContent.Sections.Should().BeEmpty();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_A_Null_Sections_Collection_Returns_An_Empty_List(HubCmsContent source, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            source.Items[0].Fields.Sections = null;

            //Act
            var actual = new HubPageModel().Build(source, menuContent, bannerContent);

            //Assert
            actual.MainContent.Sections.Should().BeEmpty();
        }

        private static HubEntry BuildEntry(string id, string contentTypeId, HubEntryFields fields)
        {
            return new HubEntry
            {
                Sys = new HubSys
                {
                    Id = id,
                    ContentType = new HubLink
                    {
                        Sys = new HubLinkSys
                        {
                            Id = contentTypeId,
                            LinkType = "ContentType",
                            Type = "Link",
                        }
                    }
                },
                Fields = fields
            };
        }
    }
}