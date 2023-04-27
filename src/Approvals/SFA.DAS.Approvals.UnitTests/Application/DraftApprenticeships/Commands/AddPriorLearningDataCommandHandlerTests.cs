using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Commands.AddPriorLearningData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.UnitTests.Application.DraftApprenticeships.Commands
{
    [TestFixture]
    public class AddPriorLearningDataCommandHandlerTests
    {
        private AddPriorLearningDataCommandHandler _handler;
        private AddPriorLearningDataCommand _request;
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsApiClient;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();
            _request = fixture.Create<AddPriorLearningDataCommand>();

            _commitmentsApiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

            _commitmentsApiClient.Setup(x => x.PostWithResponseCode<AddPriorLearningDataResponse>(It.IsAny<PostAddPriorLearningDataRequest>(), false))
                .Callback((IPostApiRequest request, bool includeResponse) => _request = request as AddPriorLearningDataCommand)
                .ReturnsAsync(new ApiResponse<AddPriorLearningDataResponse>(null, HttpStatusCode.OK, string.Empty));

            _handler = new AddPriorLearningDataCommandHandler(_commitmentsApiClient.Object);
        }

        [Test]
        public async Task Handle_Add_Prior_Learning_Response_Not_Null()
        {
            var response = await _handler.Handle(_request, CancellationToken.None);
            response.Should().NotBeNull();
        }

        [Test]
        public async Task Handle_PriorLearningData_Created()
        {
            var fixture = new Fixture();
            var expectedResponse = fixture.Create<AddPriorLearningDataResponse>();
            _commitmentsApiClient.Setup(x => x.PostWithResponseCode<AddPriorLearningDataResponse>(
                    It.Is<PostAddPriorLearningDataRequest>(r =>
                            ((AddPriorLearningDataRequest)r.Data).CostBeforeRpl == _request.CostBeforeRpl &&
                            ((AddPriorLearningDataRequest)r.Data).DurationReducedBy == _request.DurationReducedBy &&
                            ((AddPriorLearningDataRequest)r.Data).DurationReducedByHours == _request.DurationReducedByHours &&
                            ((AddPriorLearningDataRequest)r.Data).IsDurationReducedByRpl == _request.IsDurationReducedByRpl &&
                            ((AddPriorLearningDataRequest)r.Data).PriceReducedBy == _request.PriceReducedBy &&
                            ((AddPriorLearningDataRequest)r.Data).TrainingTotalHours == _request.TrainingTotalHours
                        ), true
                )).ReturnsAsync(new ApiResponse<AddPriorLearningDataResponse>(expectedResponse, HttpStatusCode.OK, string.Empty));

            var response = await _handler.Handle(_request, CancellationToken.None);

            Assert.IsInstanceOf<MediatR.Unit>(response);
        }
    }
}