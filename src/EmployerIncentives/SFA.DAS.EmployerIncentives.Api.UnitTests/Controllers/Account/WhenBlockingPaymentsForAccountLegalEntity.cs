using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Application.Commands.VendorBlock;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorBlock;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.Account
{
    [TestFixture]
    public class WhenBlockingPaymentsForAccountLegalEntity
    {
        [Test]
        [MoqAutoData]
        public async Task Then_Sends_Block_Request_To_Mediator(
            long accountId,
            List<BlockAccountLegalEntityForPaymentsRequest> request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountController controller)
        {
            var controllerResult = await controller.BlockAccountLegalEntityForPayments(request) as NoContentResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NoContent);

            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<BlockAccountLegalEntityForPaymentsCommand>(c =>
                        c.VendorBlockRequest == request),
                    It.IsAny<CancellationToken>()));
        }
    }
}