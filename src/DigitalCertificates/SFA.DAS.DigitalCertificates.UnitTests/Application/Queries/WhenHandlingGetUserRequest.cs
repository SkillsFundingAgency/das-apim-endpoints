using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetUser;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries
{
    public class WhenHandlingGetUserRequest
    {
        [Test, MoqAutoData]
        public async Task Then_Get_User_From_The_Api_By_GovUkIdentifier(
           string govUkIdentifier,
           User user,
           GetUserQuery query,
           [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
           GetUserQueryHandler handler)
        {
            // Arrange
            query.GovUkIdentifier = govUkIdentifier;

            var response = new ApiResponse<User>(user, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.GetWithResponseCode<User>(It.Is<GetUserRequest>(r => r.GovUkIdentifier == query.GovUkIdentifier)))
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
                .Setup(client => client.GetWithResponseCode<User>(It.IsAny<GetUserRequest>()))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.NotFound, "Not Found"));

            // Act & Assert
            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);
            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.NotFound);
        }
    }
}
