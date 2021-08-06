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
    public class WhenBuildingTheMenuPageModelFromApiResponse
    {
        [Test]
        public void Then_If_No_Items_Returned_Then_Null_Returned()
        {
            //Arrange
            var source = new CmsContent { Items = new List<Item>(), Total = 1 };

            //Act
            var actual = new MenuPageModel().Build(source);

            //Assert
            actual.Should().BeNull();
        }
        [Test, RecursiveMoqAutoData]
        public void Then_If_Total_Is_Zero_Items_Returned_Then_Null_Returned(CmsContent source)
        {
            //Arrange
            source.Total = 0;

            //Act
            var actual = new MenuPageModel().Build(source);

            //Assert
            actual.Should().BeNull();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Urls_Are_Built(CmsContent source, EntryFields linkedPage)
        {
            source.Items[0].Fields.MenuItems[0].Sys.Id = "2K5MZPYdhDNyPEsDk4EgZh";

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

            var actual = new MenuPageModel().Build(source);

            actual.MainContent.Should().NotBeNullOrEmpty();
            actual.MainContent[0].Title.Should().NotBeNullOrWhiteSpace();
            actual.MainContent[0].Hub.Should().NotBeNullOrWhiteSpace();
            actual.MainContent[0].Slug.Should().NotBeNullOrWhiteSpace();
        }
    }
}