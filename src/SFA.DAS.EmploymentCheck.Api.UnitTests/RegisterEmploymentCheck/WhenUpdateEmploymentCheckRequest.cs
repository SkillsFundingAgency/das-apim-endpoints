using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmploymentCheck.Api.Controllers;
using SFA.DAS.EmploymentCheck.Api.Models;
using SFA.DAS.EmploymentCheck.Application.Commands.RegisterCheck;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmploymentCheck.Api.UnitTests.RegisterEmploymentCheck
{
    [TestFixture]
    public class WhenRegisterEmploymentCheckRequest
    {
        [Test, MoqAutoData]
        public async Task Then_RegisterCheck_is_handled(
            RegisterCheckRequest request,
            RegisterCheckResponse response,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmploymentCheckController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<RegisterCheckCommand>(c =>
                        c.CorrelationId.Equals(request.CorrelationId)
                        && c.ApprenticeshipAccountId.Equals(request.ApprenticeshipAccountId)
                        && c.ApprenticeshipId.Equals(request.ApprenticeshipId)
                        && c.CheckType.Equals(request.CheckType)
                        && c.MaxDate.Equals(request.MaxDate)
                        && c.MinDate.Equals(request.MinDate)
                        && c.Uln.Equals(request.Uln)
                        ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var controllerResult = await controller.RegisterCheck(request) as OkObjectResult;
           
            Assert.IsNotNull(controllerResult);
            controllerResult.Value.Should().Be(response);
        }
    }
}
