using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Application.Commands.RevertPayments;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.RevertPayments
{
    [TestFixture]
    public class WhenRevertingPayments
    {
        [Test, MoqAutoData]
        public async Task Then_Sends_Revert_Payments_Command_To_Mediator(
            RevertPaymentsRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] RevertPaymentsController controller)
        {
            var controllerResult = await controller.RevertPayments(request) as OkResult;

            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<RevertPaymentsCommand>(c =>
                        c.RevertPaymentsRequest.Equals(request)),
                    It.IsAny<CancellationToken>()));

            Assert.That(controllerResult, Is.Not.Null);
        }
    }
}