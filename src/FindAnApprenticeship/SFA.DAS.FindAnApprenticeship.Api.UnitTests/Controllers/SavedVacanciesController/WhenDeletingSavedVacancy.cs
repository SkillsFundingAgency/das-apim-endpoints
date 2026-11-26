using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Api.Models.Vacancies;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Vacancies.DeleteSavedVacancy;
using System.Net;
using System.Threading;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.SavedVacanciesController
{
    [TestFixture]
    public class WhenDeletingSavedVacancy
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Response_Is_Returned(
            Guid candidateId,
            DeleteSavedVacancyApiRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.SavedVacanciesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<DeleteSavedVacancyCommand>(q =>
                        q.CandidateId == candidateId &&
                        q.VacancyId == request.VacancyId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value);

            var actual = await controller.DeleteSavedVacancy(candidateId, request);

            actual.Should().BeOfType<OkObjectResult>();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Command_Exception_Is_Returned(
            Guid candidateId,
            DeleteSavedVacancyApiRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.SavedVacanciesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<DeleteSavedVacancyCommand>(q =>
                        q.CandidateId == candidateId &&
                        q.VacancyId == request.VacancyId),
                    It.IsAny<CancellationToken>()))
             .ThrowsAsync(new Exception());

            var actual = await controller.DeleteSavedVacancy(candidateId, request) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
