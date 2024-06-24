using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class SupportAndGuidanceHandlerTests
    {
        [Test, MoqAutoData]
        public async Task GetCategoriesByContentTypeQueryTest(
            GetCategoriesByContentTypeQueryHandler sut,
            GetCategoriesByContentTypeQuery query,
            CancellationToken cancellationToken)
        {
            query.ContentType = "1234";

            await sut.Handle(query, cancellationToken);

            sut.Should().NotBeNull();
        }

        [Test, MoqAutoData]
        public async Task GetContentQueryTest(
            GetContentQueryHandler sut,
            GetContentQuery query,
            CancellationToken cancellationToken)
        {
            query.EntryId = "one";

            await sut.Handle(query, cancellationToken);

            sut.Should().NotBeNull();
        }
    }
}