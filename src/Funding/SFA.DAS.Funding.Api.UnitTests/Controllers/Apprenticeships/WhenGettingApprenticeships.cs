using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Funding.Api.Controllers;
using SFA.DAS.Funding.Api.Models;
using SFA.DAS.Funding.Application.Queries.GetApprenticeships;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Funding.Api.UnitTests.Controllers.Apprenticeships
{
    public class WhenGettingApprenticeships
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Correct_Response_From_Mediator(
            long ukprn,
            GetApprenticeshipsResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ApprenticeshipsController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetApprenticeshipsQuery>(c => c.Ukprn.Equals(ukprn)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetAll(ukprn) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetApprenticeshipsResponse;
            Assert.IsNotNull(model);
            model.Apprenticeships.Should().BeEquivalentTo(mediatorResult.Apprenticeships);
        }
    }
}