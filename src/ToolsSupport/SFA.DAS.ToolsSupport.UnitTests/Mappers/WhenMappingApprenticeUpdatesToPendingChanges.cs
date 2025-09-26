using AutoFixture;
using FluentAssertions;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Extensions;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Mappers;
using Party = SFA.DAS.ToolsSupport.InnerApi.Responses.Party;

namespace SFA.DAS.ToolsSupport.UnitTests.Mappers;

public class WhenMappingApprenticeUpdatesToPendingChanges
{
    [TestCase(null)]
    [TestCase(0)]
    public void And_No_Items_Exist_Then_Empty_List_Returned(int? numberOfItems)
    {
        var fixture = new Fixture();

        var updatesResponse = new GetApprenticeshipPendingUpdatesResponse();
        if (numberOfItems == 0)
            updatesResponse.ApprenticeshipUpdates = new List<ApprenticeshipUpdate>(); 

        var sut = new PendingChangesMapper();

        var actual = sut.CreatePendingChangesResponse(updatesResponse, fixture.Create<SupportApprenticeshipDetails>());

        actual.Changes.ToList().Count.Should().Be(0);
        actual.Description.Should().Be("There are no changes for this record");
    }

    [Test, MoqAutoData]
    public void Then_Description_Should_Should_Show_Employer_Changes(
        GetApprenticeshipPendingUpdatesResponse updatesResponse,
        SupportApprenticeshipDetails apprenticeship,
        PendingChangesMapper sut)
    {
        updatesResponse.ApprenticeshipUpdates[0].OriginatingParty = Party.Employer;

        var actual = sut.CreatePendingChangesResponse(updatesResponse, apprenticeship);

        actual.Description.Should().Be("Requested By Employer");
    }

    [Test, MoqAutoData]
    public void Then_Description_Should_Should_Show_Provider_Changes(
        GetApprenticeshipPendingUpdatesResponse updatesResponse,
        SupportApprenticeshipDetails apprenticeship,
        PendingChangesMapper sut)
    {
        updatesResponse.ApprenticeshipUpdates[0].OriginatingParty = Party.Provider;

        var actual = sut.CreatePendingChangesResponse(updatesResponse, apprenticeship);

        actual.Description.Should().Be("Requested By Training Provider");
    }

    [Test, MoqAutoData]
    public void Then_Name_Should_Be_Mapped(
        string firstName, 
        string lastName,
        SupportApprenticeshipDetails apprenticeship,
        PendingChangesMapper sut)
    {
        var update = new ApprenticeshipUpdate
        {
            FirstName = firstName,
            LastName = lastName
        };

        var actual = sut.CreatePendingChangesResponse(BuildPendingUpdateResponseWith(update), apprenticeship).Changes.ToList().First();

        actual.Name.Should().Be("Name");
        actual.OriginalValue.Should().Be($"{apprenticeship.FirstName} {apprenticeship.LastName}");
        actual.NewValue.Should().Be($"{update.FirstName} {update.LastName}");
    }

    [Test, MoqAutoData]
    public void Then_Email_Should_Be_Mapped(
        string email,
        SupportApprenticeshipDetails apprenticeship,
        PendingChangesMapper sut)
    {
        var update = new ApprenticeshipUpdate
        {
            Email = email
        };

        var actual = sut.CreatePendingChangesResponse(BuildPendingUpdateResponseWith(update), apprenticeship).Changes.ToList().First();

        actual.Name.Should().Be("Email");
        actual.OriginalValue.Should().Be(apprenticeship.Email);
        actual.NewValue.Should().Be(update.Email);
    }

    [Test, MoqAutoData]
    public void Then_DoB_Should_Be_Mapped(
        DateTime dob,
        SupportApprenticeshipDetails apprenticeship,
        PendingChangesMapper sut)
    {
        var update = new ApprenticeshipUpdate
        {
            DateOfBirth = dob
        };

        var actual = sut.CreatePendingChangesResponse(BuildPendingUpdateResponseWith(update), apprenticeship).Changes.ToList().First();

        actual.Name.Should().Be("Date of birth");
        actual.OriginalValue.Should().Be(apprenticeship.DateOfBirth.ToString("dd MMM yyyy"));
        actual.NewValue.Should().Be(update.DateOfBirth.Value.ToString("dd MMM yyyy"));
    }

    [Test, MoqAutoData]
    public void Then_DeliveryModel_Should_Be_Mapped(
        GetApprenticeshipUpdatesResponse.DeliveryModel dm,
        SupportApprenticeshipDetails apprenticeship,
        PendingChangesMapper sut)
    {
        var update = new ApprenticeshipUpdate
        {
            DeliveryModel = dm
        };

        var actual = sut.CreatePendingChangesResponse(BuildPendingUpdateResponseWith(update), apprenticeship).Changes.ToList().First();

        actual.Name.Should().Be("Apprenticeship delivery model");
        actual.OriginalValue.Should().Be(apprenticeship.DeliveryModel.ToDescription());
        actual.NewValue.Should().Be(update.DeliveryModel.ToDescription());
    }

    [Test, MoqAutoData]
    public void Then_EmploymentEndDate_Should_Be_Mapped(
        DateTime empEndDate,
        SupportApprenticeshipDetails apprenticeship,
        PendingChangesMapper sut)
    {
        var update = new ApprenticeshipUpdate
        {
            EmploymentEndDate = empEndDate
        };

        var actual = sut.CreatePendingChangesResponse(BuildPendingUpdateResponseWith(update), apprenticeship).Changes.ToList().First();

        actual.Name.Should().Be("Planned end date of this employment");
        actual.OriginalValue.Should().Be(apprenticeship.EmploymentEndDate.Value.ToString("dd MMM yyyy"));
        actual.NewValue.Should().Be(update.EmploymentEndDate.Value.ToString("dd MMM yyyy"));
    }

    [Test, MoqAutoData]
    public void Then_EmploymentPrice_Should_Be_Mapped(
        int price,
        SupportApprenticeshipDetails apprenticeship,
        PendingChangesMapper sut)
    {
        var update = new ApprenticeshipUpdate
        {
            EmploymentPrice = price
        };

        var actual = sut.CreatePendingChangesResponse(BuildPendingUpdateResponseWith(update), apprenticeship).Changes.ToList().First();

        actual.Name.Should().Be("Training price for this employment");
        actual.OriginalValue.Should().Be($"£{apprenticeship.EmploymentPrice.Value:n0}");
        actual.NewValue.Should().Be($"£{update.EmploymentPrice.Value:n0}");
    }

    [Test, MoqAutoData]
    public void Then_StartDate_Should_Be_Mapped(
        DateTime date,
        SupportApprenticeshipDetails apprenticeship,
        PendingChangesMapper sut)
    {
        var update = new ApprenticeshipUpdate
        {
            StartDate = date
        };

        var actual = sut.CreatePendingChangesResponse(BuildPendingUpdateResponseWith(update), apprenticeship).Changes.ToList().First();

        actual.Name.Should().Be("Planned training start date");
        actual.OriginalValue.Should().Be(apprenticeship.StartDate.ToString("MMM yyyy"));
        actual.NewValue.Should().Be(update.StartDate.Value.ToString("MMM yyyy"));
    }

    [Test, MoqAutoData]
    public void Then_EndDate_Should_Be_Mapped(
        DateTime date,
        SupportApprenticeshipDetails apprenticeship,
        PendingChangesMapper sut)
    {
        var update = new ApprenticeshipUpdate
        {
            EndDate = date
        };

        var actual = sut.CreatePendingChangesResponse(BuildPendingUpdateResponseWith(update), apprenticeship).Changes.ToList().First();

        actual.Name.Should().Be("Planned training end date");
        actual.OriginalValue.Should().Be(apprenticeship.EndDate.ToString("MMM yyyy"));
        actual.NewValue.Should().Be(update.EndDate.Value.ToString("MMM yyyy"));
    }

    [Test, MoqAutoData]
    public void The_Cost_Should_Be_Mapped(
        decimal price,
        SupportApprenticeshipDetails apprenticeship,
        PendingChangesMapper sut)
    {
        var update = new ApprenticeshipUpdate
        {
            Cost = price
        };

        var actual = sut.CreatePendingChangesResponse(BuildPendingUpdateResponseWith(update), apprenticeship).Changes.ToList().First();

        actual.Name.Should().Be("Cost");
        actual.OriginalValue.Should().Be($"£{apprenticeship.Cost.Value:n0}");
        actual.NewValue.Should().Be($"£{update.Cost.Value:n0}");
    }

    [Test, MoqAutoData]
    public void The_TrainingCode_Should_Be_Mapped(
        string code,
        SupportApprenticeshipDetails apprenticeship,
        PendingChangesMapper sut)
    {
        var update = new ApprenticeshipUpdate
        {
            TrainingCode = code
        };

        var actual = sut.CreatePendingChangesResponse(BuildPendingUpdateResponseWith(update), apprenticeship).Changes.ToList().First();

        actual.Name.Should().Be("Apprenticeship training course");
        actual.OriginalValue.Should().Be(apprenticeship.CourseName);
        actual.NewValue.Should().Be(update.TrainingName);
    }

    [Test, MoqAutoData]
    public void The_Version_Should_Be_Mapped(
        string code,
        SupportApprenticeshipDetails apprenticeship,
        PendingChangesMapper sut)
    {
        var update = new ApprenticeshipUpdate
        {
            Version = code
        };

        var actual = sut.CreatePendingChangesResponse(BuildPendingUpdateResponseWith(update), apprenticeship).Changes.ToList().First();

        actual.Name.Should().Be("Version");
        actual.OriginalValue.Should().Be(apprenticeship.TrainingCourseVersion);
        actual.NewValue.Should().Be(update.Version);
    }

    [Test, MoqAutoData]
    public void The_Option_Should_Be_Mapped(
        string code,
        SupportApprenticeshipDetails apprenticeship,
        PendingChangesMapper sut)
    {
        var update = new ApprenticeshipUpdate
        {
            Option = code
        };

        var actual = sut.CreatePendingChangesResponse(BuildPendingUpdateResponseWith(update), apprenticeship).Changes.ToList().First();

        actual.Name.Should().Be("Option");
        actual.OriginalValue.Should().Be(apprenticeship.TrainingCourseOption);
        actual.NewValue.Should().Be(update.Option);
    }

    private GetApprenticeshipPendingUpdatesResponse BuildPendingUpdateResponseWith(ApprenticeshipUpdate update)
    {
        return new GetApprenticeshipPendingUpdatesResponse()
        {
            ApprenticeshipUpdates = new List<ApprenticeshipUpdate>
            {
                update
            }
        };
    }
}

