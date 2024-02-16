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
using SFA.DAS.EmployerIncentives.Application.Commands.RegisterEmploymentCheck;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.EmploymentCheckTests
{
    [TestFixture]
    public class WhenRegisterEmploymentCheckRequest
    {
        [Test, MoqAutoData]
        public async Task Then_RegisterCheck_is_handled(
            long accountId,
            Api.Models.RegisterCheckRequest request,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmploymentCheckController controller)
        {

            var controllerResult = await controller.RegisterCheck(request) as OkObjectResult;

            mockMediator
                .Verify(mediator => mediator.Send(
                    It.Is<RegisterEmploymentCheckCommand>(c =>
                        c.CorrelationId.Equals(request.CorrelationId)
                        && c.ApprenticeshipAccountId.Equals(request.ApprenticeshipAccountId)
                        && c.ApprenticeshipId.Equals(request.ApprenticeshipId)
                        && c.CheckType.Equals(request.CheckType)
                        && c.MaxDate.Equals(request.MaxDate)
                        && c.MinDate.Equals(request.MinDate)
                        && c.Uln.Equals(request.Uln)),
                    It.IsAny<CancellationToken>()));

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
