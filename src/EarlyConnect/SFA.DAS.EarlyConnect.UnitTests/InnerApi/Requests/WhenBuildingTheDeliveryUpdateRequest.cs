using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheDeliveryUpdateRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(DeliveryUpdate deliveryUpdate)
        {
            var actual = new DeliveryUpdateRequest(deliveryUpdate);

            actual.PostUrl.Should().Be("api/delivery-update");
        }
    }
}