using SFA.DAS.LearnerData.Application.GetProviderRelationships;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application.GetProviderRelationships;

[TestFixture]
public class WhenGettingAllProviderRelationships
{
    [Test, MoqAutoData]
    public async Task GetEmployerDetails_Calls_With_Correct_Values(
        GetAllProviderRelationshipQuery request,
        CancellationToken cancellation,
        GetProviderAccountLegalEntitiesResponse[] providerLegalEntitiesresponse,
        GetProvidersResponse providerSummary,
        List<List<EmployerDetails>> employers,
        List<GetCoursesForProviderResponse> coursesForProviderResponses,
       [Frozen] Mock<IGetProviderRelationshipService> getProviderRelationshipService,
       [Frozen] Mock<IRoatpV2TrainingProviderService> roatpService,
       [Greedy] GetAllProvidersRelationshipsQueryHandler sut)
    {
        // Arrange
        int count = providerSummary.RegisteredProviders.Count();
        roatpService.Setup(t => t.GetProviders(cancellation)).
         ReturnsAsync(providerSummary);

        int index = 0;
        foreach (var provider in providerSummary.RegisteredProviders)
        {
            var providerDetails = providerLegalEntitiesresponse[index];
            var employerDetails = employers[index];
            var course = coursesForProviderResponses[index];

            getProviderRelationshipService.Setup(t => t.GetAllProviderRelationShipDetails(provider.Ukprn)).
           ReturnsAsync(providerDetails);

            getProviderRelationshipService.Setup(t => t.GetEmployerDetails((providerDetails))).
                ReturnsAsync(employerDetails);

            getProviderRelationshipService.Setup(t => t.GetCoursesForProviderByUkprn(provider.Ukprn)).
          ReturnsAsync(course);
            index++;
        }

        // Act
        var result = await sut.Handle(request, cancellation);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(providerSummary.RegisteredProviders.Count());
        result.Page.Should().Be(request.Page);
        result.PageSize.Should().Be(request.PageSize);
        result.TotalItems.Should().Be(providerSummary.TotalCount);

        index = 0;

        foreach (var provider in providerSummary.RegisteredProviders)
        {
            var response = result.Items.First(r => r.Ukprn == provider.Ukprn);
            response.Status.Should().Be(Enum.GetName(typeof(ProviderStatusType), provider.StatusId));
            response.Type.Should().Be(Enum.GetName(typeof(ProviderType), provider.ProviderTypeId));
            response.Employers.Should().BeEquivalentTo(employers[index]);
            response.SupportedCourses.Should().BeEquivalentTo(coursesForProviderResponses[index].CourseTypes);
        }
    }

    [Test, MoqAutoData]
    public async Task WhenProviderIsnull_ReturnsEmptyEmployerDetails(
        GetAllProviderRelationshipQuery request,
        CancellationToken cancellation,
        GetProvidersResponse response,
        [Frozen] Mock<IGetProviderRelationshipService> getProviderRelationshipService,
        [Frozen] Mock<IRoatpV2TrainingProviderService> roatpService,
        [Greedy] GetAllProvidersRelationshipsQueryHandler sut)
    {
        // Arrange
        roatpService.Setup(t => t.GetProviders(cancellation)).
            ReturnsAsync((GetProvidersResponse?)null);

        // Act
        var result = await sut.Handle(request, cancellation);

        // Assert
        result?.Items.Should().HaveCount(0);
        result?.Page.Should().Be(request.Page);
        result?.PageSize.Should().Be(request.PageSize);
    }

    [Test, MoqAutoData]
    public async Task WhenProviderRelationshipsIsnull_NoEmployers(
        int ukprnValue,
        GetAllProviderRelationshipQuery request,
        GetProvidersResponse response,
        GetProviderAccountLegalEntitiesResponse providerLegalEntitiesresponse,
        [Frozen] Mock<IGetProviderRelationshipService> getProviderRelationshipService,
        [Frozen] Mock<IRoatpV2TrainingProviderService> roatpService,
        [Greedy] GetAllProvidersRelationshipsQueryHandler sut)
    {
        // Arrange
        getProviderRelationshipService.Setup(t => t.GetAllProviderRelationShipDetails(ukprnValue));

        roatpService.Setup(t => t.GetProviders(CancellationToken.None)).
          ReturnsAsync(response);

        // Act
        var result = await sut.Handle(request, CancellationToken.None);

        // Assert
        result?.Items.Should().HaveCount(0);
    }

    [Test, MoqAutoData]
    public async Task WhenProvidersNull_Returns_Empty_Response(
        [Frozen] Mock<IGetProviderRelationshipService> getProviderRelationshipService,
        [Frozen] Mock<IRoatpV2TrainingProviderService> roatpService,
        [Greedy] GetAllProvidersRelationshipsQueryHandler sut)
    {
        roatpService
            .Setup(s => s.GetProviders(It.IsAny<CancellationToken>()))
            .ReturnsAsync((GetProvidersResponse?)null);

        var request = new GetAllProviderRelationshipQuery { Page = 1, PageSize = 10 };

        // Act
        var result = await sut.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
        result.Items.Should().NotBeNull();
        result.Items.Should().HaveCount(0);
    }


    [Test, MoqAutoData]
    public async Task ShouldProcessProvider_InParallel(
        GetAllProviderRelationshipQuery request,
        CancellationToken cancellation,
        GetProviderAccountLegalEntitiesResponse[] providerLegalEntitiesresponse,
        GetProvidersResponse providerSummary,
        List<List<EmployerDetails>> employers,
        List<GetCoursesForProviderResponse> coursesForProviderResponses,
       [Frozen] Mock<IGetProviderRelationshipService> getProviderRelationshipService,
       [Frozen] Mock<IRoatpV2TrainingProviderService> roatpService,
       [Greedy] GetAllProvidersRelationshipsQueryHandler sut)
    {
        var providers = Enumerable.Range(1, 20)
            .Select(t => new Provider
            {
                Ukprn = t,
                StatusId = 1,
                ProviderTypeId = 1
            });

        request.Page = 1;
        request.PageSize = 10;

        roatpService.Setup(t => t.GetProviders(cancellation))
            .ReturnsAsync(new GetProvidersResponse()
            {
                RegisteredProviders = providers
            });

        getProviderRelationshipService.Setup(s => s.GetAllProviderRelationShipDetails(It.IsAny<int>()))
            .ReturnsAsync(new GetProviderAccountLegalEntitiesResponse());

        getProviderRelationshipService.Setup(s => s.GetCoursesForProviderByUkprn(It.IsAny<long>())).
            ReturnsAsync(new GetCoursesForProviderResponse());

        getProviderRelationshipService.Setup(s => s.GetEmployerDetails(It.IsAny<GetProviderAccountLegalEntitiesResponse>())).
            ReturnsAsync(new List<EmployerDetails>());

        var result = await sut.Handle(request, cancellation);

        result.Should().NotBeNull();
        result.Items.Should().HaveCount(10);
        result.TotalItems.Should().Be(20);

        getProviderRelationshipService.Verify(x => x.GetAllProviderRelationShipDetails(It.IsAny<int>()), Times.Exactly(10));
    }
}