using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntity;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Queries
{
    public class WhenHandlingGetLegalEntityQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_AccountLegalEntity_Is_Returned(
            GetLegalEntityQuery query,
            AccountLegalEntity response,
            [Frozen] Mock<ILegalEntitiesService> legalEntitiesService,
            GetLegalEntityHandler handler
        )
        {
            legalEntitiesService.Setup(x =>
                x.GetLegalEntity(query.AccountId, query.AccountLegalEntityId)).ReturnsAsync(response);
            
            var actual = await handler.Handle(query, CancellationToken.None);
            
            actual.AccountLegalEntity.Should().BeEquivalentTo(response);
        }
    }
}