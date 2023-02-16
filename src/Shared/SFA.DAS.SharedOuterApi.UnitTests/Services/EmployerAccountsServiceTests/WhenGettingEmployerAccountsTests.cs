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
            EmployerUsersApiResponse userResponse,
            [Frozen] Mock<IEmployerProfilesApiClient<EmployerProfilesApiConfiguration>> employerProfilesApiClient,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
            EmployerAccountsService handler)
        {
            teamResponse.UserRef = userResponse.Id;
            accountsApiClient
                .Setup(x => x.GetAll<GetUserAccountsResponse>(
                    It.Is<GetUserAccountsRequest>(c => c.GetAllUrl.Contains($"user/{userResponse.Id}/accounts"))))
                .ReturnsAsync(apiResponse);
            accountsApiClient
                .Setup(x => x.GetAll<GetAccountTeamMembersResponse>(
                    It.Is<GetAccountTeamMembersRequest>(c => c.GetAllUrl.Contains($"accounts/{apiResponse.First().EncodedAccountId}/users"))))
                .ReturnsAsync(new List<GetAccountTeamMembersResponse>{teamResponse});
            employerProfilesApiClient.Setup(x => x.GetWithResponseCode<EmployerUsersApiResponse>(
                    It.Is<GetEmployerUserAccountRequest>(c =>
                        c.GetUrl.Contains($"api/users/{HttpUtility.UrlEncode(employerProfile.UserId)}"))))
                .ReturnsAsync(new ApiResponse<EmployerUsersApiResponse>(userResponse, HttpStatusCode.OK, ""));

            var actual = (await handler.GetEmployerAccounts(employerProfile)).ToList();

            actual.First().Role.Should().Be(teamResponse.Role);
            actual.First().DasAccountName.Should().Be(apiResponse.First().DasAccountName);
            actual.First().EncodedAccountId.Should().Be(apiResponse.First().EncodedAccountId);
            actual.TrueForAll(c => c.UserId.Equals(userResponse.Id)).Should().BeTrue();
            actual.TrueForAll(c => c.FirstName.Equals(userResponse.FirstName)).Should().BeTrue();
            actual.TrueForAll(c => c.LastName.Equals(userResponse.LastName)).Should().BeTrue();
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_The_Id_Is_Not_A_Guid_Then_User_Account_IsNot_Found_Then_Upserted(
            EmployerProfile employerProfile,
            List<GetUserAccountsResponse> apiResponse,
            GetAccountTeamMembersResponse teamResponse,
            EmployerUsersApiResponse userResponse,
            [Frozen] Mock<IEmployerProfilesApiClient<EmployerUsersApiConfiguration>> employerProfilesApiClient,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
            EmployerAccountsService handler)
        {
            teamResponse.UserRef = userResponse.Id;
            accountsApiClient
                .Setup(x => x.GetAll<GetUserAccountsResponse>(
                    It.Is<GetUserAccountsRequest>(c => c.GetAllUrl.Contains($"user/{userResponse.Id}/accounts"))))
                .ReturnsAsync(apiResponse);
            accountsApiClient
                .Setup(x => x.GetAll<GetAccountTeamMembersResponse>(
                    It.Is<GetAccountTeamMembersRequest>(c => c.GetAllUrl.Contains($"accounts/{apiResponse.First().EncodedAccountId}/users"))))
                .ReturnsAsync(new List<GetAccountTeamMembersResponse>{teamResponse});
            employerProfilesApiClient.Setup(x => x.GetWithResponseCode<EmployerUsersApiResponse>(
                    It.Is<GetEmployerUserAccountRequest>(c =>
                        c.GetUrl.Contains($"api/users/{HttpUtility.UrlEncode(employerProfile.UserId)}"))))
                .ReturnsAsync(new ApiResponse<EmployerUsersApiResponse>(null, HttpStatusCode.NotFound, "Not Found"));
            employerProfilesApiClient.Setup(x => x.PutWithResponseCode<EmployerUsersApiResponse>(
                    It.Is<PutUpsertEmployerUserAccountRequest>(c =>
                        c.PutUrl.Contains($"api/users/"))))
                .ReturnsAsync(new ApiResponse<EmployerUsersApiResponse>(userResponse, HttpStatusCode.Created, ""));

            var actual = (await handler.GetEmployerAccounts(employerProfile)).ToList();

            actual.First().Role.Should().Be(teamResponse.Role);
            actual.First().DasAccountName.Should().Be(apiResponse.First().DasAccountName);
            actual.First().EncodedAccountId.Should().Be(apiResponse.First().EncodedAccountId);
            actual.TrueForAll(c => c.UserId.Equals(userResponse.Id)).Should().BeTrue();
            actual.TrueForAll(c => c.FirstName.Equals(userResponse.FirstName)).Should().BeTrue();
            actual.TrueForAll(c => c.LastName.Equals(userResponse.LastName)).Should().BeTrue();
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_The_Id_Is_A_Guid_Then_User_Account_Found_And_Not_Upserted(
            EmployerProfile employerProfile,
            List<GetUserAccountsResponse> apiResponse,
            GetAccountTeamMembersResponse teamResponse,
            [Frozen] Mock<IEmployerUsersApiClient<EmployerUsersApiConfiguration>> employerUsersApiClient,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
            EmployerAccountsService handler)
        {
            employerProfile.UserId = Guid.NewGuid().ToString();
            teamResponse.UserRef = employerProfile.UserId;
            accountsApiClient
                .Setup(x => x.GetAll<GetUserAccountsResponse>(
                    It.Is<GetUserAccountsRequest>(c => c.GetAllUrl.Contains($"user/{employerProfile.UserId}/accounts"))))
                .ReturnsAsync(apiResponse);
            accountsApiClient
                .Setup(x => x.GetAll<GetAccountTeamMembersResponse>(
                    It.Is<GetAccountTeamMembersRequest>(c => c.GetAllUrl.Contains($"accounts/{apiResponse.First().EncodedAccountId}/users"))))
                .ReturnsAsync(new List<GetAccountTeamMembersResponse>{teamResponse});
            
            var actual = (await handler.GetEmployerAccounts(employerProfile)).ToList();

            actual.First().Role.Should().Be(teamResponse.Role);
            actual.First().DasAccountName.Should().Be(apiResponse.First().DasAccountName);
            actual.First().EncodedAccountId.Should().Be(apiResponse.First().EncodedAccountId);
            actual.First().FirstName.Should().BeEmpty();
            actual.First().LastName.Should().BeEmpty();
            actual.TrueForAll(c => c.UserId.Equals(employerProfile.UserId)).Should().BeTrue();
            employerUsersApiClient.Verify(x => x.GetWithResponseCode<EmployerUsersApiResponse>(
                It.IsAny<GetEmployerUserAccountRequest>( )), Times.Never);
            employerUsersApiClient.Verify(x => x.PutWithResponseCode<EmployerUsersApiResponse>(
                It.IsAny<PutUpsertEmployerUserAccountRequest>()), Times.Never);
        }
    }
}