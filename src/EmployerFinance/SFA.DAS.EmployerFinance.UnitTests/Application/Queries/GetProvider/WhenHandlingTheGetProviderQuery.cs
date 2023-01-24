using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFinance.Application.Queries.GetProvider;
using SFA.DAS.EmployerFinance.InnerApi.Requests;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.EmployerFinance.UnitTests.Application.Queries.GetProvider
{
    public class WhenHandlingTheGetProviderQuery
    {
        private GetProviderQueryHandler _handler;
        private GetProviderQuery _query;
        private Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> _courseDeliveryApiClient;
        private GetProvidersListItem _apiResponse;

        [SetUp]
        public void Setup()
        {
            var fixture = new Fixture();

            _query = fixture.Create<GetProviderQuery>();
            _apiResponse = fixture.Create<GetProvidersListItem>();

            _courseDeliveryApiClient = new Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>>();
            _courseDeliveryApiClient.Setup(x =>
                    x.Get<GetProvidersListItem>(
                        It.Is<GetProviderRequest>(c => c.GetUrl.Equals($"api/providers/{_query.Id}"))))
                .ReturnsAsync(_apiResponse);

            _handler = new GetProviderQueryHandler(_courseDeliveryApiClient.Object);
        }

        [Test]
        public async Task Then_Providers_Are_Returned()
        {
            var actual = await _handler.Handle(_query, CancellationToken.None);

            actual.Should().BeEquivalentTo(_apiResponse);
        }
    }
}