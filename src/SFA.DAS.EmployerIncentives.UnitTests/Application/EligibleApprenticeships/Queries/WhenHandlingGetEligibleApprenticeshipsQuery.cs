using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Queries;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Queries
{
    public class WhenHandlingGetEligibleApprenticeshipsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Eligible_Apprenticeships_Are_Returned(
            GetEligibleApprenticeshipsSearchQuery query,
            GetApprenticeshipListResponse response,
            ApprenticeshipItem[] items,
            [Frozen] Mock<ICommitmentsApiClient<CommitmentsConfiguration>> commitmentsClient,
            [Frozen] Mock<IEmployerIncentivesService> employerIncentivesService,
            GetEligibleApprenticeshipsSearchHandler handler
            )
        {
            commitmentsClient.Setup(client =>
                client.Get<GetApprenticeshipListResponse>(It.Is<GetApprenticeshipsRequest>(c =>
                    c.GetUrl.Contains(query.AccountId.ToString()) &&
                    c.GetUrl.Contains(query.AccountLegalEntityId.ToString()))))
                .ReturnsAsync(response);
            employerIncentivesService.Setup(x =>
                x.GetEligibleApprenticeships(
                    It.Is<IEnumerable<ApprenticeshipItem>>(c => c.Count().Equals(response.Apprenticeships.Count())),//
                    It.IsAny<CancellationToken>())).ReturnsAsync(items);
            
            var actual = await handler.Handle(query, CancellationToken.None);
            
            actual.Apprentices.Should().BeEquivalentTo(items);
        }
        
    }
}