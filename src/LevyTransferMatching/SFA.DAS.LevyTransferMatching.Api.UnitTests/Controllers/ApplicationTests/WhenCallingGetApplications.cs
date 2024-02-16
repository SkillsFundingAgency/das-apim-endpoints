using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Applications;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetApplications;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.ApplicationTests
{
    public class WhenCallingGetApplications
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Applications_From_Mediator(GetApplicationsQueryResult queryResult, [Frozen]Mock<IMediator> mediator,
            [Greedy]ApplicationsController controller)
        {
            mediator.Setup(o => o.Send(It.IsAny<GetApplicationsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var controllerResult = await controller.GetApplications(1) as OkObjectResult;
            var result = controllerResult.Value as GetApplicationsResponse;
            Assert.That(controllerResult, Is.Not.Null);
            Assert.That(result, Is.Not.Null);

            var expected = (GetApplicationsResponse) queryResult;
            var x = expected.Applications.First();
            var y = result.Applications.First();
            Assert.AreEqual(expected.Applications.Count(), result.Applications.Count());
            Assert.AreEqual(x.DasAccountName, y.DasAccountName);
            Assert.AreEqual(x.Details, y.Details);
            Assert.AreEqual(x.Amount, y.Amount);
            Assert.AreEqual(x.Status, y.Status);
            Assert.AreEqual(x.CreatedOn, y.CreatedOn);
            Assert.AreEqual(x.NumberOfApprentices, y.NumberOfApprentices);
            Assert.AreEqual(x.Id, y.Id);
            Assert.AreEqual(x.PledgeId, y.PledgeId);
            Assert.AreEqual(x.IsNamePublic, y.IsNamePublic);
        }
    }
}
