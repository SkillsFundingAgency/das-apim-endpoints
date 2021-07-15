using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    public class WhenCallingGetApprenticeshipIncentives
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_Apprenticeships_For_The_Account(
            long accountId,
            long accountLegalEntityId,
            IEnumerable<ApprenticeshipIncentiveDto> apiResponse,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service
        )
        {
            client.Setup(x =>
                    x.GetAll<ApprenticeshipIncentiveDto>(
                        It.Is<GetApprenticeshipIncentivesRequest>(c => c.GetAllUrl.Contains(accountId.ToString()) && c.GetAllUrl.Contains(accountLegalEntityId.ToString()))))
                .ReturnsAsync(apiResponse);

            var actual = await service.GetApprenticeshipIncentives(accountId, accountLegalEntityId);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}