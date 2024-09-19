using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Newtonsoft.Json;
using SFA.DAS.ApprenticePortal.Models;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticePortal.Api.UnitTests.FeatureSteps;

[Binding]
[Scope(Feature = "PutApprentice")]
public class PutApprenticeSteps
{
    private readonly Fixture _fixture = new Fixture();
    private readonly TestContext _context;
    private Apprentice _apprentice;

    public PutApprenticeSteps(TestContext context)
    {
        _context = context;
        _apprentice = _fixture.Create<Apprentice>();
    }
        
    [Given("there is an apprentice put request")]
    public void GiveThereIsAnApprenticePutRequest()
    {
        _context.ApprenticeAccountsInnerApi.WithPutApprentice(_apprentice);
    }
         
    [When(@"the apprentice put is requested")]
    public async Task WhenTheApprenticePutIsRequested()
    {
        await _context.OuterApiClient.Put($"/apprentices", new {GovUkIdentifier = "abc123", Email = "example@test.com"});
        
    }
    [Then(@"the result should contain the apprentice data")]
    public void ThenTheResultShouldContainTheApprenticeData()
    {
        var actualResult = JsonConvert.DeserializeObject<Apprentice>(_context.OuterApiClient.ReturnValue.ToString());
        actualResult.Should().BeEquivalentTo(_apprentice);
    }
}