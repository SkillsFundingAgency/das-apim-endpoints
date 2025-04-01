using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Api.Controllers;
using SFA.DAS.LearnerData.Application;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LearnerData.Api.UnitTests.Controllers;

    public class WhenReceivingLearnerData
    {

        [Test, MoqAutoData]
        public async Task And_when_working_Then_Accepted_returned(
            List<LearnerDataRequest> learners,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LearnersController sut)
        {

            var result = await sut.Post(learners) as AcceptedResult;

            result.StatusCode.Should().Be((int)HttpStatusCode.Accepted);
            mockMediator.Verify(x => x.Send(It.Is<ProcessLearnersCommand>(p=>p.Learners == learners), It.IsAny<CancellationToken>()), Times.Once);
        }

    [Test, MoqAutoData]
    public async Task And_when_not_working_Then_internalerror_returned(
        List<LearnerDataRequest> learners,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] LearnersController sut)
    {
        mockMediator.Setup(x => x.Send(It.IsAny<ProcessLearnersCommand>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("test"));

        var result = await sut.Post(learners) as StatusCodeResult;

        result.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        mockMediator.Verify(x => x.Send(It.IsAny<ProcessLearnersCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}

