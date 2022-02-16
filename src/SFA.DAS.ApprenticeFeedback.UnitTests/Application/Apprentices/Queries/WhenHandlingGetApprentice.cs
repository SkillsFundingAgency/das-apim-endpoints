using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprentice;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Apprentices.Queries
{
    public class WhenHandlingGetApprentice
    {
        [Test, MoqAutoData]
        public async Task Then_TheApiIsCalledWithTheRequest_And_ReturnsApprentice(
                GetApprenticeQuery query,
                [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClient,
                GetApprenticeQueryHandler handler,
                GetApprenticeResponse apiResponse)
        {
            apiClient.Setup(x => x.Get<GetApprenticeResponse>(It.Is<GetApprenticeRequest>(x => x.Id == query.ApprenticeId))).ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeEquivalentTo(apiResponse);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_No_Apprentice_Is_Returned(
            GetApprenticeQuery query,
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClient,
            GetApprenticeQueryHandler handler
        )
        {
            apiClient.Setup(x => x.Get<GetApprenticeResponse>(It.Is<GetApprenticeRequest>(x => x.Id == query.ApprenticeId)))
                .ReturnsAsync((GetApprenticeResponse)null);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeNull();
        }
    }
}
