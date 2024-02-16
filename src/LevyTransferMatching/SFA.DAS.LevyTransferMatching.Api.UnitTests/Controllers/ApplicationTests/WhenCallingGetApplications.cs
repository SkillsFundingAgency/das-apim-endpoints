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
            Assert.That(expected.Applications.Count(), Is.EqualTo(result.Applications.Count()));
            Assert.That(x.DasAccountName, Is.EqualTo(y.DasAccountName));
            Assert.That(x.Details, Is.EqualTo(y.Details));
            Assert.That(x.Amount, Is.EqualTo(y.Amount));
            Assert.That(x.Status, Is.EqualTo(y.Status));
            Assert.That(x.CreatedOn,Is.EqualTo( y.CreatedOn));
            Assert.That(x.NumberOfApprentices, Is.EqualTo(y.NumberOfApprentices));
            Assert.That(x.Id, Is.EqualTo(y.Id));
            Assert.That(x.PledgeId, Is.EqualTo(y.PledgeId));
            Assert.That(x.IsNamePublic, Is.EqualTo(y.IsNamePublic));
        }
    }
}
