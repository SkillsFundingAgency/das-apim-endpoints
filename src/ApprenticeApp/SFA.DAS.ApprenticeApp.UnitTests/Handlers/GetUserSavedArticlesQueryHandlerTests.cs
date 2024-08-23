using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetUserSavedArticlesQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task GetUserSavedArticlesQueryHandlerTestsCheck(
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> client,
            GetUserSavedArticlesQueryHandler sut,
            GetUserSavedArticlesQuery query,
            CancellationToken cancellationToken
        )
        {
            await sut.Handle(query, cancellationToken);
            sut.Should().NotBeNull();
        }
    }
}