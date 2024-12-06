using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetVacancyDetails;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.VacanciesController
{
    public class WhenGetNhsVacancyByReference
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned(
        string vacancyReference,
        GetVacancyDetailsQueryResult result,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.VacanciesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetVacancyDetailsQuery>(c =>
                c.VacancyReference.Equals(vacancyReference)),
                    CancellationToken.None))
                .ReturnsAsync(result);

            var actual = await controller.GetNhsVacancyByReference(vacancyReference) as OkObjectResult;

            Assert.That(actual, Is.Not.Null);
            var actualModel = actual.Value as GetApprenticeshipNhsVacancyApiResponse;
            Assert.That(actualModel, Is.Not.Null);
            actualModel.Should().BeEquivalentTo((GetApprenticeshipNhsVacancyApiResponse)result);

            mediator.Verify(m => m.Send(It.Is<GetVacancyDetailsQuery>(c =>
                    c.VacancyReference == vacancyReference),
                CancellationToken.None));
        }

        [Test, MoqAutoData]
        public async Task Then_If_An_Exception_Is_Thrown_Then_Internal_Server_Error_Response_Returned(
            string vacancyReference,
            GetVacancyDetailsQueryResult result,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.VacanciesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetVacancyDetailsQuery>(c =>
                        c.VacancyReference.Equals(vacancyReference)),
                    CancellationToken.None))
           .ThrowsAsync(new Exception());

            var actual = await controller.GetNhsVacancyByReference(vacancyReference) as StatusCodeResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);

            mediator.Verify(m => m.Send(It.Is<GetVacancyDetailsQuery>(c =>
                    c.VacancyReference == vacancyReference),
                CancellationToken.None));
        }

        [Test, MoqAutoData]
        public async Task Then_If_An_Null_Is_Returned_Then_Not_Found_Response_Returned(
            string vacancyReference,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.VacanciesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetVacancyDetailsQuery>(c =>
                        c.VacancyReference.Equals(vacancyReference)),
                    CancellationToken.None))
               .ReturnsAsync((GetVacancyDetailsQueryResult)null);

            var actual = await controller.GetNhsVacancyByReference(vacancyReference) as StatusCodeResult;

            Assert.That(actual, Is.Not.Null);
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);

            mediator.Verify(m => m.Send(It.Is<GetVacancyDetailsQuery>(c =>
                    c.VacancyReference == vacancyReference),
                CancellationToken.None));
        }
    }
}
