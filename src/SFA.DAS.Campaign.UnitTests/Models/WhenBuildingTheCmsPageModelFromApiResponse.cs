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
    public class WhenBuildingTheCmsPageModelFromApiResponse
    {
        [Test, RecursiveMoqAutoData]
        public void Then_If_No_Items_Returned_Then_Null_Returned(MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            var source = new CmsContent {Items = new List<Item>(), Total = 1};

            //Act
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);
            
            //Assert
            actual.Should().BeNull();
        }
        [Test, RecursiveMoqAutoData]
        public void Then_If_Total_Is_Zero_Items_Returned_Then_Null_Returned(CmsContent source, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            source.Total = 0;
            
            //Act
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);
            
            //Assert
            actual.Should().BeNull();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_No_Content_Items_Returns_Empty_List(CmsContent source, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            source.Items.FirstOrDefault().Fields.Content = null;
            
            //Act
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);
            
            //Assert
            actual.MainContent.Items.Should().BeEmpty();
        }
        
        [Test]
        [RecursiveMoqInlineAutoData("article", PageType.Article )]
        [RecursiveMoqInlineAutoData("landingpage", PageType.LandingPage)]
        [RecursiveMoqInlineAutoData("test", PageType.Unknown)]
        public void Then_The_PageType_Is_Correctly_Set(string pageType, PageType type, CmsContent source, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            source.Items.FirstOrDefault().Sys.ContentType.Sys.Id = pageType;
            
            //Act
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);
            
            //Assert
            actual.PageAttributes.PageType.Should().Be(type);
        }
        
        [Test, RecursiveMoqAutoData]
        public void Then_The_Page_Level_Fields_Are_Set(CmsContent source, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Act
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);

            //Assert
            actual.PageAttributes.Title.Should().Be(source.Items.FirstOrDefault()?.Fields.Title);
            actual.PageAttributes.MetaDescription.Should().Be(source.Items.FirstOrDefault()?.Fields.MetaDescription);
            actual.PageAttributes.Slug.Should().Be(source.Items.FirstOrDefault()?.Fields.Slug);
            actual.PageAttributes.HubType.Should().Be(source.Items.FirstOrDefault()?.Fields.HubType);
            actual.PageAttributes.Summary.Should().Be(source.Items.FirstOrDefault()?.Fields.Summary);
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Parent_Page_Is_Set(CmsContent source, string parentId, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            source.Items.FirstOrDefault().Fields.LandingPage.Sys.Id = parentId;
            source.Includes.Entry.FirstOrDefault().Sys.Id = parentId;
            
            //Act
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);
            
            //Assert
            actual.ParentPage.Title.Should().Be(source.Includes.Entry.FirstOrDefault()?.Fields.Title);
            actual.ParentPage.MetaDescription.Should().Be(source.Includes.Entry.FirstOrDefault()?.Fields.MetaDescription);
            actual.ParentPage.Slug.Should().Be(source.Includes.Entry.FirstOrDefault()?.Fields.Slug);
            actual.ParentPage.HubType.Should().Be(source.Includes.Entry.FirstOrDefault()?.Fields.HubType);
            actual.ParentPage.Summary.Should().Be(source.Includes.Entry.FirstOrDefault()?.Fields.Summary);
        }
        
        [Test, RecursiveMoqAutoData]
        public void Then_If_No_Parent_Page_Is_Set_Then_Null_Returned_For_Parent_Page(CmsContent source, string parentId, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            source.Items.FirstOrDefault().Fields.LandingPage = null;
            
            
            //Act
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);
            
            //Assert
            actual.ParentPage.Should().BeNull();
        }
        
        [Test, RecursiveMoqAutoData]
        public void Then_The_Content_Items_Are_Added_For_Paragraphs(CmsContent source, string contentValue, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
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
            
            //Act
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);
            
            //Assert
            actual.MainContent.Items.TrueForAll(c => c.Type.Equals("paragraph")).Should().BeTrue();
            actual.MainContent.Items.TrueForAll(c => c.Values.FirstOrDefault().Equals(contentValue)).Should().BeTrue();
        }
        
        [Test, RecursiveMoqAutoData]
        public void Then_The_Content_Items_Are_Added_For_Headings(CmsContent source, string contentValue, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            foreach (var subContentItems in source.Items.FirstOrDefault().Fields.Content.Content)
            {
                subContentItems.NodeType = "heading";
                subContentItems.Content = new List<ContentDefinition>
                {
                    new ContentDefinition
                    {
                        NodeType = "text",
                        Value = contentValue
                    }
                };
            }
            
            //Act
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);
            
            //Assert
            actual.MainContent.Items.TrueForAll(c => c.Type.Equals("heading")).Should().BeTrue();
            actual.MainContent.Items.TrueForAll(c => c.Values.FirstOrDefault().Equals(contentValue)).Should().BeTrue();
        }
         
        [Test, RecursiveMoqAutoData]
        public void Then_The_Content_Items_Are_Added_For_Block_Quotes(CmsContent source, string contentValue, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            foreach (var subContentItems in source.Items.FirstOrDefault().Fields.Content.Content)
            {
                subContentItems.NodeType = "blockquote";
                subContentItems.Content = new List<ContentDefinition>
                {
                    new ContentDefinition
                    {
                        NodeType = "paragraph",
                        Content = new List<RelatedContent>
                        {
                            new RelatedContent
                            {
                                NodeType = "text",
                                Value = contentValue
                            }
                        }
                    }
                };
            }
            
            //Act
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);
            
            //Assert
            actual.MainContent.Items.Should().NotBeEmpty();
            actual.MainContent.Items.TrueForAll(c => c.Type.Equals("blockquote")).Should().BeTrue();
            actual.MainContent.Items.TrueForAll(c => c.Values.FirstOrDefault().Equals(contentValue)).Should().BeTrue();
        }
        
        [Test, RecursiveMoqAutoData]
        public void Then_The_Content_Items_Are_Added_For_Hr_Content_Type(CmsContent source, string contentValue, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            foreach (var subContentItems in source.Items.FirstOrDefault().Fields.Content.Content)
            {
                subContentItems.NodeType = "hr";
                subContentItems.Content = new List<ContentDefinition>();
            }
            
            //Act
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);
            
            //Assert
            actual.MainContent.Items.Should().NotBeEmpty();
            actual.MainContent.Items.TrueForAll(c => c.Type.Equals("hr")).Should().BeTrue();
            actual.MainContent.Items.TrueForAll(c => c.Values.Count.Equals(0)).Should().BeTrue();
        }
        
        [Test]
        [RecursiveMoqInlineAutoData("unordered-list")]
        [RecursiveMoqInlineAutoData("ordered-list")]
        public void Then_The_Content_Items_Are_Added_For_ListItems_with_Styling(string listType, CmsContent source, string contentValue, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            foreach (var subContentItems in source.Items.FirstOrDefault().Fields.Content.Content)
            {
                subContentItems.NodeType = listType;
                subContentItems.Content = new List<ContentDefinition>
                {
                    new ContentDefinition
                    {
                        NodeType = "list-item",
                        Content = new List<RelatedContent>
                        {
                            new RelatedContent
                            {
                                NodeType = "paragraph",
                                Content = new List<RelatedContent>
                                {
                                    new RelatedContent
                                    {
                                        NodeType = "text",
                                        Value = contentValue,
                                        Marks = new List<SysElement>
                                        {
                                            new SysElement
                                            {
                                                Type = "Bold"
                                            }
                                        }
                                    }
                                }
                            },
                            new RelatedContent
                            {
                                NodeType = "text",
                                Value = contentValue,
                                Marks = new List<SysElement>
                                {
                                    new SysElement
                                    {
                                        Type = "Italic"
                                    }
                                }
                            }
                        }
                    }
                };
            }
            
            //Act
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);
            
            //Assert
            actual.MainContent.Items[0].Type.Should().Be(listType);
            actual.MainContent.Items[0].TableValue[0].FirstOrDefault().Should().Be($"[Bold]{contentValue}");
            actual.MainContent.Items[0].TableValue[1].FirstOrDefault().Should().Be($"[Italic]{contentValue}");
            actual.MainContent.Items[0].TableValue.Count.Should().Be(2);
        }

        
        [Test, RecursiveMoqAutoData]
        public void Then_The_Embedded_Items_Are_Added(CmsContent source, string contentValue, string linkedContentId, AssetFields fields, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            fields.File.Url = $"//{fields.File.Url}"; 
            source.Items.FirstOrDefault().Fields.Content.Content = new List<SubContentItems>
            {
                new SubContentItems
                {
                    NodeType = "embedded-asset-block",
                    Content = new List<ContentDefinition>(),
                    Data = new PurpleData
                    {
                        Target = new LandingPage
                        {
                            Sys = new LandingPageSys
                            {
                                Id = linkedContentId,
                                LinkType = "Asset"
                            } 
                                
                        }
                    }
                }
            };
            source.Includes.Asset = new List<Asset>()
            {
                new Asset
                {
                    Sys = new AssetSys
                    {
                        Id = linkedContentId,
                    },
                    Fields = fields
                }
            };
            
            //Act
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);
            
            //Assert
            actual.MainContent.Items[0].Type.Should().Be("embedded-asset-block");
            actual.MainContent.Items[0].EmbeddedResource.Id.Should().Be(linkedContentId);
            actual.MainContent.Items[0].EmbeddedResource.Title.Should().Be(fields.Title);
            actual.MainContent.Items[0].EmbeddedResource.FileName.Should().Be(fields.File.FileName);
            actual.MainContent.Items[0].EmbeddedResource.Url.Should().Be($"https:{fields.File.Url}");
            actual.MainContent.Items[0].EmbeddedResource.ContentType.Should().Be(fields.File.ContentType);
            actual.MainContent.Items[0].EmbeddedResource.Size.Should().Be(fields.File.Details.Size);
            actual.MainContent.Items[0].EmbeddedResource.Description.Should().Be(fields.Description);
            
        }
        
        [Test, RecursiveMoqAutoData]
        public void Then_If_Content_Contains_HyperLink_The_Content_Items_Are_Added_For_ListItems(CmsContent source, string contentValue, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            foreach (var subContentItems in source.Items.FirstOrDefault().Fields.Content.Content)
            {
                subContentItems.NodeType = "unordered-list";
                subContentItems.Content = new List<ContentDefinition>
                {
                    new ContentDefinition
                    {
                        NodeType = "list-item",
                        Content = new List<RelatedContent>
                        {
                            new RelatedContent
                            {
                                NodeType = "paragraph",
                                Content = new List<RelatedContent>
                                {
                                    new RelatedContent
                                    {
                                        NodeType = "hyperlink",
                                        Content = new List<RelatedContent>
                                        {
                                            new RelatedContent
                                            {
                                                NodeType = "text",
                                                Value = "find"
                                            }
                                        },
                                        Data = new RelatedData{ Uri = new Uri("http://www.google.com")}
                                    },
                                    new RelatedContent
                                    {
                                        NodeType = "text",
                                        Value = " a website"
                                    }
                                }
                            }
                        }
                    },
                    new ContentDefinition
                    {
                        NodeType = "list-item",
                        Content = new List<RelatedContent>
                        {
                            new RelatedContent
                            {
                                Content = new List<RelatedContent>
                                {
                                    new RelatedContent
                                    {
                                        Value = contentValue
                                    }
                                }
                            }
                        }
                    }
                };
            }

            //Act
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);

            //Assert
            actual.MainContent.Items.TrueForAll(c => c.Type.Equals("unordered-list")).Should().BeTrue();
            actual.MainContent.Items[2].TableValue[0].Count.Should().Be(2);
            actual.MainContent.Items[2].TableValue[0].FirstOrDefault().Should().Be("[find](http://www.google.com/)");
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Content_Items_Are_Added_For_Links(CmsContent source, string contentValue, string uri, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            var expectedUri = $"https://{uri}/";
            foreach (var subContentItems in source.Items.FirstOrDefault().Fields.Content.Content)
            {
                subContentItems.NodeType = "paragraph";
                subContentItems.Content = new List<ContentDefinition>
                {
                    new ContentDefinition
                    {
                        NodeType = "hyperlink",
                        Content = new List<RelatedContent>
                        {
                            new RelatedContent
                            {
                                Value = contentValue,       
                            }
                        },
                        Data = new RelatedData
                        {
                            Uri = new Uri(expectedUri)
                        }
                    }
                };
            }
            
            //Act
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);
            
            //Assert
            actual.MainContent.Items.TrueForAll(c => c.Type.Equals("paragraph")).Should().BeTrue();
            actual.MainContent.Items.TrueForAll(c => c.Values.FirstOrDefault().ToString() == $"[{contentValue}]({expectedUri})").Should().BeTrue();
        }
        
        [Test, RecursiveMoqAutoData]
        public void Then_The_Related_Articles_Are_Built(CmsContent source, EntryFields linkedPage, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            source.Includes.Entry = new List<Entry>
            {
                new Entry
                {
                    Sys = new AssetSys
                    {
                        Id="321EDF",
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
                                Id = "landingPage",
                                LinkType = "ContentType",
                                Type = "Link",
                            }
                        }
                    },
                    Fields = linkedPage
                },
                new Entry
                {
                    Sys = new AssetSys
                    {
                        Id="321EDC",
                        Space = new LandingPage
                        {
                            Sys = new LandingPageSys
                            {
                                Id = "123abc",
                                Type = "Link",
                                LinkType = "Space",
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
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);
            
            //Assert
            actual.RelatedArticles.Count.Should().Be(1);
            actual.RelatedArticles.TrueForAll(c => c.Title.Equals(linkedPage.Title)).Should().BeTrue();
            actual.RelatedArticles.TrueForAll(c => c.Summary.Equals(linkedPage.Summary)).Should().BeTrue();
            actual.RelatedArticles.TrueForAll(c => c.Slug.Equals(linkedPage.Slug)).Should().BeTrue();
            actual.RelatedArticles.TrueForAll(c => c.HubType.Equals(linkedPage.HubType)).Should().BeTrue();
            actual.RelatedArticles.TrueForAll(c => c.MetaDescription.Equals(linkedPage.MetaDescription)).Should().BeTrue();
        }

        
        [Test, RecursiveMoqAutoData]
        public void Then_The_Attachments_Are_Built(CmsContent source, EntryFields linkedPage, AssetFields fields, string linkedContentId, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            fields.File.Url = $"//{fields.File.Url}";
            source.Items.FirstOrDefault().Fields.Attachments = new List<LandingPage>
            {
                new LandingPage
                {
                    Sys = new LandingPageSys
                    {
                        Id = linkedContentId,
                        Type = "Link",
                        LinkType = "Asset"
                    }
                }
            };
            source.Includes.Asset = new List<Asset>()
            {
                new Asset
                {
                    Sys = new AssetSys
                    {
                        Id = linkedContentId,
                    },
                    Fields = fields
                }
            };
            
            //Act
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);
            
            //Assert
            actual.Attachments.Count.Should().Be(1);
            actual.Attachments[0].Id.Should().Be(linkedContentId);
            actual.Attachments[0].Title.Should().Be(fields.Title);
            actual.Attachments[0].Url.Should().Be($"https:{fields.File.Url}");
            actual.Attachments[0].ContentType.Should().Be(fields.File.ContentType);
            actual.Attachments[0].FileName.Should().Be(fields.File.FileName);
            actual.Attachments[0].Size.Should().Be(fields.File.Details.Size);
            actual.Attachments[0].Description.Should().Be(fields.Description);
        }

        [Test, RecursiveMoqAutoData]
        public void Then_Any_Linked_Types_Are_Added_To_The_Content_Items(CmsContent source,string contentValue, string linkedContentId, List<List<string>> tableData, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            //Arrange
            source.Items.FirstOrDefault().Fields.Content.Content = new List<SubContentItems>
            {
                new SubContentItems
                {
                    NodeType = "paragraph",
                    Content = new List<ContentDefinition>
                    {
                        new ContentDefinition
                        {
                            Content = new List<RelatedContent>(),
                            Data = new RelatedData
                            {
                                Target = new LandingPage
                                {
                                    Sys = new LandingPageSys
                                    {
                                        Id = linkedContentId 
                                    }
                                }
                            },
                            NodeType = "embedded-entry-inline"
                        }
                    }
                }
            };
            source.Includes.Entry = new List<Entry>
            {
                new Entry
                {
                    Sys = new AssetSys
                    {
                        Id = linkedContentId,
                        
                    },
                    Fields = new EntryFields
                    {
                        Table = new Table
                        {
                            TableData = tableData
                        }
                    }
                }
            };
            
            //Act
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);
            
            //Assert
            actual.MainContent.Items.TrueForAll(c => c.Type.Equals("paragraph")).Should().BeTrue();
            actual.MainContent.Items.FirstOrDefault().TableValue.Should().BeEquivalentTo(tableData);
        }

        [Test, RecursiveMoqAutoData]
        public void Then_Any_Tabbed_Content_Is_Added_To_The_Content_Items(CmsContent source, EntryFields linkedPage, MenuPageModel.MenuPageContent menuContent, BannerPageModel bannerContent)
        {
            source.Items.FirstOrDefault().Fields.TabbedContents[0].Sys.Id = "321EDF";
            source.Items.FirstOrDefault().Fields.TabbedContents[1].Sys.Id = "321EDC";
            //Arrange
            source.Includes.Entry = new List<Entry>
            {
                new Entry
                {
                    Sys = new AssetSys
                    {
                        Id="321EDF",
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
                                Id = "tab",
                                LinkType = "ContentType",
                                Type = "Link",
                            }
                        }
                    },
                    Fields = linkedPage
                },
                new Entry
                {
                    Sys = new AssetSys
                    {
                        Id="321EDC",
                        Space = new LandingPage
                        {
                            Sys = new LandingPageSys
                            {
                                Id = "123abc",
                                Type = "Link",
                                LinkType = "Space",
                            }
                        },
                        ContentType = new LandingPage
                        {
                            Sys = new LandingPageSys
                            {
                                Id = "tab",
                                LinkType = "ContentType",
                                Type = "Link",
                            }
                        }
                    },
                    Fields = linkedPage
                }
            };

            //Act
            var actual = new CmsPageModel().Build(source, menuContent, bannerContent);

            //Assert
            actual.TabbedContents.Any().Should().BeTrue();
            actual.TabbedContents.FirstOrDefault().TabTitle.Should().NotBeNullOrWhiteSpace();
            actual.TabbedContents.FirstOrDefault().TabName.Should().NotBeNullOrWhiteSpace();
        }
    }
}