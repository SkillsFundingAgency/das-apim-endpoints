﻿using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.SignIntoYourOldAccount;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.NServiceBus.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Candidate
{
    [TestFixture]
    public class WhenGettingSignIntoYourOldAccountQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Validity_Of_Credentials_Is_Returned(
            GetSignIntoYourOldAccountQuery query,
            PostLegacyValidateCredentialsApiResponse apiResponse,
            [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> candidateAccountApiClient,
            GetSignIntoYourOldAccountQueryHandler handler)
        {
            var expectedApiRequest = new PostLegacyValidateUserCredentialsApiRequest(new PostLegacyValidateUserCredentialsApiRequestBody
            {
                Email = query.Email,
                Password = query.Password
            });
            candidateAccountApiClient.Setup(x =>
                    x.PostWithResponseCode<PostLegacyValidateCredentialsApiResponse>(
                        It.Is<PostLegacyValidateUserCredentialsApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                            && ((PostLegacyValidateUserCredentialsApiRequestBody)r.Data).Email == query.Email
                            && ((PostLegacyValidateUserCredentialsApiRequestBody)r.Data).Password == query.Password), true))
                .ReturnsAsync(new ApiResponse<PostLegacyValidateCredentialsApiResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

            var result = await handler.Handle(query, CancellationToken.None);

            result.IsValid.Should().Be(apiResponse.IsValid);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_User_Fails_SignIn_Then_The_Failed_Attempt_Is_Recorded(
            GetSignIntoYourOldAccountQuery query,
            PostLegacyValidateCredentialsApiResponse apiResponse,
            DateTime now,
            [Frozen] Mock<IDateTimeService> dateTimeService,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> candidateAccountApiClient,
            GetSignIntoYourOldAccountQueryHandler handler)
        {
            apiResponse.IsValid = false;
            dateTimeService.Setup(x => x.UtcNow).Returns(now);

            var cacheItem = new GetSignIntoYourOldAccountQueryHandler.SignInAttemptHistory();

            var expectedCacheKey = $"{nameof(GetSignIntoYourOldAccountQueryHandler.SignInAttemptHistory)}-{query.CandidateId}";
            cacheStorageService.Setup(x =>
                    x.RetrieveFromCache<GetSignIntoYourOldAccountQueryHandler.SignInAttemptHistory>(It.Is<string>(key =>
                        key == expectedCacheKey)))
                .ReturnsAsync(cacheItem);

            var expectedApiRequest = new PostLegacyValidateUserCredentialsApiRequest(new PostLegacyValidateUserCredentialsApiRequestBody
            {
                Email = query.Email,
                Password = query.Password
            });
            candidateAccountApiClient.Setup(x =>
                    x.PostWithResponseCode<PostLegacyValidateCredentialsApiResponse>(
                        It.Is<PostLegacyValidateUserCredentialsApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                            && ((PostLegacyValidateUserCredentialsApiRequestBody)r.Data).Email == query.Email
                            && ((PostLegacyValidateUserCredentialsApiRequestBody)r.Data).Password == query.Password), true))
                .ReturnsAsync(new ApiResponse<PostLegacyValidateCredentialsApiResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

            var result = await handler.Handle(query, CancellationToken.None);

            cacheStorageService.Verify(x => x.SaveToCache(expectedCacheKey,
                It.Is<GetSignIntoYourOldAccountQueryHandler.SignInAttemptHistory>(h =>
                    h.SignInAttempts.Count == 1 && h.SignInAttempts.First() == now),
                It.Is<int>(e => e == 1)));

            result.IsValid.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_User_Has_Exceeded_SignIn_Attempts_Then_Sign_In_Fails(
            GetSignIntoYourOldAccountQuery query,
            PostLegacyValidateCredentialsApiResponse apiResponse,
            DateTime now,
            [Frozen] Mock<IDateTimeService> dateTimeService,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> candidateAccountApiClient,
            GetSignIntoYourOldAccountQueryHandler handler)
        {
            dateTimeService.Setup(x => x.UtcNow).Returns(now);

            var cacheItem = new GetSignIntoYourOldAccountQueryHandler.SignInAttemptHistory
            {
                SignInAttempts =
                [
                    now.AddMinutes(-5),
                    now.AddMinutes(-10),
                    now.AddMinutes(-15),
                    now.AddMinutes(-20),
                    now.AddMinutes(-25)
                ]
            };

            var expectedCacheKey = $"{nameof(GetSignIntoYourOldAccountQueryHandler.SignInAttemptHistory)}-{query.CandidateId}";
            cacheStorageService.Setup(x =>
                    x.RetrieveFromCache<GetSignIntoYourOldAccountQueryHandler.SignInAttemptHistory>(It.Is<string>(key =>
                        key == expectedCacheKey)))
                .ReturnsAsync(cacheItem);

            var result = await handler.Handle(query, CancellationToken.None);

            candidateAccountApiClient.Verify(x =>
                x.PostWithResponseCode<PostLegacyValidateCredentialsApiResponse>(
                    It.IsAny<PostLegacyValidateUserCredentialsApiRequest>(), true), Times.Never);

            result.IsValid.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_User_Successfully_Signs_In_Then_Previous_Attempts_Are_Cleared(
            GetSignIntoYourOldAccountQuery query,
            PostLegacyValidateCredentialsApiResponse apiResponse,
            DateTime now,
            [Frozen] Mock<IDateTimeService> dateTimeService,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> candidateAccountApiClient,
            GetSignIntoYourOldAccountQueryHandler handler)
        {
            dateTimeService.Setup(x => x.UtcNow).Returns(now);

            var cacheItem = new GetSignIntoYourOldAccountQueryHandler.SignInAttemptHistory
            {
                SignInAttempts =
                [
                    now.AddMinutes(-5),
                    now.AddMinutes(-10),
                    now.AddMinutes(-15)
                ]
            };

            var expectedCacheKey = $"{nameof(GetSignIntoYourOldAccountQueryHandler.SignInAttemptHistory)}-{query.CandidateId}";
            cacheStorageService.Setup(x =>
                    x.RetrieveFromCache<GetSignIntoYourOldAccountQueryHandler.SignInAttemptHistory>(It.Is<string>(key =>
                        key == expectedCacheKey)))
                .ReturnsAsync(cacheItem);

            var expectedApiRequest = new PostLegacyValidateUserCredentialsApiRequest(new PostLegacyValidateUserCredentialsApiRequestBody
            {
                Email = query.Email,
                Password = query.Password
            });
            candidateAccountApiClient.Setup(x =>
                    x.PostWithResponseCode<PostLegacyValidateCredentialsApiResponse>(
                        It.Is<PostLegacyValidateUserCredentialsApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                            && ((PostLegacyValidateUserCredentialsApiRequestBody)r.Data).Email == query.Email
                            && ((PostLegacyValidateUserCredentialsApiRequestBody)r.Data).Password == query.Password), true))
                .ReturnsAsync(new ApiResponse<PostLegacyValidateCredentialsApiResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

            var result = await handler.Handle(query, CancellationToken.None);

            result.IsValid.Should().BeTrue();
            cacheStorageService.Verify(x => x.DeleteFromCache(expectedCacheKey), Times.Once);
        }


        [Test, MoqAutoData]
        public async Task Then_If_The_User_Exceeds_SignIn_Attempts_Then_They_May_Retry_After_Half_An_Hour(
            GetSignIntoYourOldAccountQuery query,
            PostLegacyValidateCredentialsApiResponse apiResponse,
            DateTime now,
            [Frozen] Mock<IDateTimeService> dateTimeService,
            [Frozen] Mock<ICacheStorageService> cacheStorageService,
            [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> candidateAccountApiClient,
            GetSignIntoYourOldAccountQueryHandler handler)
        {
            dateTimeService.Setup(x => x.UtcNow).Returns(now);

            var cacheItem = new GetSignIntoYourOldAccountQueryHandler.SignInAttemptHistory
            {
                SignInAttempts =
                [
                    now.AddMinutes(-5),
                    now.AddMinutes(-10),
                    now.AddMinutes(-15),
                    now.AddMinutes(-20),
                    now.AddMinutes(-45)
                ]
            };

            var expectedCacheKey = $"{nameof(GetSignIntoYourOldAccountQueryHandler.SignInAttemptHistory)}-{query.CandidateId}";
            cacheStorageService.Setup(x =>
                    x.RetrieveFromCache<GetSignIntoYourOldAccountQueryHandler.SignInAttemptHistory>(It.Is<string>(key =>
                        key == expectedCacheKey)))
                .ReturnsAsync(cacheItem);

            var expectedApiRequest = new PostLegacyValidateUserCredentialsApiRequest(new PostLegacyValidateUserCredentialsApiRequestBody
            {
                Email = query.Email,
                Password = query.Password
            });
            candidateAccountApiClient.Setup(x =>
                    x.PostWithResponseCode<PostLegacyValidateCredentialsApiResponse>(
                        It.Is<PostLegacyValidateUserCredentialsApiRequest>(r => r.PostUrl == expectedApiRequest.PostUrl
                            && ((PostLegacyValidateUserCredentialsApiRequestBody)r.Data).Email == query.Email
                            && ((PostLegacyValidateUserCredentialsApiRequestBody)r.Data).Password == query.Password), true))
                            .ReturnsAsync(new ApiResponse<PostLegacyValidateCredentialsApiResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

            var result = await handler.Handle(query, CancellationToken.None);

            result.IsValid.Should().BeTrue();
            cacheStorageService.Verify(x => x.DeleteFromCache(expectedCacheKey), Times.Once);
        }
    }
}
