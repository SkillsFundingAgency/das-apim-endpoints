﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications.Qualifications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetDeleteQualification;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetDeleteQualifications;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.QualificationsController
{
    [TestFixture]
    public class WhenGettingDeleteQualifications
    {
        [Test, MoqAutoData]
        public async Task When_Getting_Multiple_Of_A_Type_Then_The_Query_Response_Is_Returned(
           Guid candidateId,
           Guid applicationId,
           Guid qualificationReferenceId,
           GetDeleteQualificationsQueryResult queryResult,
           [Frozen] Mock<IMediator> mediator,
           [Greedy] Api.Controllers.QualificationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetDeleteQualificationsQuery>(q =>
                        q.CandidateId == candidateId && q.ApplicationId == applicationId && q.QualificationReference == qualificationReferenceId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetDeleteQualifications(applicationId, qualificationReferenceId, candidateId, null);

            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetDeleteQualificationsApiResponse;
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo((GetDeleteQualificationsApiResponse)queryResult);
        }

        [Test, MoqAutoData]
        public async Task When_Getting_Multiple_Then_Mediator_Returns_Null_Response_So_NotFound_Is_Returned(
            Guid candidateId,
            Guid applicationId,
            Guid qualificationReferenceId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.QualificationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetDeleteQualificationsQuery>(q =>
                        q.CandidateId == candidateId && q.ApplicationId == applicationId && q.QualificationReference == qualificationReferenceId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var actual = await controller.GetDeleteQualifications(applicationId, qualificationReferenceId, candidateId, null);

            actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task When_Getting_Multiple_Then_Mediator_Returns_Empty_Response_So_NotFound_Is_Returned(
            Guid candidateId,
            Guid applicationId,
            Guid qualificationReferenceId,
            GetDeleteQualificationsQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.QualificationsController controller)
        {
            queryResult.Qualifications = [];
            mediator.Setup(x => x.Send(It.Is<GetDeleteQualificationsQuery>(q =>
                        q.CandidateId == candidateId && q.ApplicationId == applicationId && q.QualificationReference == qualificationReferenceId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetDeleteQualifications(applicationId, qualificationReferenceId, candidateId, null);

            actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task When_Getting_One_Of_A_Type_Then_The_Query_Response_Is_Returned(
            Guid candidateId,
            Guid applicationId,
            Guid qualificationReferenceId,
            Guid id,
            GetDeleteQualificationQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.QualificationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetDeleteQualificationQuery>(q =>
                        q.CandidateId == candidateId && q.ApplicationId == applicationId && q.QualificationReference == qualificationReferenceId && q.Id == id),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetDeleteQualifications(applicationId, qualificationReferenceId, candidateId, id);

            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetDeleteQualificationsApiResponse;
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo((GetDeleteQualificationsApiResponse)queryResult);
        }

        [Test, MoqAutoData]
        public async Task When_Getting_One_Then_Mediator_Returns_Null_Response_So_NotFound_Is_Returned(
            Guid candidateId,
            Guid applicationId,
            Guid qualificationReferenceId,
            Guid id,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.QualificationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetDeleteQualificationQuery>(q =>
                        q.CandidateId == candidateId && q.ApplicationId == applicationId && q.QualificationReference == qualificationReferenceId && q.Id == id),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => null);

            var actual = await controller.GetDeleteQualifications(applicationId, qualificationReferenceId, candidateId, id);

            actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
