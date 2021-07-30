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
        public void Then_The_Banner_Is_Built(CmsContent source, EntryFields linkedPage)
        {
            var actual = new BannerPageModel().Build(source);

            //actual.MainContent.Should().NotBeNullOrEmpty();
            //actual.MainContent[0].Title.Should().NotBeNullOrWhiteSpace();
            //actual.MainContent[0].Hub.Should().NotBeNullOrWhiteSpace();
            //actual.MainContent[0].Slug.Should().NotBeNullOrWhiteSpace();
        }
    }
}