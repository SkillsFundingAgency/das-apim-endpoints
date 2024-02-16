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
using SFA.DAS.EpaoRegister.InnerApi.Requests;
using SFA.DAS.EpaoRegister.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
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
        public async Task Then_Gets_EpaoCourses_From_Assessor_Api(
            GetEpaoCoursesQuery query,
            List<GetEpaoCoursesListItem> apiResponse,
            GetStandardsListResponse getStandardResponses,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            GetEpaoCoursesQueryHandler handler)
        {
            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetEpaoCoursesListItem>(
                    It.Is<GetEpaoCoursesRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetActiveStandardsListRequest>()))
                .ReturnsAsync(getStandardResponses);

            var result = await handler.Handle(query, CancellationToken.None);

            result.EpaoId.Should().Be(query.EpaoId);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Courses_From_Api(
            GetEpaoCoursesQuery query,
            List<GetEpaoCoursesListItem> apiResponse,
            List<GetStandardResponse> matchingStandards,
            GetStandardResponse nonMatchedStandard,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            [Frozen] Mock<ICacheStorageService> mockCacheStorageService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            GetEpaoCoursesQueryHandler handler)
        {
            var i = 0;
            foreach (var course in matchingStandards)
            {
                course.LarsCode = apiResponse[i].StandardCode;
                i++;
            }
            var allStandards = new List<GetStandardResponse>(); 
            allStandards.AddRange(matchingStandards);
            allStandards.Add(nonMatchedStandard);
            
            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetEpaoCoursesListItem>(
                    It.Is<GetEpaoCoursesRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(apiResponse);
            
            foreach (var standard in allStandards)
            {
                mockCoursesApiClient
                    .Setup(client => client.Get<GetStandardResponse>(It.Is<GetStandardRequest>(c=>c.StandardId.Equals(standard.LarsCode))))
                    .ReturnsAsync(standard);    
            }
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.EpaoId.Should().Be(query.EpaoId);
            result.Courses.Should().BeEquivalentTo(matchingStandards);
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
            var i = 0;
            foreach (var course in matchingStandards)
            {
                course.LarsCode = apiResponse[i].StandardCode;
                i++;
            }
            apiResponse.Add(additionalItem);
            var allStandards = new List<GetStandardResponse>(); 
            allStandards.AddRange(matchingStandards);
            allStandards.Add(nonMatchedStandard);
            
            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetEpaoCoursesListItem>(
                    It.Is<GetEpaoCoursesRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardResponse>(
                    It.IsAny<GetStandardRequest>()))
                .ReturnsAsync((GetStandardResponse)null);   
            
            foreach (var standard in matchingStandards)
            {
                mockCoursesApiClient
                    .Setup(client => client.Get<GetStandardResponse>(
                        It.Is<GetStandardRequest>(c=>c.StandardId.Equals(standard.LarsCode))))
                    .ReturnsAsync(standard);    
            }
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.EpaoId.Should().Be(query.EpaoId);
            result.Courses.Should().BeEquivalentTo(matchingStandards);
            result.Courses.SingleOrDefault(c => c.LarsCode.Equals(additionalItem.StandardCode)).Should().BeNull();
            
        }
    }
}
