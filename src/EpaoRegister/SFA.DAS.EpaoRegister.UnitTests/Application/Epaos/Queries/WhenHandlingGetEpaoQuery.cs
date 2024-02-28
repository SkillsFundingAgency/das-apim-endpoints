using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpao;
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
    public class WhenHandlingGetEpaoQuery
    {
        [Test, MoqAutoData]
        public void And_Validation_Error_Then_Throws_ValidationException(
            GetEpaoQuery query,
            string propertyName,
            ValidationResult validationResult,
            [Frozen] Mock<IValidator<GetEpaoQuery>> mockValidator,
            GetEpaoQueryHandler handler)
        {
            validationResult.AddError(propertyName);
            mockValidator
                .Setup(validator => validator.ValidateAsync(It.IsAny<GetEpaoQuery>()))
                .ReturnsAsync(validationResult);

            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            act.Should().ThrowAsync<ValidationException>()
                .WithMessage($"*{propertyName}*");
        }

        [Test, MoqAutoData]
        public void And_No_Epao_Courses_Then_Throws_NotFoundException(
            GetEpaoQuery query,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetEpaoQueryHandler handler)
        {
            mockAssessorsApiClient
                .Setup(client => client.Get<GetEpaoResponse>(
                    It.Is<GetEpaoRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(default(GetEpaoResponse));

            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            act.Should().ThrowAsync<NotFoundException<GetEpaoResult>>();
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Epao_From_Assessor_Api(
            GetEpaoQuery query,
            GetEpaoResponse apiResponse,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            GetEpaoQueryHandler handler)
        {
            mockAssessorsApiClient
                .Setup(client => client.Get<GetEpaoResponse>(
                    It.Is<GetEpaoRequest>(request => request.EpaoId == query.EpaoId)))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Epao.Should().BeEquivalentTo(apiResponse);
        }
    }
}
