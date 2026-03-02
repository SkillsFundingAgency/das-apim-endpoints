using System.Text.Json;
using System.Threading;
using AutoFixture;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Domain.Vacancy;
using SFA.DAS.Recruit.GraphQL;
using SFA.DAS.Recruit.GraphQL.RecruitInner.Mappers;
using SFA.DAS.SharedOuterApi.Models;
using StrawberryShake;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Vacancies;

public class WhenGettingVacancyByReference
{
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Not_Found(
        long vacancyReference,
        Mock<IOperationResult<IGetVacancyByReferenceResult>> queryResult,
        Mock<IRecruitGqlClient> recruitGqlClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        queryResult.Setup(x => x.Data).Returns((IGetVacancyByReferenceResult)null!);
        recruitGqlClient
            .Setup(x => x.GetVacancyByReference.ExecuteAsync(vacancyReference, CancellationToken.None))
            .ReturnsAsync(queryResult.Object);

        // act
        var result = await sut.GetOneByReference(vacancyReference, recruitGqlClient.Object, CancellationToken.None) as NotFound;

        // assert
        result.Should().NotBeNull();
    }
    
    [Test, MoqAutoData]
    public async Task Then_The_Vacancy_Is_Returned(
        IFixture fixture,
        Mock<IGetVacancyByReference_Vacancies> vacancy,
        Mock<IOperationResult<IGetVacancyByReferenceResult>> queryResult,
        Mock<IRecruitGqlClient> recruitGqlClient,
        [Greedy] VacanciesController sut)
    {
        // arrange
        vacancy.Setup(x => x.EmployerLocations).Returns(JsonSerializer.Serialize(fixture.CreateMany<Address>(), Global.JsonSerializerOptions));
        vacancy.Setup(x => x.EmployerReviewFieldIndicators).Returns(JsonSerializer.Serialize(fixture.CreateMany<ReviewFieldIndicator>(), Global.JsonSerializerOptions));
        vacancy.Setup(x => x.ProviderReviewFieldIndicators).Returns(JsonSerializer.Serialize(fixture.CreateMany<ReviewFieldIndicator>(), Global.JsonSerializerOptions));
        vacancy.Setup(x => x.Qualifications).Returns(JsonSerializer.Serialize(fixture.CreateMany<Qualification>(), Global.JsonSerializerOptions));
        vacancy.Setup(x => x.Skills).Returns(JsonSerializer.Serialize(fixture.CreateMany<string>(), Global.JsonSerializerOptions));
        vacancy.Setup(x => x.TrainingProvider_Address).Returns(JsonSerializer.Serialize(fixture.Create<Address>(), Global.JsonSerializerOptions));
        vacancy.Setup(x => x.TransferInfo).Returns(JsonSerializer.Serialize(fixture.Create<TransferInfo>(), Global.JsonSerializerOptions));
        
        queryResult.Setup(x => x.Data).Returns(new GetVacancyByReferenceResult([vacancy.Object]));
        recruitGqlClient
            .Setup(x => x.GetVacancyByReference.ExecuteAsync(vacancy.Object.VacancyReference, CancellationToken.None))
            .ReturnsAsync(queryResult.Object);

        // act
        var result = await sut.GetOneByReference(vacancy.Object.VacancyReference!.Value, recruitGqlClient.Object, CancellationToken.None) as Ok<DataResponse<Vacancy>>;

        // assert
        result.Should().NotBeNull();
        result.Value!.Data.Should().NotBeNull();
        result.Value!.Data.Should().BeEquivalentTo(GqlVacancyMapper.From(queryResult.Object.Data!.Vacancies[0]));
    }
}