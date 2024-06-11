using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApprovedAndAcceptedApplications;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.ApplicationTests
{
    public class WhenCallingGetApprovedAndAcceptedApplications
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_ApplicationsByStatus_From_Mediator(
            GetApprovedAndAcceptedApplicationsResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ApplicationsController controller)
        {
            mediator.Setup(o => o.Send(It.IsAny<GetApprovedAndAcceptedApplicationsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var controllerResult = await controller.GetApprovedAndAcceptedApplications(1) as OkObjectResult;
            var result = controllerResult.Value as GetApprovedAndAcceptedApplicationsResult;

            controllerResult.Should().NotBeNull();
            result.Should().NotBeNull();           
        }
    }
}
