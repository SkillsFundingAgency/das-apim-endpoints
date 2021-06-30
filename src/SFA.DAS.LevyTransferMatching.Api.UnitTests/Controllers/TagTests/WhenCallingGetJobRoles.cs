﻿using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetAccount;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetJobRoles;
using SFA.DAS.LevyTransferMatching.Models.Tags;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.TagTests
{
    [TestFixture]
    public class WhenCallingGetJobRoles
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_JobRoles(
            GetJobRolesQueryResult queryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] ReferenceController referenceController)
        {
            mockMediator
                .Setup(x => x.Send(It.IsAny<GetJobRolesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var controllerResult = await referenceController.JobRoles() as ObjectResult;

            Assert.IsNotNull(controllerResult);
            Assert.AreEqual(controllerResult.StatusCode, (int) HttpStatusCode.OK);

            var model = controllerResult.Value as IEnumerable<ReferenceDataItem>;
            Assert.IsNotNull(model);
        }
    }
}
