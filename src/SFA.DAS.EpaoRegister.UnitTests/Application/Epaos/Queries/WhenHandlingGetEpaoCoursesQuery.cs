using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

            act.Should().Throw<ValidationException>()
                .WithMessage($"*{propertyName}*");
        }

        [Test, MoqAutoData]
        public void And_No_Epao_Courses_Then_Throws_EntityNotFoundException(
            GetEpaoCoursesQuery query,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetEpaoCoursesQueryHandler handler)
        {
            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetEpaoCoursesListItem>(
                    It.Is<GetEpaoCoursesRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(new List<GetEpaoCoursesListItem>());

            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            act.Should().Throw<EntityNotFoundException<GetStandardResponse>>();
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_EpaoCourses_From_Assessor_Api_And_Courses_From_Courses_Api(
            GetEpaoCoursesQuery query,
            List<GetEpaoCoursesListItem> apiResponse,
            List<GetStandardResponse> getStandardResponses,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            GetEpaoCoursesQueryHandler handler)
        {
            mockAssessorsApiClient
                .Setup(client => client.GetAll<GetEpaoCoursesListItem>(
                    It.Is<GetEpaoCoursesRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .SetupSequence(client => client.Get<GetStandardResponse>(It.IsAny<GetStandardRequest>()))
                .ReturnsAsync(getStandardResponses[0])
                .ReturnsAsync(getStandardResponses[1])
                .ReturnsAsync(getStandardResponses[2]);

            var result = await handler.Handle(query, CancellationToken.None);

            result.EpaoId.Should().Be(query.EpaoId);
            result.Courses.Should().BeEquivalentTo(getStandardResponses);
        }
    }
}