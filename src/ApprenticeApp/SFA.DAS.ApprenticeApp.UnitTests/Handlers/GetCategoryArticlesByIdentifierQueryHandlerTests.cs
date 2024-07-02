using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetCategoryArticlesByIdentifierQueryHandlerTests
    {
        public async Task GetCategoryArticlesByIdentifierQueryHandlerTestsCheck(
        [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> client,
            GetCategoryArticlesByIdentifierQueryHandler sut,
            GetCategoryArticlesByIdentifierQuery query,
            CancellationToken cancellationToken
        )
        {
            await sut.Handle(query, cancellationToken);
            sut.Should().NotBeNull();
        }
    }
}