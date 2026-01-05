using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.Application.Queries.GetEducationalOrganisationsByLepCode;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EarlyConnect.UnitTests.Application.GetEducationalOrganisationDataByLepCode
{
    public class WhenGettingEducationalOrganisationDataByLepCode
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_EducationalOrganisationData_Is_Returned(
            GetEducationalOrganisationsByLepCodeQuery query,
            GetEducationalOrganisationsByLepCodeResponse apiResponse,
            [Frozen] Mock<IEarlyConnectApiClient<EarlyConnectApiConfiguration>> apiClient,
            GetEducationalOrganisationsByLepCodeQueryHandler handler
        )
        {
            apiClient.Setup(x => x.GetWithResponseCode<GetEducationalOrganisationsByLepCodeResponse>(
                    It.Is<GetEducationalOrganisationsByLepCodeRequest>(r => r.LepCode == query.LepCode && r.SearchTerm == query.SearchTerm)))
                .ReturnsAsync(new ApiResponse<GetEducationalOrganisationsByLepCodeResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.EducationalOrganisations.Should().BeEquivalentTo(apiResponse.EducationalOrganisations, options => options.ExcludingMissingMembers());

            apiClient.Verify(x => x.GetWithResponseCode<GetEducationalOrganisationsByLepCodeResponse>(
                It.Is<GetEducationalOrganisationsByLepCodeRequest>(r => r.LepCode == query.LepCode && r.SearchTerm == query.SearchTerm)), Times.Once);
        }

    }
}
