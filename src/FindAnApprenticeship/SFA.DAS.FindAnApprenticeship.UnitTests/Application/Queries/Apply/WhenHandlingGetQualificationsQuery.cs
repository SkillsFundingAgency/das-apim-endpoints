﻿using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.Qualifications;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply
{
    [TestFixture]
    public class WhenHandlingGetQualificationsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(
            GetQualificationsQuery query,
            GetApplicationApiResponse applicationApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetQualificationsQueryHandler handler)
        {
            applicationApiResponse.QualificationsStatus = Constants.SectionStatus.Incomplete;

            var expectedApplicationRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId);
            applicationApiResponse.JobsStatus = Constants.SectionStatus.Incomplete;
            candidateApiClient.Setup(client =>
                client.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedApplicationRequest.GetUrl)))
                .ReturnsAsync(applicationApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();

            result.Should().BeEquivalentTo(new GetQualificationsQueryResult
            {
                IsSectionCompleted = false,
                Qualifications = Enumerable.Empty<GetQualificationsQueryResult.Qualification>().ToList()
            });
        }
    }
}
