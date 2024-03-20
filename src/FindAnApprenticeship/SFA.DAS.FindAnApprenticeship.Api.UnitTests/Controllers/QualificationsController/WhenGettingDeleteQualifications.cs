using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetDeleteQualifications;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.QualificationsController
{
    [TestFixture]
    public class WhenGettingDeleteQualifications
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned(
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

            var actual = await controller.GetDeleteQualifications(applicationId, qualificationReferenceId, candidateId);

            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetDeleteQualificationsApiResponse;
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo((GetDeleteQualificationsApiResponse)queryResult);
        }
    }
}
