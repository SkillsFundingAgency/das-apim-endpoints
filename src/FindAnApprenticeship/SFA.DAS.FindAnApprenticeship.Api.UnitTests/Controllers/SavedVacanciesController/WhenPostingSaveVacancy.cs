using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Vacancies;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Vacancies.SaveVacancy;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.SavedVacanciesController
{
    [TestFixture]
    public class WhenPostingSaveVacancy
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Response_Is_Returned(
            Guid candidateId,
            SaveVacancyApiRequest request,
            SaveVacancyCommandResult commandResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.SavedVacanciesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<SaveVacancyCommand>(q =>
                        q.CandidateId == candidateId &&
                        q.VacancyId == request.VacancyId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(commandResult);

            var actual = await controller.AddSavedVacancy(candidateId, request);

            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as SaveVacancyCommandResult;
            actualObject.Should().NotBeNull();
            actualObject.Should().Be(commandResult);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Command_Exception_Is_Returned(
            Guid candidateId,
            SaveVacancyApiRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.SavedVacanciesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<SaveVacancyCommand>(q =>
                        q.CandidateId == candidateId &&
                        q.VacancyId == request.VacancyId),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var actual = await controller.AddSavedVacancy(candidateId, request) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
