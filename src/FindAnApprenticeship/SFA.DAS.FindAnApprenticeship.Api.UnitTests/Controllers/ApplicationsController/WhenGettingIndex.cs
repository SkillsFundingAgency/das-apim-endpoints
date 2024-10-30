﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetApplications;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.ApplicationsController
{
    [TestFixture]
    public class WhenGettingIndex
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned(
            Guid candidateId,
            ApplicationStatus status,
            GetApplicationsQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.ApplicationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetApplicationsQuery>(q =>
                        q.CandidateId == candidateId && q.Status == status),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var actual = await controller.Index(candidateId, status);

            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetApplicationsApiResponse;
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo(queryResult);
        }
    }
}
