﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.MigrateData;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.LegacyApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply
{
    public class WhenHandlingMigrateDataCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_CommandResult_Is_Returned_As_Expected(
            MigrateDataCommand command,
            PutCandidateApiResponse putCandidateApiResponse,
            PostCandidateAddressApiResponse postCandidateAddressApiResponse,
            GetLegacyUserByEmailApiResponse legacyUserByEmailApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockCandidateApiClient,
            [Frozen] Mock<IFindApprenticeshipLegacyApiClient<FindApprenticeshipLegacyApiConfiguration>> mockLegacyApiClient,
            [Frozen] Mock<ILegacyApplicationMigrationService> vacancyMigrationService,
            MigrateDataCommandHandler handler)
        {
            var legacyGetRequest = new GetLegacyUserByEmailApiRequest(command.EmailAddress);
            mockLegacyApiClient
                .Setup(client => client.Get<GetLegacyUserByEmailApiResponse>(
                    It.Is<GetLegacyUserByEmailApiRequest>(r => r.GetUrl == legacyGetRequest.GetUrl)))
                .ReturnsAsync(legacyUserByEmailApiResponse);

            var expectedRequest = new PutCandidateApiRequest(command.CandidateId, new PutCandidateApiRequestData());

            mockCandidateApiClient
                .Setup(client => client.PutWithResponseCode<PutCandidateApiResponse>(
                    It.Is<PutCandidateApiRequest>(c =>
                        c.PutUrl == expectedRequest.PutUrl
                        && ((PutCandidateApiRequestData)c.Data).MigratedEmail == legacyUserByEmailApiResponse.RegistrationDetails!.EmailAddress
                        && ((PutCandidateApiRequestData)c.Data).MigratedCandidateId == legacyUserByEmailApiResponse.Id
                        && ((PutCandidateApiRequestData)c.Data).FirstName == legacyUserByEmailApiResponse.RegistrationDetails!.FirstName
                        && ((PutCandidateApiRequestData)c.Data).LastName == legacyUserByEmailApiResponse.RegistrationDetails!.LastName
                        && ((PutCandidateApiRequestData)c.Data).DateOfBirth == legacyUserByEmailApiResponse.RegistrationDetails!.DateOfBirth
                        && ((PutCandidateApiRequestData)c.Data).PhoneNumber == legacyUserByEmailApiResponse.RegistrationDetails!.PhoneNumber
                        )))
                .ReturnsAsync(new ApiResponse<PutCandidateApiResponse>(putCandidateApiResponse, HttpStatusCode.OK, string.Empty));

            var expectedPostRequest = new PutCandidateAddressApiRequest(command.CandidateId, new PutCandidateAddressApiRequestData());

            mockCandidateApiClient
                .Setup(client => client.PutWithResponseCode<PostCandidateAddressApiResponse>(
                    It.Is<PutCandidateAddressApiRequest>(c =>
                        c.PutUrl == expectedPostRequest.PutUrl
                        && ((PutCandidateAddressApiRequestData)c.Data).AddressLine1 == legacyUserByEmailApiResponse.RegistrationDetails!.Address.AddressLine1
                        && ((PutCandidateAddressApiRequestData)c.Data).AddressLine2 == legacyUserByEmailApiResponse.RegistrationDetails!.Address.AddressLine2
                        && ((PutCandidateAddressApiRequestData)c.Data).AddressLine3 == legacyUserByEmailApiResponse.RegistrationDetails!.Address.AddressLine3
                        && ((PutCandidateAddressApiRequestData)c.Data).AddressLine4 == legacyUserByEmailApiResponse.RegistrationDetails!.Address.AddressLine4
                        && ((PutCandidateAddressApiRequestData)c.Data).Postcode == legacyUserByEmailApiResponse.RegistrationDetails!.Address.Postcode
                        && Math.Abs(((PutCandidateAddressApiRequestData)c.Data).Longitude - legacyUserByEmailApiResponse.RegistrationDetails!.Address.GeoPoint.Longitude) < 0.001
                        && Math.Abs(((PutCandidateAddressApiRequestData)c.Data).Latitude - legacyUserByEmailApiResponse.RegistrationDetails!.Address.GeoPoint.Latitude) < 0.001
                        && ((PutCandidateAddressApiRequestData)c.Data).Uprn == legacyUserByEmailApiResponse.RegistrationDetails!.Address.Uprn
                        && ((PutCandidateAddressApiRequestData)c.Data).Email == putCandidateApiResponse.Email
                        )
                    )
                )
                .ReturnsAsync(new ApiResponse<PostCandidateAddressApiResponse>(postCandidateAddressApiResponse, HttpStatusCode.OK, string.Empty));

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().Be(Unit.Value);
            vacancyMigrationService
                .Verify(client => client.MigrateLegacyApplications(command.CandidateId, command.EmailAddress), Times.Once);
        }
    }
}
