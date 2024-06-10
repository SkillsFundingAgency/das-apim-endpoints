﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplicationsByStatus;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.ApplicationTests
{
    public class WhenCallingGetApplicationsByStatus
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_ApplicationsByStatus_From_Mediator(
            GetApplicationsByStatusResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ApplicationsController controller)
        {
            mediator.Setup(o => o.Send(It.IsAny<GetApplicationsByStatusQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var controllerResult = await controller.GetApplicationsByStatus(1, "Approved") as OkObjectResult;
            var result = controllerResult.Value as GetApplicationsByStatusResponse;

            controllerResult.Should().NotBeNull();
            result.Should().NotBeNull();

            var expected = (GetApplicationsByStatusResponse)queryResult;
            var x = expected.Applications.First();
            var y = result.Applications.First();

            expected.Applications.Count().Should().Be(result.Applications.Count());
            x.DasAccountName.Should().Be(y.DasAccountName);
            x.Details.Should().Be(y.Details);
            x.Amount.Should().Be(y.Amount);
            x.Status.Should().Be(y.Status);
            x.CreatedOn.Should().Be(y.CreatedOn);
            x.NumberOfApprentices.Should().Be(y.NumberOfApprentices);
            x.PledgeId.Should().Be(y.PledgeId);
            x.Id.Should().Be(y.Id);
            x.IsNamePublic.Should().Be(y.IsNamePublic);
        }
    }
}
