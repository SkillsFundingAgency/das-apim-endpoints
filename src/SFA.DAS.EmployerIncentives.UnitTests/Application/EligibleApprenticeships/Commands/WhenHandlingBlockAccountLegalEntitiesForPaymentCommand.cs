using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Commands.VendorBlock;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorBlock;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Commands
{
    [TestFixture]
    public class WhenHandlingBlockAccountLegalEntitiesForPaymentCommand
    {
        [Test, MoqAutoData]
        public async Task Then_the_vendor_block_request_is_sent_via_the_service(
            [Frozen] Mock<ILegalEntitiesService> legalEntitiesService,
            [Frozen] BlockAccountLegalEntityForPaymentsCommandHandler handler,
            BlockAccountLegalEntityForPaymentsCommand command)
        {
            legalEntitiesService.Setup(x => x.BlockAccountLegalEntitiesForPayments(command.VendorBlockRequest))
                .Returns(Task.CompletedTask);

            await handler.Handle(command, CancellationToken.None);

            legalEntitiesService.Verify(x => x.BlockAccountLegalEntitiesForPayments(It.Is<BlockAccountLegalEntityForPaymentsRequest>(
                r => r.VendorBlocks == command.VendorBlockRequest.VendorBlocks
                && r.ServiceRequest == command.VendorBlockRequest.ServiceRequest)), Times.Once);
        }
    }
}
