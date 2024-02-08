using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetLatestDetails;
using SFA.DAS.EmployerAccounts.Exceptions;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ReferenceData;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetLatestDetails
{
    public class WhenCallingHandler
    {
        [Test, MoqAutoData]
        public async Task Then_GetLatestDetails_from_Reference_Api(
          GetLatestDetailsQuery query,
          ApiResponse<Organisation> apiResponse,
          [Frozen] Mock<IReferenceDataApiClient<ReferenceDataApiConfiguration>> mockApiClient,
          GetLatestDetailsQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.GetWithResponseCode<Organisation>(It.IsAny<GetLatestDetailsRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.OrganisationDetail.Should().BeEquivalentTo(apiResponse.Body);
        }

        [Test, MoqAutoData]
        public void Then_Throw_OrganisationNotFoundException_When_NotFound_Status_Code_Returned(
        string errorMessage,
        GetLatestDetailsQuery query,
        [Frozen] Mock<IReferenceDataApiClient<ReferenceDataApiConfiguration>> mockApiClient,
        GetLatestDetailsQueryHandler handler)
        {
            var notFoundResponse = new ApiResponse<Organisation>(null, HttpStatusCode.NotFound, errorMessage);

            mockApiClient
                .Setup(client => client.GetWithResponseCode<Organisation>(It.IsAny<GetLatestDetailsRequest>()))
                .ReturnsAsync(notFoundResponse);

            Func<Task> action = async () => await handler.Handle(query, CancellationToken.None);

            action.Should().Throw<OrganisationNotFoundException>()
                .WithMessage($"Did not find an organisation type Company with identifier {query.Identifier}");
        }

        [Test, MoqAutoData]
        public void Then_ThrowInvalidGetOrganisationRequest_When_BadRequest_Status_Code_Returned(
        string errorMessage,
        GetLatestDetailsQuery query,
        [Frozen] Mock<IReferenceDataApiClient<ReferenceDataApiConfiguration>> mockApiClient,
        GetLatestDetailsQueryHandler handler)
        {
            var badRequestResponse = new ApiResponse<Organisation>(null, HttpStatusCode.BadRequest, errorMessage);

            mockApiClient
                .Setup(client => client.GetWithResponseCode<Organisation>(It.IsAny<GetLatestDetailsRequest>()))
                .ReturnsAsync(badRequestResponse);

            Func<Task> action = async () => await handler.Handle(query, CancellationToken.None);

            action.Should().Throw<InvalidGetOrganisationException>()
                .WithMessage(errorMessage);
        }
    }
}
