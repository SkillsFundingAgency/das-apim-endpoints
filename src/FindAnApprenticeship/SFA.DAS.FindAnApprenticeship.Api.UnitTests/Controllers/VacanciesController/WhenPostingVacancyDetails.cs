using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Vacancies;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Vacancies;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.VacanciesController
{
    [TestFixture]
    public class WhenPostingVacancyDetails
    {
        [Test, MoqAutoData]
        public async Task Then_The_Command_Response_Is_Returned(
        string vacancyReference,
        PostApplyApiRequest apiRequest,
        ApplyCommandResponse result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.VacanciesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<ApplyCommand>(c =>
                c.VacancyReference.Equals(vacancyReference)),
                    CancellationToken.None))
                .ReturnsAsync(result);

            var actual = await controller.Apply(vacancyReference, apiRequest) as CreatedResult;

            Assert.That(actual, Is.Not.Null);
            var actualModel = actual.Value as PostApplyApiResponse;
            Assert.That(actualModel, Is.Not.Null);
            actualModel.Should().BeEquivalentTo((PostApplyApiResponse)result);

            mediator.Verify(m => m.Send(It.Is<ApplyCommand>(c =>
                    c.VacancyReference == vacancyReference && c.CandidateId == apiRequest.CandidateId),
                CancellationToken.None));
        }

        [Test, MoqAutoData]
        public async Task Then_If_An_Exception_Is_Thrown_Then_Internal_Server_Error_Response_Returned(
            string vacancyReference,
            PostApplyApiRequest apiRequest,
            [Frozen] Mock<ILogger<Api.Controllers.VacanciesController>> logger,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.VacanciesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<ApplyCommand>(c =>
                        c.VacancyReference.Equals(vacancyReference)),
                    CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.Apply(vacancyReference, apiRequest) as StatusCodeResult;

            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Test, MoqAutoData]
        public async Task Then_If_An_Null_Is_Returned_Then_Not_Found_Response_Returned(
            string vacancyReference,
            PostApplyApiRequest apiRequest,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.VacanciesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<ApplyCommand>(c =>
                        c.VacancyReference.Equals(vacancyReference)),
                    CancellationToken.None))
                .ReturnsAsync((() => null));

            var actual = await controller.Apply(vacancyReference, apiRequest) as StatusCodeResult;

            Assert.IsNotNull(actual);
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }
    }
}
