using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.CommitmentPayment.Queries.GetDataLockEvents;
using SFA.DAS.Approvals.InnerApi.Responses.ProviderEvent;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.CommitmentPayment.Queries
{
    public class WhenGettingDataLockEvents
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_DataLockEvents_Returned(
            GetDataLockStatuesQuery query,
            GetDataLockStatusesQueryResult result,
            PageOfResults<DataLockStatusEvent> apiPagedResponse,
            [Frozen] Mock<IProviderPaymentEventsApiClient<ProviderEventsConfiguration>> apiClient,
            GetDataLockStatusesQueryHandler handler
        )
        {
            var apiResponse =
                new ApiResponse<PageOfResults<DataLockStatusEvent>>(apiPagedResponse, HttpStatusCode.OK, string.Empty);

            apiClient
                .Setup(x => x.GetWithResponseCode<PageOfResults<DataLockStatusEvent>>(It.IsAny<GetDataLockEventsRequest>()))
                .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.PagedDataLockEvent.Should().BeEquivalentTo(apiResponse);
        }
    }
}