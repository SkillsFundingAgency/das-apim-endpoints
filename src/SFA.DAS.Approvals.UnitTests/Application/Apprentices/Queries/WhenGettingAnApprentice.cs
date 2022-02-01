using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Apprentices.Queries;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Queries
{
    public class WhenGettingAnApprentice
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_The_Apprentice_Is_Returned(
            GetApprenticeQuery query,
            GetApprenticeResponse apiResponse,
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClient,
            GetApprenticeQueryHandler handler
            )
        {
            apiClient.Setup(x => x.Get<GetApprenticeResponse>(It.Is<GetApprenticeRequest>(x=>x.ApprenticeId == query.ApprenticeId))).ReturnsAsync(apiResponse);

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
            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeNull();
        }
    }
}