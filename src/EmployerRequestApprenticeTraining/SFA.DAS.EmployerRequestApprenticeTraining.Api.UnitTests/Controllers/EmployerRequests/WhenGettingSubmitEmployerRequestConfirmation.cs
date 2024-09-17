using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.Api.Controllers;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerProfileUser;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerRequest;
using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetStandard;
using SFA.DAS.EmployerRequestApprenticeTraining.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Api.UnitTests.Controllers.EmployerRequests
{
    public class WhenGettingSubmitEmployerRequestConfirmation
    {

        [Test, MoqAutoData]
        public async Task Then_The_SubmitEmployerRequestConfirmation_Is_Returned_From_Mediator(
            Guid employerRequestId,
            GetEmployerRequestResult queryResult,
            GetStandardResult standardResult,
            GetEmployerProfileUserResult profileUserResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EmployerRequestsController controller)
        {
            mockMediator
                .Setup(x => x.Send(It.Is<GetEmployerRequestQuery>(p => p.EmployerRequestId == employerRequestId), CancellationToken.None))
                .ReturnsAsync(queryResult);

            mockMediator
                .Setup(x => x.Send(It.Is<GetStandardQuery>(p => p.StandardId == queryResult.EmployerRequest.StandardReference), CancellationToken.None))
                .ReturnsAsync(standardResult);

            mockMediator
                .Setup(x => x.Send(It.Is<GetEmployerProfileUserQuery>(p => p.UserId == queryResult.EmployerRequest.RequestedBy), CancellationToken.None))
                .ReturnsAsync(profileUserResult);

            var actual = await controller.GetSubmitEmployerRequestConfirmation(employerRequestId) as ObjectResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
            actual.Value.Should().BeEquivalentTo(new SubmitEmployerRequestConfirmation
            {
                EmployerRequestId = queryResult.EmployerRequest.Id,
                StandardTitle = standardResult.Standard.Title,
                StandardLevel = standardResult.Standard.Level,
                NumberOfApprentices = queryResult.EmployerRequest.NumberOfApprentices,
                SameLocation = queryResult.EmployerRequest.SameLocation,
                SingleLocation = queryResult.EmployerRequest.SingleLocation,
                AtApprenticesWorkplace = queryResult.EmployerRequest.AtApprenticesWorkplace,
                DayRelease = queryResult.EmployerRequest.DayRelease,
                BlockRelease = queryResult.EmployerRequest.BlockRelease,
                RequestedByEmail = profileUserResult.Email,
                Regions = queryResult.EmployerRequest.Regions
            });
        }

        [Test, MoqAutoData]
        public async Task Then_NotFound_Returned_If_Request_Does_Not_Exist(
            Guid employerRequestId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetEmployerRequestQuery>(), CancellationToken.None))
                .ReturnsAsync(new GetEmployerRequestResult());

            var actual = await controller.GetSubmitEmployerRequestConfirmation(employerRequestId) as NotFoundResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Test, MoqAutoData]
        public async Task Then_InternalServerError_Returned_If_An_Exception_Is_Thrown(
            Guid employerRequestId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] EmployerRequestsController controller)
        {
            mediator.Setup(x => x.Send(It.IsAny<GetEmployerRequestQuery>(), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.GetSubmitEmployerRequestConfirmation(employerRequestId) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}