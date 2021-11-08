using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.Interfaces;
using SFA.DAS.Vacancies.Application;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;
using SFA.DAS.Vacancies.InnerApi.Requests;
using SFA.DAS.Vacancies.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.Vacancies.UnitTests.Application.Vacancies.Queries
{
    public class WhenHandlingGetVacancies
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Vacancies_Returned(
            GetVacanciesQuery query,
            GetVacanciesResponse apiResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            GetVacanciesQueryHandler handler)
        {
            apiClient.Setup(x => x.Get<GetVacanciesResponse>(It.IsAny<GetVacanciesRequest>())).ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancies.Should().BeEquivalentTo(apiResponse.ApprenticeshipVacancies);

        }

        [Test, MoqAutoData]
        public async Task And_The_Account_Is_Provider_and_AccountLegalEntityPublicHashedId_Is_Null(
            GetVacanciesQuery query,
            GetProviderAccountLegalEntitiesResponse apiResponse, [Frozen] Mock<AccountsApiClient> accountsApi,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            GetVacanciesQueryHandler handler)

        {
            apiClient.Setup(x => x.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>())).ReturnsAsync(apiResponse);

            accountsApi.Verify(x => x.Get<AccountDetail>(It.IsAny<GetAccountDetailRequest>()), Times.Never);
        }
    }
}
