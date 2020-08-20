using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntities;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Queries
{
    public class WhenHandlingGetAccountLegalEntitiesQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_AccountLegalEntities_Are_Returned(
            GetLegalEntitiesQuery query,
            AccountLegalEntity[] response,
            [Frozen] Mock<IEmployerIncentivesService> employerIncentivesService,
            GetLegalEntitiesHandler handler
        )
        {
            employerIncentivesService.Setup(x =>
                x.GetAccountLegalEntities(query.AccountId)).ReturnsAsync(response);
            
            var actual = await handler.Handle(query, CancellationToken.None);
            
            actual.AccountLegalEntities.Should().BeEquivalentTo(response);
        }
    }
}