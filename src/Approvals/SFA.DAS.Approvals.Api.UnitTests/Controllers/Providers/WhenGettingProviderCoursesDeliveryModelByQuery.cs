﻿using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Application.DeliveryModels.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Providers
{
    public class WhenGettingProviderCoursesDeliveryModelByQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_ProviderCourseDeliveryModel_From_Mediator(
            GetDeliveryModelsQueryResult mediatorResponse,
            int providerId,
            string trainingCode,
            long accountLegalEntityId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetDeliveryModelsQuery>(p=>p.ProviderId == providerId && p.TrainingCode == trainingCode),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResponse);

            var controllerResult = await controller.GetProviderCoursesDeliveryModelByQuery(providerId, trainingCode, accountLegalEntityId) as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetDeliveryModelsQueryResult;
            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo(mediatorResponse);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int providerId,
            string trainingCode,
            long accountLegalEntityId,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetDeliveryModelsQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetProviderCoursesDeliveryModelByQuery(providerId, trainingCode, accountLegalEntityId) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}