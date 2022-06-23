using System.Collections.Generic;
using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.DeliveryModels.Queries;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.DeliveryModels.Constants;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Approvals.UnitTests.Application.DeliveryModels.Queries
{
    public class WhenGettingDeliveryModels
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_DeliveryModels_Returned(
            GetDeliveryModelsQuery query,
            GetDeliveryModelsResponse apiResponse,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
            [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
            long maLegalEntityId,
            GetDeliveryModelsQueryHandler handler
        )
        {
            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync(apiResponse);

            accountsApiClient.Setup(x => x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new GetAccountLegalEntityResponse { MaLegalEntityId = maLegalEntityId });

            fjaaApiClient.Setup(x => x
                    .GetWithResponseCode<GetAgencyResponse>(It.IsAny<GetAgencyRequest>()))
                .ReturnsAsync(new ApiResponse<GetAgencyResponse>(null, HttpStatusCode.NotFound, string.Empty));

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.DeliveryModels.Should().BeEquivalentTo(apiResponse.DeliveryModels);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_When_No_Response_Returned_We_Create_Default(
            GetDeliveryModelsQuery query,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
            [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
            long maLegalEntityId,
            GetDeliveryModelsQueryHandler handler
        )
        {
            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync((GetDeliveryModelsResponse)null);

            accountsApiClient.Setup(x => x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new GetAccountLegalEntityResponse { MaLegalEntityId = maLegalEntityId });

            fjaaApiClient.Setup(x => x
                    .GetWithResponseCode<GetAgencyResponse>(It.IsAny<GetAgencyRequest>()))
                .ReturnsAsync(new ApiResponse<GetAgencyResponse>(null, HttpStatusCode.NotFound, string.Empty));

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeEquivalentTo(new { DeliveryModels = new[] { DeliveryModelStringTypes.Regular } });
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_When_Null_Response_Returned_We_Create_Default(
            GetDeliveryModelsQuery query,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
            [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
            long maLegalEntityId,
            GetDeliveryModelsQueryHandler handler
        )
        {
            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync(new GetDeliveryModelsResponse { DeliveryModels = null });

            accountsApiClient.Setup(x => x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new GetAccountLegalEntityResponse { MaLegalEntityId = maLegalEntityId });

            fjaaApiClient.Setup(x => x
                    .GetWithResponseCode<GetAgencyResponse>(It.IsAny<GetAgencyRequest>()))
                .ReturnsAsync(new ApiResponse<GetAgencyResponse>(null, HttpStatusCode.NotFound, string.Empty));

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeEquivalentTo(new { DeliveryModels = new[] { DeliveryModelStringTypes.Regular } });
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_And_When_Empty_Response_Returned_We_Create_Default(
            GetDeliveryModelsQuery query,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
            [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
            long maLegalEntityId,
            GetDeliveryModelsQueryHandler handler)
        {
            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync(new GetDeliveryModelsResponse { DeliveryModels = new System.Collections.Generic.List<string>() { DeliveryModelStringTypes.Regular } });

            accountsApiClient.Setup(x => x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new GetAccountLegalEntityResponse { MaLegalEntityId = maLegalEntityId });

            fjaaApiClient.Setup(x => x
                .GetWithResponseCode<GetAgencyResponse>(It.IsAny<GetAgencyRequest>()))
                .ReturnsAsync(new ApiResponse<GetAgencyResponse>(null, HttpStatusCode.NotFound, string.Empty));

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.DeliveryModels.Should().BeEquivalentTo(DeliveryModelStringTypes.Regular);
        }


        [Test, MoqAutoData]
        public async Task Then_FlexiJobAgency_Is_Returned_For_LegalEntity_On_FJAA_Register(
            GetDeliveryModelsQuery query,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
            [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
            long maLegalEntityId,
            GetDeliveryModelsQueryHandler handler)
        {
            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync(new GetDeliveryModelsResponse { DeliveryModels = new System.Collections.Generic.List<string>() { DeliveryModelStringTypes.Regular } });

            accountsApiClient.Setup(x => x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new GetAccountLegalEntityResponse { MaLegalEntityId = maLegalEntityId });

            fjaaApiClient.Setup(x => x
                    .GetWithResponseCode<GetAgencyResponse>(It.IsAny<GetAgencyRequest>()))
                .ReturnsAsync(new ApiResponse<GetAgencyResponse>(new GetAgencyResponse(), HttpStatusCode.OK, string.Empty));

            var actual = await handler.Handle(query, CancellationToken.None);

            var expected = new List<string> { DeliveryModelStringTypes.Regular, DeliveryModelStringTypes.FlexiJobAgency };

            actual.DeliveryModels.Should().BeEquivalentTo(expected);
        }

        [Test, MoqAutoData]
        public async Task Then_PortableFlexiJob_Is_Returned_For_LegalEntity_On_FJAA_Register(
            GetDeliveryModelsQuery query,
            [Frozen] Mock<IProviderCoursesApiClient<ProviderCoursesApiConfiguration>> apiClient,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
            [Frozen] Mock<IFjaaApiClient<FjaaApiConfiguration>> fjaaApiClient,
            long maLegalEntityId,
            GetDeliveryModelsQueryHandler handler)
        {
            apiClient
                .Setup(x => x.Get<GetDeliveryModelsResponse>(It.IsAny<GetDeliveryModelsRequest>()))
                .ReturnsAsync(new GetDeliveryModelsResponse { DeliveryModels = new List<string>() { DeliveryModelStringTypes.Regular, DeliveryModelStringTypes.PortableFlexiJob } });

            accountsApiClient.Setup(x => x.Get<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntityRequest>()))
                .ReturnsAsync(new GetAccountLegalEntityResponse { MaLegalEntityId = maLegalEntityId });

            fjaaApiClient.Setup(x => x
                    .GetWithResponseCode<GetAgencyResponse>(It.IsAny<GetAgencyRequest>()))
                .ReturnsAsync(new ApiResponse<GetAgencyResponse>(new GetAgencyResponse(), HttpStatusCode.OK, string.Empty));

            var actual = await handler.Handle(query, CancellationToken.None);

            var expected = new List<string> { DeliveryModelStringTypes.Regular, DeliveryModelStringTypes.FlexiJobAgency };

            actual.DeliveryModels.Should().BeEquivalentTo(expected);
        }
    }
}