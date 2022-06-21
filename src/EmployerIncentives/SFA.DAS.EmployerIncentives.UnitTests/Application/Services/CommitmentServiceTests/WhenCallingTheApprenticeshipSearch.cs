using System;
using System.Net;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.CommitmentServiceTests
{
    [TestFixture] 
    public class WhenCallingTheApprenticeshipSearch
    {

        [Test, MoqAutoData]
        public async Task Then_Returns_The_Apprenticeships(
            long accountId,
            long accountLegalEntityId,
            DateTime startDateFrom,
            DateTime startDateTo,
            GetApprenticeshipListResponse response, 
            [Frozen] Mock<ICommitmentsApiClient<CommitmentsConfiguration>> client,
            [Greedy] CommitmentsService sut)
        {
            client.Setup(x => x.Get< GetApprenticeshipListResponse>(It.Is<IGetApiRequest>(p => 
                    p.GetUrl == $"api/apprenticeships?accountId={accountId}&accountLegalEntityId={accountLegalEntityId}&startDateRangeFrom={WebUtility.UrlEncode(startDateFrom.ToString("u"))}&startDateRangeTo={WebUtility.UrlEncode(startDateTo.ToString("u"))}")))
                .ReturnsAsync(response);

            var result = await sut.Apprenticeships(accountId, accountLegalEntityId, startDateFrom, startDateTo);
            result.Should().BeEquivalentTo(response.Apprenticeships);
        }
    }
}
