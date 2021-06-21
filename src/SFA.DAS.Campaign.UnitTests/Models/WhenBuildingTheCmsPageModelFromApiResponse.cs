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
        [Test]
        public void Then_If_No_Items_Returned_Then_Null_Returned()
        {
            //Arrange
            var source = new CmsContent {Items = new List<Item>(), Total = 1};
            
            //Act
            var actual = new CmsPageModel().Build(source);
            
            //Assert
            actual.Should().BeNull();
        }
        [Test, RecursiveMoqAutoData]
        public void Then_If_Total_Is_Zero_Items_Returned_Then_Null_Returned(CmsContent source)
        {
            //Arrange
            source.Total = 0;
            
            //Act
            var actual = new CmsPageModel().Build(source);
            
            //Assert
            actual.Should().BeNull();
        }
        
        [Test]
        [RecursiveMoqInlineAutoData("article",PageType.Article )]
        [RecursiveMoqInlineAutoData("landingpage", PageType.LandingPage)]
        [RecursiveMoqInlineAutoData("test", PageType.Unknown)]
        public void Then_The_PageType_Is_Correctly_Set(string pageType, PageType type, CmsContent source)
        {
            //Arrange
            source.Items.FirstOrDefault().Sys.ContentType.Sys.Id = pageType;
            
            //Act
            var actual = new CmsPageModel().Build(source);
            
            //Assert
            actual.PageAttributes.PageType.Should().Be(type);
        }
        
        [Test, RecursiveMoqAutoData]
        public void Then_The_Page_Level_Fields_Are_Set(CmsContent source)
        {
            //Act
            var actual = new CmsPageModel().Build(source);

            //Assert
            actual.PageAttributes.Title.Should().Be(source.Items.FirstOrDefault()?.Fields.PageTitle);
            actual.PageAttributes.MetaDescription.Should().Be(source.Items.FirstOrDefault()?.Fields.MetaDescription);
            actual.PageAttributes.Slug.Should().Be(source.Items.FirstOrDefault()?.Fields.Slug);
            actual.PageAttributes.HubType.Should().Be(source.Items.FirstOrDefault()?.Fields.HubType);
            actual.PageAttributes.Summary.Should().Be(source.Items.FirstOrDefault()?.Fields.Summary);
        }
        
        [Test, RecursiveMoqAutoData]
        public void Then_The_Content_Items_Are_Added_For_Paragraphs(CmsContent source, string contentValue)
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
            var actual = new CmsPageModel().Build(source);
            
            //Assert
            actual.MainContent.Items.TrueForAll(c => c.Type.Equals("paragraph"));
            actual.MainContent.Items.TrueForAll(c => c.Values.FirstOrDefault().Equals(contentValue));
        }
        
        [Test, RecursiveMoqAutoData]
        public void Then_The_Content_Items_Are_Added_For_Headings(CmsContent source, string contentValue)
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
            var actual = new CmsPageModel().Build(source);
            
            //Assert
            actual.MainContent.Items.TrueForAll(c => c.Type.Equals("heading"));
            actual.MainContent.Items.TrueForAll(c => c.Values.FirstOrDefault().Equals(contentValue));
        }
        
        [Test, RecursiveMoqAutoData]
        public void Then_The_Content_Items_Are_Added_For_ListItems(CmsContent source, string contentValue)
        {
            //Arrange
            foreach (var subContentItems in source.Items.FirstOrDefault().Fields.Content.Content)
            {
                subContentItems.NodeType = "unordered-list";
                subContentItems.Content = new List<ContentDefinition>
                {
                    new ContentDefinition
                    {
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
                    },
                    new ContentDefinition
                    {
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
            var actual = new CmsPageModel().Build(source);
            
            //Assert
            actual.MainContent.Items.TrueForAll(c => c.Type.Equals("unordered-list"));
            actual.MainContent.Items.TrueForAll(c => c.Values.Count.Equals(2));
            actual.MainContent.Items.TrueForAll(c => c.Values.TrueForAll(x=>x.Equals(contentValue)));
        }
        
        [Test, RecursiveMoqAutoData]
        public void Then_The_Content_Items_Are_Added_For_Links(CmsContent source, string contentValue, string uri)
        {
            //Arrange
            var expectedUri = $"https://{uri}";
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
            var actual = new CmsPageModel().Build(source);
            
            //Assert
            actual.MainContent.Items.TrueForAll(c => c.Type.Equals("paragraph"));
            actual.MainContent.Items.TrueForAll(c => c.Values.FirstOrDefault().Equals($"[{contentValue}]({expectedUri})"));
        }
        
        [Test, RecursiveMoqAutoData]
        public void Then_The_Related_Articles_Are_Built(CmsContent source, EntryFields linkedPage)
        {
            //Arrange
            source.Includes.Entry = new List<Entry>
            {
                new Entry
                {
                    Sys = new AssetSys
                    {
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
            var actual = new CmsPageModel().Build(source);
            
            //Assert
            actual.RelatedArticles.Count.Should().Be(1);
            actual.RelatedArticles.TrueForAll(c => c.Title.Equals(linkedPage.Title));
            actual.RelatedArticles.TrueForAll(c => c.Summary.Equals(linkedPage.Summary));
            actual.RelatedArticles.TrueForAll(c => c.Slug.Equals(linkedPage.Slug));
            actual.RelatedArticles.TrueForAll(c => c.HubType.Equals(linkedPage.HubType));
            actual.RelatedArticles.TrueForAll(c => c.MetaDescription.Equals(linkedPage.MetaDescription));
        }


        [Test, RecursiveMoqAutoData]
        public void Then_Any_Linked_Types_Are_Added_To_The_Content_Items(CmsContent source,string contentValue, string linkedContentId, List<List<string>> tableData)
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
            var actual = new CmsPageModel().Build(source);
            
            //Assert
            actual.MainContent.Items.TrueForAll(c => c.Type.Equals("paragraph"));
            actual.MainContent.Items.FirstOrDefault().TableValue.Should().BeEquivalentTo(tableData);
        }
    }
}