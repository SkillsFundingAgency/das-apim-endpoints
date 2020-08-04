using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingGetProviderCourseItemFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetProviderStandardItem providerStandardItem)
        {
            var actual = (GetProviderCourseItem) providerStandardItem;
            
            actual.Should().BeEquivalentTo(providerStandardItem, options => options
                .Excluding(c=>c.ContactUrl)
                .Excluding(c=>c.StandardId)
            );

            actual.Website.Should().Be(providerStandardItem.ContactUrl);
        }
    }
}