using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WhatInterestsYou;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WorkHistory;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Apply
{
    [TestFixture]
    public class WhenHandlingGetWhatInterestsYouQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_QueryResult_Is_Returned_As_Expected(GetWhatInterestsYouQuery query,
            GetApplicationApiResponse applicationApiResponse,
            GetApprenticeshipVacancyItemResponse vacancyApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> faaApiClient,
            GetWhatInterestsYouQueryHandler handler)
        {
            var expectedGetApplicationApiRequest = new GetApplicationApiRequest(query.CandidateId, query.ApplicationId);

            candidateApiClient.Setup(x => x.Get<GetApplicationApiResponse>(It.Is<GetApplicationApiRequest>(r => r.GetUrl == expectedGetApplicationApiRequest.GetUrl)))
                .ReturnsAsync(applicationApiResponse);
            
            var expectedVacancyApiRequest = new GetVacancyRequest(applicationApiResponse.VacancyReference);

            faaApiClient
                .Setup(client => client.Get<GetApprenticeshipVacancyItemResponse>(
                    It.Is<GetVacancyRequest>(r => r.GetUrl == expectedVacancyApiRequest.GetUrl)))
                .ReturnsAsync(vacancyApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            using var scope = new AssertionScope();

            result.Should().BeEquivalentTo(new { vacancyApiResponse.EmployerName, StandardName = vacancyApiResponse.CourseTitle });
        }

    }
}
