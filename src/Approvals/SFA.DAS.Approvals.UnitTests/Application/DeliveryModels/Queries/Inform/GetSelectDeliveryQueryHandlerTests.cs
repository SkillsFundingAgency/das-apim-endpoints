﻿using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.Inform;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.DeliveryModels.Queries.Inform
{
    [TestFixture]
    public class GetInformQueryHandlerTests
    {
        private GetInformQueryHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClient;
        private static readonly Fixture _fixture = new();
        private readonly GetInformQuery _query = _fixture.Create<GetInformQuery>();
        private readonly GetApprenticeshipResponse _apprenticeshipResponse = _fixture.Create<GetApprenticeshipResponse>();

        [SetUp]
        public void Setup()
        {
            _apprenticeshipResponse.ProviderId = _query.ProviderId;
            _commitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
            _commitmentsApiClient.Setup(x =>
                    x.Get<GetApprenticeshipResponse>(
                        It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
                .ReturnsAsync(_apprenticeshipResponse);

            _handler = new GetInformQueryHandler(_commitmentsApiClient.Object);
        }

        [Test]
        public async Task Handle_Returns_LegalEntityName_For_Apprenticeship()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.That(result.LegalEntityName, Is.EqualTo(_apprenticeshipResponse.EmployerName));
        }

        [Test]
        public async Task Handle_Returns_Null_If_Apprentice_Is_Not_Found()
        {
            _commitmentsApiClient.Setup(x =>
                    x.Get<GetApprenticeshipResponse>(
                        It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
                .ReturnsAsync(() => null);

            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Handle_Returns_Null_If_Apprentice_Does_Not_Belong_To_Provider()
        {
            _apprenticeshipResponse.ProviderId = _query.ProviderId + 1;

            var result = await _handler.Handle(_query, CancellationToken.None);

            Assert.That(result, Is.Null);
        }
    }
}
