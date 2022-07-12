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
using SFA.DAS.Approvals.Api.Controllers;
using SFA.DAS.Approvals.Application.DeliveryModels.Queries;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;
using GetProvidersListResponse = SFA.DAS.Approvals.Api.Models.GetProvidersListResponse;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Providers
{
    public class WhenGettingProviderCoursesDeliveryModel
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

            var controllerResult = await controller.GetProviderCoursesDeliveryModel(providerId, trainingCode, accountLegalEntityId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetDeliveryModelsQueryResult;
            Assert.IsNotNull(model);
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

            var controllerResult = await controller.GetProviderCoursesDeliveryModel(providerId, trainingCode, accountLegalEntityId) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}