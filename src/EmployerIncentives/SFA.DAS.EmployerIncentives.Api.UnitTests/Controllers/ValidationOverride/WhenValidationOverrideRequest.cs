using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Application.Commands.ValidationOverride;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.ValidationOverrideControllerTests
{
    [TestFixture]
    public class WhenValidationOverrideRequest
    {
        [Test, MoqAutoData]
        public async Task Then_WithdrawCommand_is_handled(
            ValidationOverrideRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ValidationOverrideController controller)
        {
            var controllerResult = await controller.Add(request) as AcceptedResult;

            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<ValidationOverrideCommand>(c =>
                        c.ValidationOverrideRequest.Equals(request)),
                    It.IsAny<CancellationToken>()), Times.Once);

            Assert.IsNotNull(controllerResult);
        }
    }
}
