using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaoCourses;
using SFA.DAS.EpaoRegister.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Validation;
using SFA.DAS.Testing.AutoFixture;
using ValidationResult = SFA.DAS.SharedOuterApi.Validation.ValidationResult;

namespace SFA.DAS.EpaoRegister.UnitTests.Application.Epaos.Queries
{
    public class WhenHandlingGetEpaoCoursesQuery
    {
        [Test, MoqAutoData]
        public void And_Validation_Error_Then_Throws_ValidationException(
            GetEpaoCoursesQuery query,
            string propertyName,
            ValidationResult validationResult,
            [Frozen] Mock<IValidator<GetEpaoCoursesQuery>> mockValidator,
            GetEpaoCoursesQueryHandler handler)
        {
            validationResult.AddError(propertyName);
            mockValidator
                .Setup(validator => validator.ValidateAsync(It.IsAny<GetEpaoCoursesQuery>()))
                .ReturnsAsync(validationResult);

            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"*{propertyName}*");
        }

        [Test, MoqAutoData]
        public async Task And_No_Epao_Courses_Then_Returns_Empty_List(
            GetEpaoCoursesQuery query,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetEpaoCoursesQueryHandler handler)
        {
            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetEpaoCoursesListItem>(
                    It.Is<GetEpaoCoursesRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(new List<GetEpaoCoursesListItem>());

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Courses.Should().BeEmpty();
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_Only_Matching_Active_Courses_For_Epao(
            GetEpaoCoursesQuery query,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            GetEpaoCoursesQueryHandler handler)
        {
            // Arrange
            var epaoStandardCodes = new List<int> { 101, 102, 103 };

            var epaoCourses = epaoStandardCodes
                .Select(code => new GetEpaoCoursesListItem { StandardCode = code })
                .ToList();

            var activeStandards = new List<GetStandardResponse>
            {
                new() { LarsCode = 101, }, // match
                new() { LarsCode = 102, }, // match
                new() { LarsCode = 999, }, // no match
                new() { LarsCode = 103, }  // match
            };

            var getStandardResponses = new GetStandardsListResponse
            {
                Standards = activeStandards
            };

            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetEpaoCoursesListItem>(
                    It.Is<GetEpaoCoursesRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(epaoCourses);

            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.IsAny<GetActiveStandardsListRequest>()))
                .ReturnsAsync(getStandardResponses);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.EpaoId.Should().Be(query.EpaoId);
            result.Courses.Should().HaveCount(3);
            result.Courses.Select(c => c.LarsCode).Should().BeEquivalentTo([101, 102, 103]);
        }
        
        [Test, MoqAutoData]
        public async Task Then_Gets_Courses_From_Api_And_If_Course_Doesnt_Exist_Its_Not_Added(
            GetEpaoCoursesQuery query,
            List<GetEpaoCoursesListItem> apiResponse,
            GetEpaoCoursesListItem additionalItem,
            List<GetStandardResponse> matchingStandards,
            GetStandardResponse nonMatchedStandard,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            [Frozen] Mock<ICacheStorageService> mockCacheStorageService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            GetEpaoCoursesQueryHandler handler)
        {
            // Arrange
            var epaoStandardCodes = new List<int> { 101, 102, 103 };

            var epaoCourses = epaoStandardCodes
                .Select(code => new GetEpaoCoursesListItem { StandardCode = code })
                .ToList();

            var activeStandards = new List<GetStandardResponse>
            {
                new() { LarsCode = 101, }, // match
                new() { LarsCode = 999, }, // no match
                new() { LarsCode = 103, }  // match
            };

            var getStandardResponses = new GetStandardsListResponse
            {
                Standards = activeStandards
            };

            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetEpaoCoursesListItem>(
                    It.Is<GetEpaoCoursesRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(epaoCourses);

            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(
                    It.IsAny<GetActiveStandardsListRequest>()))
                .ReturnsAsync(getStandardResponses);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.EpaoId.Should().Be(query.EpaoId);
            result.Courses.Should().HaveCount(2);
            result.Courses.Select(c => c.LarsCode).Should().BeEquivalentTo([101, 103]);
        }
    }
}
