using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionHistory;
using SFA.DAS.EmployerAccounts.InnerApi.Requests;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetEnglishFractionHistory
{
    [TestFixture]
    public class WhenCallingHandler
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_English_Fractions_History_From_Finance_Api(
            GetEnglishFractionHistoryQuery query,
            GetEnglishFractionHistoryResponse apiResponse,
            [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockApiClient,
            GetEnglishFractionHistoryQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetEnglishFractionHistoryResponse>(It.IsAny<GetEnglishFractionHistoryRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Fractions.Should().BeEquivalentTo(apiResponse);
        }
    }
}
