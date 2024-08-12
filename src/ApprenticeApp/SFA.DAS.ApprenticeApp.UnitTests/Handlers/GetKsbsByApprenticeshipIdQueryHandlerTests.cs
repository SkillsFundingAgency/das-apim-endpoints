using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetKsbsByApprenticeshipIdQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task GetKsbsByStandardId
           (GetKsbsByApprenticeshipIdQueryHandler sut,
           GetKsbsByApprenticeshipIdQuery query,
           CancellationToken cancellationToken)
        {
            query.ApprenticeshipId = Guid.Empty;

            await sut.Handle(query, cancellationToken);
            sut.Should().NotBeNull();
        }
    }
}
