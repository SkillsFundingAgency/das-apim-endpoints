using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using SFA.DAS.ApprenticeApp.Client;
using SFA.DAS.ApprenticeApp.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetCategoryArticlesByIdentifierQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task GetCategoryArticlesByIdentifierQueryHandlerTestsCheck(
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> client,
            [Frozen] Mock<IContentService> contentService,
            [Frozen] Mock<IContentClient> contentClient,
            GetCategoryArticlesByIdentifierQueryHandler sut,
            GetCategoryArticlesByIdentifierQuery query,
            CancellationToken cancellationToken
        )
        {
            contentClient.Setup(x =>
                               x.GetBySlugWithChildren("type").Result);
            await sut.Handle(query, cancellationToken);
            sut.Should().NotBeNull();
        } 
    }
}