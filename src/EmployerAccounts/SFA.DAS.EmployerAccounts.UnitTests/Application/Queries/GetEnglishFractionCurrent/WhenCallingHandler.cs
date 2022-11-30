using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionCurrent;
using SFA.DAS.EmployerAccounts.InnerApi.Requests;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetEnglishFractionCurrent
{
    [TestFixture]
    public class WhenCallingHandler
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_English_Fraction_Current_From_Finance_Api(
            GetEnglishFractionCurrentQuery query,
            GetEnglishFractionCurrentResponse apiResponse,
            [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockApiClient,
            GetEnglishFractionCurrentQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetEnglishFractionCurrentResponse>(It.IsAny<GetEnglishFractionCurrentRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Fractions.Should().BeEquivalentTo(apiResponse);
        }
    }
}
