using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Application.Commands.ReinstateApplication;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.WithdrawalControllerTests
{
    [TestFixture]
    public class WhenReinstateApplicationRequest
    {
        [Test, MoqAutoData]
        public async Task Then_ReinstateApplicationCommand_is_handled(
            ReinstateApplicationRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] WithdrawalController controller)
        {
            var controllerResult = await controller.ReinstateApplication(request) as AcceptedResult;

            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<ReinstateApplicationCommand>(c =>
                        c.ReinstateApplicationRequest.Equals(request)),
                    It.IsAny<CancellationToken>()));

            Assert.That(controllerResult, Is.Not.Null);
        }
    }
}
