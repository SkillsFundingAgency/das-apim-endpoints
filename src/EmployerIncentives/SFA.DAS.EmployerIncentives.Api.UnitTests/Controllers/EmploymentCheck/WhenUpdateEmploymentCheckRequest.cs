using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Application.Commands.EmploymentCheck;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.EmploymentCheckTests
{
    [TestFixture]
    public class WhenUpdateEmploymentCheckRequest
    {
        [Test, MoqAutoData]
        public async Task Then_UpdateCheck_is_handled(
            long accountId,
            Api.Models.UpdateEmploymentCheckRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmploymentCheckController controller)
        {

            var controllerResult = await controller.UpdateCheck(request) as StatusCodeResult;

            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<EmploymentCheckCommand>(c =>
                        c.CorrelationId.Equals(request.CorrelationId)
                        && c.Result.Equals(request.Result)
                        && c.DateChecked.Equals(request.DateChecked)),
                    It.IsAny<CancellationToken>()));

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
