using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.CourseOptionKsbs;
using SFA.DAS.Testing.AutoFixture;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetStandardOptionKsbsQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task GetKsbsByStandardId
            (GetStandardOptionKsbsQueryHandler sut,
            GetStandardOptionKsbsQuery query,
            CancellationToken cancellationToken)
        {
               query.Id = "StandardUid";
               query.Option ="core";

            await sut.Handle(query, cancellationToken);
            sut.Should().NotBeNull();
        }
    }
}
