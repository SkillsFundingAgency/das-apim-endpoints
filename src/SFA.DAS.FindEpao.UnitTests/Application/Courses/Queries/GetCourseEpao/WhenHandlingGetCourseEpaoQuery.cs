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
using SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpao;
using SFA.DAS.FindEpao.InnerApi.Requests;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Validation;
using SFA.DAS.Testing.AutoFixture;
using ValidationResult = SFA.DAS.SharedOuterApi.Validation.ValidationResult;

namespace SFA.DAS.FindEpao.UnitTests.Application.Courses.Queries.GetCourseEpao
{
    public class WhenHandlingGetCourseEpaoQuery
    {
        [Test, MoqAutoData]
        public void And_Validation_Error_Then_Throws_ValidationException(
            GetCourseEpaoQuery query,
            string propertyName,
            ValidationResult validationResult,
            [Frozen] Mock<IValidator<GetCourseEpaoQuery>> mockValidator,
            GetCourseEpaoQueryHandler handler)
        {
            validationResult.AddError(propertyName);
            mockValidator
                .Setup(validator => validator.ValidateAsync(It.IsAny<GetCourseEpaoQuery>()))
                .ReturnsAsync(validationResult);

            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            act.Should().Throw<ValidationException>()
                .WithMessage($"*{propertyName}*");
        }

        [Test, MoqAutoData]
        public void And_No_Epao_Courses_Then_Throws_NotFoundException(
            GetCourseEpaoQuery query,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetCourseEpaoQueryHandler handler)
        {
            mockAssessorsApiClient
                .Setup(client => client.Get<GetEpaoResponse>(
                    It.Is<GetEpaoRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(default(GetEpaoResponse));

            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            act.Should().Throw<NotFoundException<GetCourseEpaoResult>>();
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Epao_From_Assessors_Api_And_Course_From_Courses_Api(
            GetCourseEpaoQuery query,
            GetEpaoResponse epaoApiResponse,
            List<GetCourseEpaoListItem> courseEpaosApiResponse,
            GetStandardsListItem coursesApiResponse,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            GetCourseEpaoQueryHandler handler)
        {
            courseEpaosApiResponse[0].EpaoId = query.EpaoId;
            mockAssessorsApiClient
                .Setup(client => client.Get<GetEpaoResponse>(
                    It.Is<GetEpaoRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(epaoApiResponse);
            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetCourseEpaoListItem>(
                    It.Is<GetCourseEpaosRequest>(request => request.CourseId == query.CourseId)))
                .ReturnsAsync(courseEpaosApiResponse);

            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(
                    It.Is<GetStandardRequest>(request => request.StandardId == query.CourseId)))
                .ReturnsAsync(coursesApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Epao.Should().BeEquivalentTo(epaoApiResponse);
            result.EpaoDeliveryAreas.Should().BeEquivalentTo(courseEpaosApiResponse.Single(item => item.EpaoId == query.EpaoId).DeliveryAreas);
            result.CourseEpaosCount.Should().Be(courseEpaosApiResponse.Count);
            result.Course.Should().BeEquivalentTo(coursesApiResponse);
        }
    }
}