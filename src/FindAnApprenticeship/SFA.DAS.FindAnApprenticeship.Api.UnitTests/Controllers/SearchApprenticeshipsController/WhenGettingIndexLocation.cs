using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchIndexLocation;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.SearchApprenticeshipsController
{
    public class WhenGettingIndexLocation
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned(
            string searchTerm,
            IndexLocationResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.SearchApprenticeshipsController controller)
        {
            mediator.Setup(x => x.Send(
                    It.Is<IndexLocationQuery>(c => c.LocationSearchTerm.Equals(searchTerm)),
                    CancellationToken.None))
                .ReturnsAsync(mediatorResult);

            var actual = await controller.IndexLocation(searchTerm) as OkObjectResult;

            Assert.IsNotNull(actual);
            var actualModel = actual.Value as IndexLocationApiResponse;
            Assert.IsNotNull(actualModel);
            actualModel.Should().BeEquivalentTo((IndexLocationApiResponse)mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task Then_If_An_Exception_Is_Thrown_Then_Internal_Server_Error_Response_Returned(
            string searchTerm,
            IndexLocationResult mediatorResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.SearchApprenticeshipsController controller)
        {
            mediator.Setup(x => x.Send(
                It.Is<IndexLocationQuery>(c => c.LocationSearchTerm.Equals(searchTerm)),
                CancellationToken.None)).ThrowsAsync(new Exception());

            var actual = await controller.IndexLocation(searchTerm) as StatusCodeResult;

            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
