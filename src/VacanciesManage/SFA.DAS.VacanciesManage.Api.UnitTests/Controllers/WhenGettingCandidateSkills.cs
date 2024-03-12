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
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.VacanciesManage.Api.Controllers;
using SFA.DAS.VacanciesManage.Api.Models;
using SFA.DAS.VacanciesManage.Application.Recruit.Queries.GetCandidateSkills;

namespace SFA.DAS.VacanciesManage.Api.UnitTests.Controllers
{
    public class WhenGettingCandidateSkills
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Skills_From_Mediator(
            GetCandidateSkillsQueryResponse mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ReferenceDataController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetCandidateSkillsQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetSkills() as ObjectResult;

            Assert.That(controllerResult, Is.Not.Null);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as GetCandidateSkillsListResponse;
            Assert.That(model, Is.Not.Null);
            model.CandidateSkills.Should().BeEquivalentTo(mediatorResult.CandidateSkills);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Then_Returns_Bad_Request(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ReferenceDataController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetCandidateSkillsQuery>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            var controllerResult = await controller.GetSkills() as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}