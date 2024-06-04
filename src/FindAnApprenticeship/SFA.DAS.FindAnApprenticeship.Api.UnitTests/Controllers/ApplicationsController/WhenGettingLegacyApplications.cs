using System;
using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetLegacyApplications;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.ApplicationsController
{
    [TestFixture]
    public class WhenGettingLegacyApplications
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned(
            string email,
            GetLegacyApplicationsQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.ApplicationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetLegacyApplicationsQuery>(q =>
                        q.EmailAddress == email),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetLegacyApplications(email);

            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetLegacyApplicationsApiResponse;
            actualObject.Should().NotBeNull();
            actualObject!.Applications.Should().BeEquivalentTo(queryResult.Applications);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Is_Thrown_Then_Returns_InternalServerError(
            string email,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.ApplicationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetLegacyApplicationsQuery>(q =>
                        q.EmailAddress == email),
                    It.IsAny<CancellationToken>()))
               .ThrowsAsync(new InvalidOperationException());

            var actual = await controller.GetLegacyApplications(email);

            actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
