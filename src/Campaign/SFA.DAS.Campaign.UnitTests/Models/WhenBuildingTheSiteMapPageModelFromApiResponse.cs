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
    public class WhenBuildingTheSiteMapPageModelFromApiResponse
    {
        [Test]
        public void Then_If_No_Items_Returned_Then_Null_Returned()
        {
            //Arrange
            var source = new CmsContent { Items = new List<Item>(), Total = 1 };

            //Act
            var actual = new SiteMapPageModel().Build(source);

            //Assert
            actual.Should().BeNull();
        }
        [Test, RecursiveMoqAutoData]
        public void Then_If_Total_Is_Zero_Items_Returned_Then_Null_Returned(CmsContent source)
        {
            //Arrange
            source.Total = 0;

            //Act
            var actual = new SiteMapPageModel().Build(source);

            //Assert
            actual.Should().BeNull();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_The_Urls_Are_Built(CmsContent source)
        {
            var actual = new SiteMapPageModel().Build(source);

            actual.MainContent.Pages.Should().NotBeNullOrEmpty();
            actual.MainContent.Pages[0].Title.Should().NotBeNullOrWhiteSpace();
            actual.MainContent.Pages[0].Hub.Should().NotBeNullOrWhiteSpace();
            actual.MainContent.Pages[0].Slug.Should().NotBeNullOrWhiteSpace();
        }
    }
}