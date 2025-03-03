using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Payments.Application.Learners;
using SFA.DAS.Payments.Models.Requests;
using SFA.DAS.Payments.Models.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Payments.UnitTests.Application.Learners
{
    [TestFixture]
    public class WhenGetLearnersQueryHandler
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. - nUnit initializes fields in SetUp
        private Fixture _fixture;
        private Mock<ILearnerDataApiClient<LearnerDataApiConfiguration>> _apiClientMock;
        private Mock<ILogger<GetLearnersQueryHandler>> _loggerMock;
        private GetLearnersQueryHandler _handler;
#pragma warning restore CS8618 // Non-nullable field is uninitialized.

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _apiClientMock = new Mock<ILearnerDataApiClient<LearnerDataApiConfiguration>>();
            _loggerMock = new Mock<ILogger<GetLearnersQueryHandler>>();
            _handler = new GetLearnersQueryHandler(_loggerMock.Object, _apiClientMock.Object);
        }

        [Test]
        public async Task And_OnePageOfDataReturned_ThenShouldReturnLearners()
        {
            // Arrange
            var query = _fixture.Create<GetLearnersQuery>();
            var learners = _fixture.CreateMany<LearnerResponse>(9).ToList();

            MockResponse(learners, 1, 1);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(learners);
        }

        [Test]
        public async Task And_MultiplePagesOfDataReturned_ThenShouldReturnLearners()
        {
            // Arrange
            var query = _fixture.Create<GetLearnersQuery>();
            var learners = _fixture.CreateMany<LearnerResponse>(9).ToList();

            MockResponse(learners, 1, 3); // page 1 of 3
            MockResponse(learners, 2, 3); // page 2 of 3
            MockResponse(learners, 3, 3); // page 3 of 3

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(learners);
        }

        [Test]
        public async Task And_NoDataReturned_ThenShouldReturnEmptyList()
        {
            // Arrange
            var query = _fixture.Create<GetLearnersQuery>();
            var emptyList = new List<LearnerResponse>();
            var apiResponse = new ApiResponse<List<LearnerResponse>>(null, System.Net.HttpStatusCode.NoContent, null, new Dictionary<string, IEnumerable<string>>());

            _apiClientMock.Setup(x => x.GetWithResponseCode<List<LearnerResponse>>(It.IsAny<GetLearnersRequest>())).ReturnsAsync(apiResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(emptyList);
        }

        private void MockResponse(List<LearnerResponse> learners, int pageNumber, int totalPages)
        {
            var itemsPerPage = learners.Count / totalPages;
            var startIndex = (pageNumber - 1) * itemsPerPage;

            var sliceOfLearners = learners.GetRange(startIndex, itemsPerPage);

            var paginationHeaderValue = $"{{\"TotalItems\":{learners.Count},\"PageNumber\":{pageNumber},\"PageSize\":{itemsPerPage},\"TotalPages\":{totalPages}}}";
            var paginationHeader = new Dictionary<string, IEnumerable<string>> { { "X-Pagination", new[] { paginationHeaderValue } } };
            var apiResponse = new ApiResponse<List<LearnerResponse>>(sliceOfLearners, System.Net.HttpStatusCode.OK, null, paginationHeader);

            _apiClientMock.Setup(x => x.GetWithResponseCode<List<LearnerResponse>>(It.Is<GetLearnersRequest>(r => RequestPageNumberIs(r,pageNumber)))).ReturnsAsync(apiResponse);
        }

        private static bool RequestPageNumberIs(GetLearnersRequest request, int expectedPageNumber)
        {
            if(request.GetUrl.Contains($"pageNumber={expectedPageNumber}"))
            {
                return true;
            }
            return false;
        }
    }
}
