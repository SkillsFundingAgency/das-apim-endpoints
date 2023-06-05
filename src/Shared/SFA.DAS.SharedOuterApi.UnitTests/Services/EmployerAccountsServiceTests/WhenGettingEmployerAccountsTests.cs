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
    public class WhenGettingEmployerAccountsTests
    {
        
        [Test, MoqAutoData]
        public async Task Then_If_The_Id_Is_Not_A_Guid_Then_User_Account_Is_Retrieved(
            EmployerProfile employerProfile,
            List<GetUserAccountsResponse> apiResponse,
            GetAccountTeamMembersResponse teamResponse,
            EmployerProfileUsersApiResponse profileUserResponse,
            [Frozen] Mock<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>> employerProfilesApiClient,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
            EmployerAccountsService handler)
        {
            employerProfile.UserId = new Guid().ToString();
            teamResponse.UserRef = profileUserResponse.Id;
            accountsApiClient
                .Setup(x => x.GetAll<GetUserAccountsResponse>(
                    It.Is<GetUserAccountsRequest>(c => c.GetAllUrl.Contains($"user/{profileUserResponse.Id}/accounts"))))
                .ReturnsAsync(apiResponse);
            accountsApiClient
                .Setup(x => x.GetAll<GetAccountTeamMembersResponse>(
                    It.Is<GetAccountTeamMembersRequest>(c => c.GetAllUrl.Contains($"accounts/{apiResponse.First().EncodedAccountId}/users"))))
                .ReturnsAsync(new List<GetAccountTeamMembersResponse>{teamResponse});
            employerProfilesApiClient.Setup(x => x.GetWithResponseCode<EmployerProfileUsersApiResponse>(
                    It.Is<GetEmployerUserAccountRequest>(c =>
                        c.GetUrl.Contains($"api/users/{HttpUtility.UrlEncode(employerProfile.UserId)}"))))
                .ReturnsAsync(new ApiResponse<EmployerProfileUsersApiResponse>(profileUserResponse, HttpStatusCode.OK, ""));

            employerProfilesApiClient.Setup(x => x.PutWithResponseCode<EmployerProfileUsersApiResponse>(
                    It.Is<PutUpsertEmployerUserAccountRequest>(c =>
                        c.PutUrl.Contains($"api/users/{HttpUtility.UrlEncode(employerProfile.UserId)}"))))
                .ReturnsAsync(new ApiResponse<EmployerProfileUsersApiResponse>(profileUserResponse, HttpStatusCode.OK, ""));

            var actual = (await handler.GetEmployerAccounts(employerProfile)).ToList();

            actual.First().Role.Should().Be(teamResponse.Role);
            actual.First().DasAccountName.Should().Be(apiResponse.First().DasAccountName);
            actual.First().EncodedAccountId.Should().Be(apiResponse.First().EncodedAccountId);
            actual.TrueForAll(c => c.UserId.Equals(profileUserResponse.Id)).Should().BeTrue();
            actual.TrueForAll(c => c.FirstName.Equals(profileUserResponse.FirstName)).Should().BeTrue();
            actual.TrueForAll(c => c.LastName.Equals(profileUserResponse.LastName)).Should().BeTrue();
            actual.TrueForAll(c => c.DisplayName.Equals(profileUserResponse.DisplayName)).Should().BeTrue();
            actual.TrueForAll(c => c.IsSuspended.Equals(profileUserResponse.IsSuspended)).Should().BeTrue();
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_The_Id_Is_Not_A_Guid_Then_User_Account_IsNot_Found_Then_Upserted(
            EmployerProfile employerProfile,
            List<GetUserAccountsResponse> apiResponse,
            GetAccountTeamMembersResponse teamResponse,
            EmployerProfileUsersApiResponse profileUserResponse,
            [Frozen] Mock<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>> employerProfilesApiClient,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
            EmployerAccountsService handler)
        {
            teamResponse.UserRef = profileUserResponse.Id;
            accountsApiClient
                .Setup(x => x.GetAll<GetUserAccountsResponse>(
                    It.Is<GetUserAccountsRequest>(c => c.GetAllUrl.Contains($"user/{profileUserResponse.Id}/accounts"))))
                .ReturnsAsync(apiResponse);
            accountsApiClient
                .Setup(x => x.GetAll<GetAccountTeamMembersResponse>(
                    It.Is<GetAccountTeamMembersRequest>(c => c.GetAllUrl.Contains($"accounts/{apiResponse.First().EncodedAccountId}/users"))))
                .ReturnsAsync(new List<GetAccountTeamMembersResponse>{teamResponse});
            employerProfilesApiClient.Setup(x => x.GetWithResponseCode<EmployerProfileUsersApiResponse>(
                    It.Is<GetEmployerUserAccountRequest>(c =>
                        c.GetUrl.Contains($"api/users/{HttpUtility.UrlEncode(employerProfile.UserId)}"))))
                .ReturnsAsync(new ApiResponse<EmployerProfileUsersApiResponse>(null, HttpStatusCode.NotFound, "Not Found"));
            employerProfilesApiClient.Setup(x => x.PutWithResponseCode<EmployerProfileUsersApiResponse>(
                    It.Is<PutUpsertEmployerUserAccountRequest>(c =>
                        c.PutUrl.Contains($"api/users/"))))
                .ReturnsAsync(new ApiResponse<EmployerProfileUsersApiResponse>(profileUserResponse, HttpStatusCode.Created, ""));

            var actual = (await handler.GetEmployerAccounts(employerProfile)).ToList();

            actual.First().Role.Should().Be(teamResponse.Role);
            actual.First().DasAccountName.Should().Be(apiResponse.First().DasAccountName);
            actual.First().EncodedAccountId.Should().Be(apiResponse.First().EncodedAccountId);
            actual.TrueForAll(c => c.UserId.Equals(profileUserResponse.Id)).Should().BeTrue();
            actual.TrueForAll(c => c.FirstName.Equals(profileUserResponse.FirstName)).Should().BeTrue();
            actual.TrueForAll(c => c.LastName.Equals(profileUserResponse.LastName)).Should().BeTrue();
            actual.TrueForAll(c => c.DisplayName.Equals(profileUserResponse.DisplayName)).Should().BeTrue();
            actual.TrueForAll(c => c.IsSuspended.Equals(profileUserResponse.IsSuspended)).Should().BeTrue();
        }
        [Test, MoqAutoData]
        public async Task Then_If_The_Id_Is_Not_A_Guid_And_A_Existing_User_With_Different_Email_Then_User_Upserted_And_User_Information_Returned(
            Guid userId,
            EmployerProfile employerProfile,
            EmployerProfileUsersApiResponse profileUserResponse,
            [Frozen] Mock<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>> employerProfilesApiClient,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
            EmployerAccountsService handler)
        {
            profileUserResponse.Id = userId.ToString();
            accountsApiClient
                .Setup(x => x.GetAll<GetUserAccountsResponse>(
                    It.Is<GetUserAccountsRequest>(c => c.GetAllUrl.Contains($"user/{profileUserResponse.Id}/accounts"))))
                .ReturnsAsync(new List<GetUserAccountsResponse>());
            accountsApiClient
                .Setup(x => x.GetAll<GetAccountTeamMembersResponse>(
                    It.IsAny<GetAccountTeamMembersRequest>()))
                .ReturnsAsync(new List<GetAccountTeamMembersResponse>());
            employerProfilesApiClient.Setup(x => x.GetWithResponseCode<EmployerProfileUsersApiResponse>(
                    It.Is<GetEmployerUserAccountRequest>(c =>
                        c.GetUrl.Contains($"api/users/{HttpUtility.UrlEncode(employerProfile.UserId)}"))))
                .ReturnsAsync(new ApiResponse<EmployerProfileUsersApiResponse>(profileUserResponse, HttpStatusCode.OK, ""));
            employerProfilesApiClient.Setup(x => x.PutWithResponseCode<EmployerProfileUsersApiResponse>(
                    It.Is<PutUpsertEmployerUserAccountRequest>(c =>
                        c.PutUrl.Contains($"api/users/{userId}")
                        && c.Data.GetType().GetProperty("GovIdentifier").GetValue(c.Data, null).ToString() == employerProfile.GovIdentifier
                        && c.Data.GetType().GetProperty("FirstName").GetValue(c.Data, null).ToString() == employerProfile.FirstName
                        && c.Data.GetType().GetProperty("LastName").GetValue(c.Data, null).ToString() == employerProfile.LastName
                        && c.Data.GetType().GetProperty("Email").GetValue(c.Data, null).ToString() == profileUserResponse.Email
                        )))
                .ReturnsAsync(new ApiResponse<EmployerProfileUsersApiResponse>(profileUserResponse, HttpStatusCode.Created, ""));

            var actual = (await handler.GetEmployerAccounts(employerProfile)).ToList();

            actual.Count.Should().Be(1);
            var actualRecord = actual.First();
            actualRecord.UserId.Should().Be(profileUserResponse.Id);
            actualRecord.FirstName.Should().Be(profileUserResponse.FirstName);
            actualRecord.LastName.Should().Be(profileUserResponse.LastName);
            actualRecord.DisplayName.Should().Be(profileUserResponse.DisplayName);
            actualRecord.IsSuspended.Should().Be(profileUserResponse.IsSuspended);
            actualRecord.DasAccountName.Should().BeNullOrEmpty();
            actualRecord.EncodedAccountId.Should().BeNullOrEmpty();
            actualRecord.Role.Should().BeNullOrEmpty();
            
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_The_Id_Is_Not_A_Guid_And_A_New_User_With_No_Accounts_Then_User_Upserted_And_User_Information_Returned(
            EmployerProfile employerProfile,
            EmployerProfileUsersApiResponse profileUserResponse,
            [Frozen] Mock<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>> employerProfilesApiClient,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
            EmployerAccountsService handler)
        {
            accountsApiClient
                .Setup(x => x.GetAll<GetUserAccountsResponse>(
                    It.Is<GetUserAccountsRequest>(c => c.GetAllUrl.Contains($"user/{profileUserResponse.Id}/accounts"))))
                .ReturnsAsync(new List<GetUserAccountsResponse>());
            accountsApiClient
                .Setup(x => x.GetAll<GetAccountTeamMembersResponse>(
                    It.IsAny<GetAccountTeamMembersRequest>()))
                .ReturnsAsync(new List<GetAccountTeamMembersResponse>());
            employerProfilesApiClient.Setup(x => x.GetWithResponseCode<EmployerProfileUsersApiResponse>(
                    It.Is<GetEmployerUserAccountRequest>(c =>
                        c.GetUrl.Contains($"api/users/{HttpUtility.UrlEncode(employerProfile.UserId)}"))))
                .ReturnsAsync(new ApiResponse<EmployerProfileUsersApiResponse>(null, HttpStatusCode.NotFound, "Not Found"));
            employerProfilesApiClient.Setup(x => x.PutWithResponseCode<EmployerProfileUsersApiResponse>(
                    It.Is<PutUpsertEmployerUserAccountRequest>(c =>
                        c.PutUrl.Contains($"api/users/"))))
                .ReturnsAsync(new ApiResponse<EmployerProfileUsersApiResponse>(profileUserResponse, HttpStatusCode.Created, ""));

            var actual = (await handler.GetEmployerAccounts(employerProfile)).ToList();

            actual.Count.Should().Be(1);
            var actualRecord = actual.First();
            actualRecord.UserId.Should().Be(profileUserResponse.Id);
            actualRecord.FirstName.Should().Be(profileUserResponse.FirstName);
            actualRecord.LastName.Should().Be(profileUserResponse.LastName);
            actualRecord.DisplayName.Should().Be(profileUserResponse.DisplayName);
            actualRecord.IsSuspended.Should().Be(profileUserResponse.IsSuspended);
            actualRecord.DasAccountName.Should().BeNullOrEmpty();
            actualRecord.EncodedAccountId.Should().BeNullOrEmpty();
            actualRecord.Role.Should().BeNullOrEmpty();
            
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_The_Id_Is_A_Guid_Then_User_Account_Found_And_Not_Upserted(
            EmployerProfile employerProfile,
            List<GetUserAccountsResponse> apiResponse,
            GetAccountTeamMembersResponse teamResponse,
            EmployerProfileUsersApiResponse profileUserResponse,
            [Frozen] Mock<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>> employerProfilesApiClient,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
            EmployerAccountsService handler)
        {
            employerProfile.UserId = Guid.NewGuid().ToString();
            profileUserResponse.Id = employerProfile.UserId;
            teamResponse.UserRef = employerProfile.UserId;
            accountsApiClient
                .Setup(x => x.GetAll<GetUserAccountsResponse>(
                    It.Is<GetUserAccountsRequest>(c => c.GetAllUrl.Contains($"user/{employerProfile.UserId}/accounts"))))
                .ReturnsAsync(apiResponse);
            accountsApiClient
                .Setup(x => x.GetAll<GetAccountTeamMembersResponse>(
                    It.Is<GetAccountTeamMembersRequest>(c => c.GetAllUrl.Contains($"accounts/{apiResponse.First().EncodedAccountId}/users"))))
                .ReturnsAsync(new List<GetAccountTeamMembersResponse>{teamResponse});
            employerProfilesApiClient.Setup(x => x.GetWithResponseCode<EmployerProfileUsersApiResponse>(
                It.Is<GetEmployerUserAccountRequest>(c =>
                    c.GetUrl.Contains($"api/users/{HttpUtility.UrlEncode(employerProfile.UserId)}"))))
                .ReturnsAsync(new ApiResponse<EmployerProfileUsersApiResponse>(profileUserResponse, HttpStatusCode.OK, ""));
            
            var actual = (await handler.GetEmployerAccounts(employerProfile)).ToList();

            actual.First().Role.Should().Be(teamResponse.Role);
            actual.First().DasAccountName.Should().Be(apiResponse.First().DasAccountName);
            actual.First().EncodedAccountId.Should().Be(apiResponse.First().EncodedAccountId);
            actual.TrueForAll(c => c.UserId.Equals(profileUserResponse.Id)).Should().BeTrue();
            actual.TrueForAll(c => c.FirstName.Equals(profileUserResponse.FirstName)).Should().BeTrue();
            actual.TrueForAll(c => c.LastName.Equals(profileUserResponse.LastName)).Should().BeTrue();
            actual.TrueForAll(c => c.DisplayName.Equals(profileUserResponse.DisplayName)).Should().BeTrue();
            actual.TrueForAll(c => c.IsSuspended.Equals(profileUserResponse.IsSuspended)).Should().BeTrue();
            actual.TrueForAll(c => c.UserId.Equals(employerProfile.UserId)).Should().BeTrue();
            employerProfilesApiClient.Verify(x => x.PutWithResponseCode<EmployerProfileUsersApiResponse>(
                It.IsAny<PutUpsertEmployerUserAccountRequest>()), Times.Never);
        }
    }
}