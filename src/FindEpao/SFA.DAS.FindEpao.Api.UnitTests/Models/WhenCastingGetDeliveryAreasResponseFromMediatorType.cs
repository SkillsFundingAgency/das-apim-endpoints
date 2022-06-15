using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using GetDeliveryAreaListItem = SFA.DAS.FindEpao.InnerApi.Responses.GetDeliveryAreaListItem;

namespace SFA.DAS.FindEpao.Api.UnitTests.Models
{
    public class WhenCastingGetDeliveryAreasResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            GetDeliveryAreaListItem source)
        {
            var response = (Api.Models.GetDeliveryAreaListItem)source;

            response.Should().BeEquivalentTo(source);
        }
    }
}