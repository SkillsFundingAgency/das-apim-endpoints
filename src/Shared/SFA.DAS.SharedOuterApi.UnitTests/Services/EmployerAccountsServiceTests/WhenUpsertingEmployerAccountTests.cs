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
using SFA.DAS.SharedOuterApi.Infrastructure;
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
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
            EmployerAccountsService handler)
        {
            employerProfile.UserId = Guid.NewGuid().ToString();

            accountsApiClient
                .Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<PutAccountUserRequest>()))
                .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.Created, ""));
            employerProfilesApiClient.Setup(x => x.PutWithResponseCode<EmployerProfileUsersApiResponse>(
                It.Is<PutUpsertEmployerUserAccountRequest>(c =>
                    c.PutUrl.Contains($"api/users/{HttpUtility.UrlEncode(employerProfile.UserId)}") 
                    && c.Data.GetType().GetProperty("GovIdentifier").GetValue(c.Data, null).ToString() == employerProfile.GovIdentifier
                    && c.Data.GetType().GetProperty("FirstName").GetValue(c.Data, null).ToString() == employerProfile.FirstName
                    && c.Data.GetType().GetProperty("LastName").GetValue(c.Data, null).ToString() == employerProfile.LastName
                    && c.Data.GetType().GetProperty("Email").GetValue(c.Data, null).ToString() == employerProfile.Email
                    )))
                .ReturnsAsync(new ApiResponse<EmployerProfileUsersApiResponse>(profileUserResponse, HttpStatusCode.OK, ""));
            
            var actual = (await handler.PutEmployerAccount(employerProfile));

            actual.FirstName.Should().Be(profileUserResponse.FirstName);
            actual.LastName.Should().Be(profileUserResponse.LastName);
            actual.Email.Should().Be(profileUserResponse.Email);
            actual.UserId.Should().Be(profileUserResponse.GovUkIdentifier);
           
            employerProfilesApiClient.Verify(x => x.PutWithResponseCode<EmployerProfileUsersApiResponse>(
                It.IsAny<PutUpsertEmployerUserAccountRequest>()), Times.Once);
            accountsApiClient.Verify(x=>x.PutWithResponseCode<NullResponse>(
                It.Is<PutAccountUserRequest>(c=>
                    c.PutUrl.Contains("api/user/upsert")
                    && c.Data.GetType().GetProperty("CorrelationId").GetValue(c.Data, null).ToString() == employerProfile.CorrelationId.Value.ToString()
                    && c.Data.GetType().GetProperty("UserRef").GetValue(c.Data, null).ToString() == employerProfile.UserId
                    && c.Data.GetType().GetProperty("FirstName").GetValue(c.Data, null).ToString() == employerProfile.FirstName
                    && c.Data.GetType().GetProperty("LastName").GetValue(c.Data, null).ToString() == employerProfile.LastName
                    && c.Data.GetType().GetProperty("EmailAddress").GetValue(c.Data, null).ToString() == employerProfile.Email
                )), Times.Once);
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