using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.EditApprenticeship;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Queries
{
    [TestFixture]
    public class GetEditApprenticeshipQueryHandlerTests
    {
        private GetEditApprenticeshipQueryHandler _handler;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
        private Mock<IDeliveryModelService> _deliveryModelService;
        private ServiceParameters _serviceParameters;

        private GetApprenticeshipResponse _apprenticeship;
        private GetEditApprenticeshipQuery _query;
        private List<string> _deliveryModels;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _apprenticeship = fixture.Build<GetApprenticeshipResponse>()
                .With(x => x.EmployerAccountId, 123)
                .Create();
            _query = fixture.Create<GetEditApprenticeshipQuery>();
            _deliveryModels = fixture.Create<List<string>>();

            _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _apiClient.Setup(x =>
                    x.GetWithResponseCode<GetApprenticeshipResponse>(It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
                .ReturnsAsync(new ApiResponse<GetApprenticeshipResponse>(_apprenticeship, HttpStatusCode.OK, string.Empty));

            _deliveryModelService = new Mock<IDeliveryModelService>();
            _deliveryModelService.Setup(x => x.GetDeliveryModels(
                It.Is<long>(p => p == _apprenticeship.ProviderId),
                It.Is<string>(s => s == _apprenticeship.CourseCode),
                It.Is<long>(ale => ale == _apprenticeship.AccountLegalEntityId),
                It.Is<long?>(a => a == _apprenticeship.ContinuationOfId)))
            .ReturnsAsync(_deliveryModels);

            _serviceParameters = new ServiceParameters(Party.Employer, 123);

            _handler = new GetEditApprenticeshipQueryHandler(_apiClient.Object, _deliveryModelService.Object, _serviceParameters);
        }

        [Test]
        public async Task Handle_IsFundedByTransfer_Mapped()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(_apprenticeship.TransferSenderId.HasValue, result.IsFundedByTransfer);
        }

        [TestCase(0, false)]
        [TestCase(1, false)]
        [TestCase(2, true)]
        public async Task Handle_HasMultipleDeliveryModelOptions_Reflects_Number_Of_Options_Available(int optionCount, bool expectedHasMultiple)
        {
            var fixture = new Fixture();
            _deliveryModels.Clear();
            _deliveryModels.AddRange(fixture.CreateMany<string>(optionCount));

            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(expectedHasMultiple, result.HasMultipleDeliveryModelOptions);
        }
    }
}
