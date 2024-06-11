﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetPledges;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Finance;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses.Finance;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Pledges.GetPledges
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private GetPledgesQueryHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private Mock<IFinanceApiClient<FinanceApiConfiguration>> _financeApiClient;
        private GetPledgesQuery _query;
        private GetPledgesResponse _pledgeResponse;
        private GetApplicationsResponse _applicationsResponse;
        private GetTransferAllowanceResponse _fundingResponse;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _pledgeResponse = _fixture.Create<GetPledgesResponse>();
            _fundingResponse = _fixture.Create<GetTransferAllowanceResponse>();
            _applicationsResponse = _fixture.Create<GetApplicationsResponse>();

            var accountId = _fixture.Create<int>();
            _query = new GetPledgesQuery(accountId);

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _levyTransferMatchingService.Setup(x => x.GetPledges(It.IsAny<GetPledgesRequest>())).ReturnsAsync(_pledgeResponse);

            _levyTransferMatchingService.Setup(x => x.GetApplications(It.IsAny<GetApplicationsRequest>())).ReturnsAsync(_applicationsResponse);
            _financeApiClient = new Mock<IFinanceApiClient<FinanceApiConfiguration>>();
            _financeApiClient
                 .Setup(x => x.Get<GetTransferAllowanceResponse>(It.IsAny<GetTransferAllowanceByAccountIdRequest>()))
                  .ReturnsAsync(_fundingResponse);

            _handler = new GetPledgesQueryHandler(_levyTransferMatchingService.Object, _financeApiClient.Object);
        }

        [Test]
        public async Task Returns_Pledges()
        {
            var result = await _handler.Handle(_query, new CancellationToken());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Pledges, Is.Not.Null);
            result.Pledges.Should().NotBeEmpty();
            result.RemainingTransferAllowance.Should().Be(_fundingResponse.RemainingTransferAllowance);
            Assert.That(!result.Pledges.Any(x => x.Id == 0));
            Assert.That(!result.Pledges.Any(x => x.Amount == 0));
            Assert.That(!result.Pledges.Any(x => x.RemainingAmount == 0));
            Assert.That(!result.Pledges.Any(x => x.ApplicationCount == 0));
            Assert.That(!result.Pledges.Any(x => x.Status == string.Empty));
            result.AcceptedAndApprovedApplications.Should().NotBeEmpty();
            result.AcceptedAndApprovedApplications.Count().Should().Be(_applicationsResponse.Applications.Count() * 2);
        }
    }
}
