using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Api.Controllers;
using SFA.DAS.EmployerAccounts.Application.Queries.GetLatestDetails;
using SFA.DAS.EmployerAccounts.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.Api.UnitTests.Controllers.SearchOrganisations
{
    public class WhenGettingLatestDetails
    {
        [Test, MoqAutoData]
        public async Task Then_GetsLatestDetails_From_Mediator(
            string identifier,
            OrganisationType organisationType,
          GetLatestDetailsResult mediatorResult,
          [Frozen] Mock<IMediator> mockMediator,
          [Greedy] SearchOrganisationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetLatestDetailsQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetLatestDetails(identifier, organisationType) as ObjectResult;

            controllerResult.Should().NotBeNull();

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as Api.Models.Organisation;

            model.Should().NotBeNull();

            model.Should().BeEquivalentTo(mediatorResult.OrganisationDetail);
        }


        [Test, MoqAutoData]
        public async Task And_Mediator_Response_IsOrganisationNotFoundException_Returns_Message(
             string identifier,
            OrganisationType organisationType,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] SearchOrganisationController controller)
        {
            mockMediator
               .Setup(mediator => mediator.Send(
                   It.IsAny<GetLatestDetailsQuery>(),
                   It.IsAny<CancellationToken>()))
                   .Throws(new OrganisationNotFoundException(organisationType, identifier));

            // Act
            Func<Task> act = async () => await controller.GetLatestDetails(identifier, organisationType);

            // Assert
            await act.Should().ThrowAsync<OrganisationNotFoundException>()
                .WithMessage($"Did not find an organisation type {organisationType} with identifier {identifier}");
        }

        [Test, MoqAutoData]
        public async Task And_Mediator_Response_InvalidGetOrganisationRequest(
            string identifier,
            string errorMessage,
           OrganisationType organisationType,
       [Frozen] Mock<IMediator> mockMediator,
       [Greedy] SearchOrganisationController controller)
        {
            mockMediator
               .Setup(mediator => mediator.Send(
                   It.IsAny<GetLatestDetailsQuery>(),
                   It.IsAny<CancellationToken>()))
                   .Throws(new InvalidGetOrganisationException(errorMessage));

            // Act
            Func<Task> act = async () => await controller.GetLatestDetails(identifier, organisationType);

            // Assert
            await act.Should().ThrowAsync<InvalidGetOrganisationException>()
                .WithMessage(errorMessage);
        }

    }
}
