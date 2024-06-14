using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetLatestDetails;
using SFA.DAS.EmployerAccounts.Strategies;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetLatestDetails
{
    public class WhenCallingHandler
    {
        [Test, MoqAutoData]
        public async Task Then_GetLatestDetails_from_Reference_Api(
          GetLatestDetailsQuery query,
          GetLatestDetailsResult expectedResult,
          [Frozen] Mock<IOrganisationApiStrategyFactory> factory,
          Mock<IOrganisationApiStrategy> strategy,
          GetLatestDetailsQueryHandler handler)
        {
            factory.Setup(x => x.CreateStrategy(query.OrganisationType)).Returns(strategy.Object);

            strategy.Setup(x => x.GetOrganisationDetails(query.Identifier, query.OrganisationType)).ReturnsAsync(expectedResult);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
