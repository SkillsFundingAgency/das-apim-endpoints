using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.ApprenticeshipDetails;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetApprenticeshipQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task GetApprenticeshipQuery
          (GetApprenticeshipQueryHandler sut,
          GetApprenticeshipQuery query,
          CancellationToken cancellationToken)
        {
            query.ApprenticeshipId = 1;

            await sut.Handle(query, cancellationToken);
            sut.Should().NotBeNull();
        }
    }
}
