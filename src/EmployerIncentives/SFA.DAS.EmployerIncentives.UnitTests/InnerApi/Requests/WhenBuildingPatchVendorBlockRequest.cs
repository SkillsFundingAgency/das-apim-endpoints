using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorBlock;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPatchVendorBlockRequest
    {
        [Test, AutoData]
        public void Then_The_PatchUrl_Is_Correctly_Built(List<BlockAccountLegalEntityForPaymentsRequest> data)
        {
            var actual = new PatchVendorBlockRequest(data);

            actual.PatchUrl.Should().Be($"/blockedpayments");
            actual.Data.Should().BeEquivalentTo(data);
        }
    }
}