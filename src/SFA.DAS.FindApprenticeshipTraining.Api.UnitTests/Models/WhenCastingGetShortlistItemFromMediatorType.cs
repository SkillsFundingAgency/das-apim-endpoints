using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using GetShortlistItem = SFA.DAS.FindApprenticeshipTraining.Api.Models.GetShortlistItem;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingGetShortlistItemFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            InnerApi.Responses.GetShortlistItem source)
        {
            var expectedProvider = new GetProviderCourseItem().Map(source);

            var response = (GetShortlistItem)source;

            response.Id.Should().Be(source.Id);
            response.ShortlistUserId.Should().Be(source.ShortlistUserId);
            response.Provider.Should().BeEquivalentTo(expectedProvider);
            response.Course.Should().BeEquivalentTo((GetTrainingCourseListItem)source.Course);
            response.LocationDescription.Should().Be(source.LocationDescription);
            response.CreatedDate.Should().Be(source.CreatedDate);
        }
    }
}