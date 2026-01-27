using SFA.DAS.FindApprenticeshipJobs.Application.Queries.CivilServiceJobs;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Application;
[TestFixture]
internal class WhenMappingTheRoute
{
    private List<GetRoutesListItem> _routes;

    [SetUp]
    public void Setup()
    {
        _routes =
        [
            new GetRoutesListItem { Name = "Agriculture" },
            new GetRoutesListItem { Name = "Business Admin" },
            new GetRoutesListItem { Name = "Business Services" },
            new GetRoutesListItem { Name = "Care Services" },
            new GetRoutesListItem { Name = "Construction & Trades" },
            new GetRoutesListItem { Name = "Digital" },
            new GetRoutesListItem { Name = "Engineering Technologies" },
            new GetRoutesListItem { Name = "Health Sciences" },
            new GetRoutesListItem { Name = "Legal Services" },
            new GetRoutesListItem { Name = "Protective Services" },
            new GetRoutesListItem { Name = "Sales & Marketing" }
        ];
    }

    // -------------------------------------------------------------
    // Null / Empty
    // -------------------------------------------------------------
    [Test, MoqAutoData]
    public void When_Profession_Is_Null_Should_Return_Default_Business([Greedy] GetCivilServiceJobsQueryHandler handler)
    {
        var result = GetCivilServiceJobsQueryHandler.GetBusinessRoute(_routes, null);
        result!.Name.Should().StartWith("Business");
    }

    [Test, MoqAutoData]
    public void When_Profession_Is_Empty_Should_Return_Default_Business([Greedy] GetCivilServiceJobsQueryHandler handler)
    {
        var result = GetCivilServiceJobsQueryHandler.GetBusinessRoute(_routes, "");
        result!.Name.Should().StartWith("Business");
    }

    // -------------------------------------------------------------
    // Agriculture
    // -------------------------------------------------------------
    [Test, MoqAutoData]
    public void Agriculture_Profession_Should_Return_Agriculture([Greedy] GetCivilServiceJobsQueryHandler handler)
    {
        var result = GetCivilServiceJobsQueryHandler.GetBusinessRoute(_routes, "Government Veterinary Profession");
        result!.Name.Should().Be("Agriculture");
    }

    // -------------------------------------------------------------
    // Business Group
    // -------------------------------------------------------------
    [Test, MoqAutoData]
    public void Business_Group_Should_Return_Business([Greedy] GetCivilServiceJobsQueryHandler handler)
    {
        var result = GetCivilServiceJobsQueryHandler.GetBusinessRoute(_routes, "Commercial");
        result!.Name.Should().StartWith("Business");
    }

    [Test, MoqAutoData]
    public void Business_Group_CaseInsensitive_Should_Still_Work([Greedy] GetCivilServiceJobsQueryHandler handler)
    {
        var result = GetCivilServiceJobsQueryHandler.GetBusinessRoute(_routes, "commercial");
        result!.Name.Should().StartWith("Business");
    }

    // -------------------------------------------------------------
    // Care
    // -------------------------------------------------------------
    [Test, MoqAutoData]
    public void Care_Profession_Should_Return_Care_Route([Greedy] GetCivilServiceJobsQueryHandler handler)
    {
        var result = GetCivilServiceJobsQueryHandler.GetBusinessRoute(_routes, "Psychology Profession");
        result!.Name.Should().StartWith("Care");
    }

    // -------------------------------------------------------------
    // Construction
    // -------------------------------------------------------------
    [Test, MoqAutoData]
    public void Construction_Profession_Should_Return_Construction([Greedy] GetCivilServiceJobsQueryHandler handler)
    {
        var result = GetCivilServiceJobsQueryHandler.GetBusinessRoute(_routes, "Planning Profession");
        result!.Name.Should().StartWith("Construction");
    }

    // -------------------------------------------------------------
    // Digital
    // -------------------------------------------------------------
    [Test, MoqAutoData]
    public void Digital_Profession_Should_Return_Digital([Greedy] GetCivilServiceJobsQueryHandler handler)
    {
        var result = GetCivilServiceJobsQueryHandler.GetBusinessRoute(_routes, "Government Digital and Data Profession");
        result!.Name.Should().Be("Digital");
    }

    // -------------------------------------------------------------
    // Engineering
    // -------------------------------------------------------------
    [Test, MoqAutoData]
    public void Engineering_Profession_Should_Return_Engineering([Greedy] GetCivilServiceJobsQueryHandler handler)
    {
        var result = GetCivilServiceJobsQueryHandler.GetBusinessRoute(_routes, "Government Science and Engineering");
        result!.Name.Should().StartWith("Engineering");
    }

    // -------------------------------------------------------------
    // Health
    // -------------------------------------------------------------
    [Test, MoqAutoData]
    public void Health_Profession_Should_Return_Health([Greedy] GetCivilServiceJobsQueryHandler handler)
    {
        var result = GetCivilServiceJobsQueryHandler.GetBusinessRoute(_routes, "Medical Profession");
        result!.Name.Should().StartWith("Health");
    }

    // -------------------------------------------------------------
    // Legal
    // -------------------------------------------------------------
    [Test, MoqAutoData]
    public void Legal_Profession_Should_Return_Legal([Greedy] GetCivilServiceJobsQueryHandler handler)
    {
        var result = GetCivilServiceJobsQueryHandler.GetBusinessRoute(_routes, "Internal Audit");
        result!.Name.Should().StartWith("Legal");
    }

    // -------------------------------------------------------------
    // Protective Services
    // -------------------------------------------------------------
    [Test, MoqAutoData]
    public void Protective_Profession_Should_Return_Protective_Services([Greedy] GetCivilServiceJobsQueryHandler handler)
    {
        var result = GetCivilServiceJobsQueryHandler.GetBusinessRoute(_routes, "Security Profession");
        result!.Name.Should().StartWith("Protective");
    }

    // -------------------------------------------------------------
    // Sales
    // -------------------------------------------------------------
    [Test, MoqAutoData]
    public void Communication_Profession_Should_Return_Sales([Greedy] GetCivilServiceJobsQueryHandler handler)
    {
        var result = GetCivilServiceJobsQueryHandler.GetBusinessRoute(_routes, "Government Communication Service");
        result!.Name.Should().StartWith("Sales");
    }

    // -------------------------------------------------------------
    // Default route
    // -------------------------------------------------------------
    [Test, MoqAutoData]
    public void Unknown_Profession_Should_Default_To_Business([Greedy] GetCivilServiceJobsQueryHandler handler)
    {
        var result = GetCivilServiceJobsQueryHandler.GetBusinessRoute(_routes, "Unknown Profession");
        result!.Name.Should().StartWith("Business");
    }
}
