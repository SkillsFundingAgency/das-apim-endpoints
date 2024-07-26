using System;
using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Users.MigrateData;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController
{
    [TestFixture]
    public class WhenGettingMigrateDataTransfer
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned_And_Legacy_Details_Used_If_Candidate_Values_Null(
            Guid candidateId,
            string email,
            MigrateDataQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.UsersController controller)
        {
            queryResult.CandidateDetail.FirstName = null;
            queryResult.CandidateDetail.LastName = null;
            mediator.Setup(x => x.Send(It.Is<MigrateDataQuery>(q =>
                        q.EmailAddress == email && q.CandidateId == candidateId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var actual = await controller.MigrateDataTransfer(email,candidateId);

            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetMigrateDataTransferApiResponse;
            actualObject.Should().NotBeNull();
            actualObject!.Applications.Should().BeEquivalentTo(queryResult.Applications, options => options
                .Excluding(fil => fil.Id)
                .Excluding(fil => fil.AdditionalQuestion1Answer)
                .Excluding(fil => fil.AdditionalQuestion2Answer)
                .Excluding(fil => fil.CandidateInformation)
                .Excluding(fil => fil.DateApplied)
                .Excluding(fil => fil.SuccessfulDateTime)
                .Excluding(fil => fil.UnsuccessfulDateTime)
                .Excluding(fil => fil.Vacancy)
                .Excluding(fil => fil.UnsuccessfulReason)
                .Excluding(fil => fil.DateCreated)
            );
            actualObject!.FirstName.Should().Be(queryResult.LegacyUserDetail.FirstName);
            actualObject!.LastName.Should().Be(queryResult.LegacyUserDetail.LastName);
        }
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned_And_Candidate_Details_Used_If_Values_Not_Null(
            Guid candidateId,
            string email,
            MigrateDataQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.UsersController controller)
        {
            mediator.Setup(x => x.Send(It.Is<MigrateDataQuery>(q =>
                        q.EmailAddress == email && q.CandidateId == candidateId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var actual = await controller.MigrateDataTransfer(email,candidateId);

            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetMigrateDataTransferApiResponse;
            actualObject.Should().NotBeNull();
            actualObject!.Applications.Should().BeEquivalentTo(queryResult.Applications, options => options
                .Excluding(fil => fil.Id)
                .Excluding(fil => fil.AdditionalQuestion1Answer)
                .Excluding(fil => fil.AdditionalQuestion2Answer)
                .Excluding(fil => fil.CandidateInformation)
                .Excluding(fil => fil.DateApplied)
                .Excluding(fil => fil.SuccessfulDateTime)
                .Excluding(fil => fil.UnsuccessfulDateTime)
                .Excluding(fil => fil.Vacancy)
                .Excluding(fil => fil.UnsuccessfulReason)
                .Excluding(fil => fil.DateCreated)
            );
            actualObject!.FirstName.Should().Be(queryResult.CandidateDetail.FirstName);
            actualObject!.LastName.Should().Be(queryResult.CandidateDetail.LastName);
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Is_Thrown_Then_Returns_InternalServerError(
            string email,
            Guid candidateId,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.UsersController controller)
        {
            mediator.Setup(x => x.Send(It.Is<MigrateDataQuery>(q =>
                        q.EmailAddress == email && q.CandidateId == candidateId),
                    It.IsAny<CancellationToken>()))
               .ThrowsAsync(new InvalidOperationException());

            var actual = await controller.MigrateDataTransfer(email, candidateId);

            actual.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}

