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
using SFA.DAS.Approvals.Application.Providers.Queries;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;
using GetProvidersListResponse = SFA.DAS.Approvals.Api.Models.GetProvidersListResponse;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Providers
{
    public class WhenGettingProviderCoursesDeliveryModel
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_ProviderCourseDeliveryModel_From_Mediator(
            GetProviderCourseDeliveryModelsResponse mediatorResponse,
            int providerId,
            string trainingCode,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetProviderCoursesDeliveryModelQuery>(p=>p.ProviderId == providerId && p.TrainingCode == trainingCode),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResponse);

            var controllerResult = await controller.GetProviderCoursesDeliveryModel(providerId, trainingCode) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetProviderCourseDeliveryModelsResponse;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo(mediatorResponse);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            int providerId,
            string trainingCode,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ProvidersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetProviderCoursesDeliveryModelQuery>(),
                    It.IsAny<CancellationToken>()))
                .Throws<InvalidOperationException>();

            var controllerResult = await controller.GetProviderCoursesDeliveryModel(providerId, trainingCode) as BadRequestResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}