using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorBlock;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    [TestFixture]
    public class WhenCallingBlockAccountLegalEntitiesForPayments
    {
        [Test]
        [MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Vendor_Block_Request(
            List<BlockAccountLegalEntityForPaymentsRequest> request,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            LegalEntitiesService service
        )
        {
            client.Setup(x => x.PatchWithResponseCode(It.Is<PatchVendorBlockRequest>(
                c => c.PatchUrl.Contains("blockedpayments")
            ))).ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.NoContent, ""));

            await service.BlockAccountLegalEntitiesForPayments(request);

            client.Verify(x => x.PatchWithResponseCode(It.Is<PatchVendorBlockRequest>(
                c => c.PatchUrl.Contains("blockedpayments")
            )), Times.AtLeastOnce);
        }
    }
}