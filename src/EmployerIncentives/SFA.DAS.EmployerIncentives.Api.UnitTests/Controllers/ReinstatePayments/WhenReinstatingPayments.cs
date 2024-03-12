using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Application.Commands.ReinstatePayments;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.ReinstatePayments
{
    [TestFixture]
    public class WhenReinstatingPayments
    {
        [Test, MoqAutoData]
        public async Task Then_Sends_Reinstate_Payments_Command_To_Mediator(
            ReinstatePaymentsRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ReinstatePaymentsController controller)
        {
            var controllerResult = await controller.ReinstatePayments(request) as OkResult;

            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<ReinstatePaymentsCommand>(c =>
                        c.ReinstatePaymentsRequest.Equals(request)),
                    It.IsAny<CancellationToken>()));

            Assert.That(controllerResult, Is.Not.Null);
        }
    }
}
