using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUser;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;

using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries
{
    public class WhenHandlingGetUsersByGovUkIdentifierApiRequest
    {
        [Test, MoqAutoData]
        public async Task Then_Get_User_From_The_Api_By_GovUkIdentifier(
           string govUkIdentifier,
           GetUserResponse user,
           GetUserQuery query,
           [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
           GetUserQueryHandler handler)
        {
            // Arrange
            query.GovUkIdentifier = govUkIdentifier;

            var response = new ApiResponse<GetUserResponse>(user, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.GetWithResponseCode<GetUserResponse>(It.Is<GetUsersByGovUkIdentifierApiRequest>(r => r.GovUkIdentifier == query.GovUkIdentifier)))
                .ReturnsAsync(response);

            // Act
            var actual = await handler.Handle(query, CancellationToken.None);

            // Assert
            actual.User.Should().BeEquivalentTo(user, options => options.ExcludingMissingMembers());
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            GetUserQuery query,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            GetUserQueryHandler handler)
        {
            // Arrange
            mockDigitalCertificatesApiClient
                .Setup(client => client.GetWithResponseCode<GetUserResponse>(It.IsAny<GetUsersByGovUkIdentifierApiRequest>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.NotFound, "Not Found"));

            // Act & Assert
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.NotFound);
        }
    }
}
