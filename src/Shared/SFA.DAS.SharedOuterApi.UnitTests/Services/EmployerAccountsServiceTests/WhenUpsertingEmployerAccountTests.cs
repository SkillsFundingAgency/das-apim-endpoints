using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.SharedOuterApi.UnitTests.Services.EmployerAccountsServiceTests
{
    public class WhenUpsertingEmployerAccountTests
    {
        [Test, MoqAutoData]
        public async Task When_Given_EmployerProfile_Then_Account_Found_And_Upserted_Should_Return_EmployerProfile(
            EmployerProfile employerProfile,
            EmployerProfileUsersApiResponse profileUserResponse,
            [Frozen] Mock<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>> employerProfilesApiClient,
            EmployerAccountsService handler)
        {
            employerProfile.UserId = Guid.NewGuid().ToString();
            
            employerProfilesApiClient.Setup(x => x.PutWithResponseCode<EmployerProfileUsersApiResponse>(
                It.Is<PutUpsertEmployerUserAccountRequest>(c =>
                    c.PutUrl.Contains($"api/users/{HttpUtility.UrlEncode(employerProfile.UserId)}"))))
                .ReturnsAsync(new ApiResponse<EmployerProfileUsersApiResponse>(profileUserResponse, HttpStatusCode.OK, ""));
            
            var actual = (await handler.PutEmployerAccount(employerProfile));

            actual.FirstName.Should().Be(profileUserResponse.FirstName);
            actual.LastName.Should().Be(profileUserResponse.LastName);
            actual.Email.Should().Be(profileUserResponse.Email);
            actual.UserId.Should().Be(profileUserResponse.GovUkIdentifier);
           
            employerProfilesApiClient.Verify(x => x.PutWithResponseCode<EmployerProfileUsersApiResponse>(
                It.IsAny<PutUpsertEmployerUserAccountRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task When_Given_EmployerProfile_Then_Account_Not_Found_And_Upserted_Should_Return_Null(
            EmployerProfile employerProfile,
            EmployerProfileUsersApiResponse profileUserResponse,
            [Frozen] Mock<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>> employerProfilesApiClient,
            EmployerAccountsService handler)
        {
            employerProfile.UserId = Guid.NewGuid().ToString();

            employerProfilesApiClient.Setup(x => x.PutWithResponseCode<EmployerProfileUsersApiResponse>(
                    It.Is<PutUpsertEmployerUserAccountRequest>(c =>
                        c.PutUrl.Contains($"api/users/{HttpUtility.UrlEncode(employerProfile.UserId)}"))))
                .ReturnsAsync(new ApiResponse<EmployerProfileUsersApiResponse>(null, HttpStatusCode.OK, ""));

            var actual = (await handler.PutEmployerAccount(employerProfile));

            actual.Should().Be(null);

            employerProfilesApiClient.Verify(x => x.PutWithResponseCode<EmployerProfileUsersApiResponse>(
                It.IsAny<PutUpsertEmployerUserAccountRequest>()), Times.Once);
        }
    }
}