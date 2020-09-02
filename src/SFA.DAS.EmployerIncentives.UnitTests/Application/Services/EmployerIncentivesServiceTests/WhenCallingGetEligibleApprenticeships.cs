using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    public class WhenCallingGetEligibleApprenticeships
    {
        [Test,MoqAutoData]
        public async Task Then_Each_Apprentice_Record_Is_Checked(
            long accountId,
            long accountLegalEntityId,
            List<ApprenticeshipItem> allApprenticeship,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service)
        {
            //Arrange
            var eligibleApprenticeships = (from apprenticeship in allApprenticeship
                                           select new EligibleApprenticeshipResult { Eligible = true, Uln = apprenticeship.Uln }).ToList();
            client.Setup(x =>
                    x.Post<IEnumerable<EligibleApprenticeshipResult>>(It.IsAny<GetMultipleEligibleApprenticeshipsRequest>()))
                .ReturnsAsync(eligibleApprenticeships);
            
            //Act
            var actual = await service.GetEligibleApprenticeships(accountId, accountLegalEntityId, allApprenticeship);
            
            //Assert
            actual.Count().Should().Be(eligibleApprenticeships.Count());
            client.Verify(x => x.Post<IEnumerable<EligibleApprenticeshipResult>>(It.IsAny<GetMultipleEligibleApprenticeshipsRequest>()), Times.Once);
        }
        
    }
}