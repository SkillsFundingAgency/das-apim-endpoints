using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Applications.GetLegacyApplications;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Applications
{
    [TestFixture]
    public class WhenHandlingGetLegacyApplicationsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            GetLegacyApplicationsQuery query,
            GetLegacyApplicationsByEmailApiResponse applicationsApiResponse,
            [Frozen] Mock<ILegacyApplicationMigrationService> vacancyMigrationService,
            GetLegacyApplicationsQueryHandler handler)
        {  
            vacancyMigrationService.Setup(client => client.GetLegacyApplications(query.EmailAddress))
                .ReturnsAsync(applicationsApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();
            result.Applications.Should().BeEquivalentTo(applicationsApiResponse.Applications);
        }
    }
}
