using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetKsbsByApprenticeshipIdAndGuidListQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task GetKsbsByStandardId
           (GetKsbsByApprenticeshipIdAndGuidListQueryHandler sut,
           GetKsbsByApprenticeshipIdAndGuidListQuery query,
           CancellationToken cancellationToken)
        {
            query.ApprenticeshipId = 1;
            query.Guids = new Guid[] { Guid.Empty };

            await sut.Handle(query, cancellationToken);
            sut.Should().NotBeNull();
        }
    }
}
