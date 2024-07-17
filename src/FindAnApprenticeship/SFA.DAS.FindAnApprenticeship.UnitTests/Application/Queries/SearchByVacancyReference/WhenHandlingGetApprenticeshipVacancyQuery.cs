﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Azure.Amqp.Framing;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.SearchByVacancyReference
{
    public class WhenHandlingGetApprenticeshipVacancyQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Vacancy_And_Associated_Course_Data_Is_Returned(
            GetApprenticeshipVacancyQuery query,
            GetApprenticeshipVacancyItemResponse vacancy,
            GetStandardsListItemResponse courseResponse,
            GetCourseLevelsListResponse courseLevelsResponse,
            [Frozen] Mock<IVacancyService> vacancyService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> courseApiClient,
            [Frozen] Mock<ICourseService> courseService,
            GetApprenticeshipVacancyQueryHandler handler)
        {
            // Arrange
            query.CandidateId = null;

            courseApiClient
                .Setup(x => x.Get<GetStandardsListItemResponse>(
                    It.Is<GetStandardRequest>(c => c.StandardId.Equals(vacancy.CourseId))))
                .ReturnsAsync(courseResponse);

            vacancyService
                .Setup(x => x.GetVacancy(query.VacancyReference))
                .ReturnsAsync(vacancy);

            courseService.Setup(x => x.GetLevels()).ReturnsAsync(courseLevelsResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ApprenticeshipVacancy.Should().BeEquivalentTo(vacancy, options => 
                options
                    .Excluding(x => x.Application)
                    .Excluding(x=>x.ClosingDate)
                    .Excluding(x=>x.ClosedDate)
                    .Excluding(x => x.ExternalVacancyUrl)
                    .Excluding(x => x.IsExternalVacancy)
                    .Excluding(x => x.City)
                    .Excluding(x => x.Postcode)
                    .Excluding(x => x.ApplicationUrl)
                );
            result.CourseDetail.Should().BeEquivalentTo(courseResponse);
            result.Levels.Should().BeEquivalentTo(courseLevelsResponse.Levels);
            result.Application.Should().BeNull();
            result.ApprenticeshipVacancy.ClosingDate.Should().Be(vacancy.ClosedDate ?? vacancy.ClosingDate);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Vacancy_And_Candidate_Data_Is_Returned(
            GetApprenticeshipVacancyQuery query,
            GetApprenticeshipVacancyItemResponse vacancy,
            GetStandardsListItemResponse courseResponse,
            GetCourseLevelsListResponse courseLevelsResponse,
            GetApplicationByReferenceApiResponse applicationResponse,
            GetCandidateAddressApiResponse candidateAddressApiResponse,
            [Frozen] Mock<IVacancyService> vacancyService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> courseApiClient,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            GetApprenticeshipVacancyQueryHandler handler)
        {
            // Arrange
            courseApiClient
                .Setup(x => x.Get<GetStandardsListItemResponse>(
                    It.Is<GetStandardRequest>(c => c.StandardId.Equals(vacancy.CourseId))))
                .ReturnsAsync(courseResponse);

            vacancyService
                .Setup(x => x.GetVacancy(query.VacancyReference))
                .ReturnsAsync(vacancy);

            courseService.Setup(x => x.GetLevels()).ReturnsAsync(courseLevelsResponse);

            var expectedGetCandidateApplicationRequest =
                new GetApplicationByReferenceApiRequest(query.CandidateId.Value, query.VacancyReference.Replace("VAC","", StringComparison.CurrentCultureIgnoreCase));
            candidateApiClient
                .Setup(client =>
                    client.Get<GetApplicationByReferenceApiResponse>(
                        It.Is<GetApplicationByReferenceApiRequest>(c=>c.GetUrl == expectedGetCandidateApplicationRequest.GetUrl)))
                .ReturnsAsync(applicationResponse);
            
            var expectedGetCandidateAddressRequest =
                new GetCandidateAddressApiRequest(query.CandidateId.Value);
            candidateApiClient
                .Setup(client =>
                    client.Get<GetCandidateAddressApiResponse>(
                        It.Is<GetCandidateAddressApiRequest>(c=>c.GetUrl == expectedGetCandidateAddressRequest.GetUrl)))
                .ReturnsAsync(candidateAddressApiResponse);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.ApprenticeshipVacancy.Should().BeEquivalentTo(vacancy, options => options
                    .Excluding(x => x.Application)
                    .Excluding(x => x.ClosingDate)
                    .Excluding(x => x.ClosedDate)
                    .Excluding(x => x.ExternalVacancyUrl)
                    .Excluding(x => x.IsExternalVacancy)
                    .Excluding(x => x.City)
                    .Excluding(x => x.Postcode)
                    .Excluding(x => x.ApplicationUrl)
                );

            result.CourseDetail.Should().BeEquivalentTo(courseResponse);
            result.Levels.Should().BeEquivalentTo(courseLevelsResponse.Levels);
            result.Application.Should().NotBeNull();
            result.Application.ApplicationId.Should().Be(applicationResponse.Id);
            result.Application.Status.Should().Be(applicationResponse.Status);
            result.Application.SubmittedDate.Should().Be(applicationResponse.SubmittedDate);
            result.ApprenticeshipVacancy.ClosingDate.Should().Be(vacancy.ClosedDate ?? vacancy.ClosingDate);
            result.CandidatePostcode.Should().Be(candidateAddressApiResponse.Postcode);
        }
    }
}