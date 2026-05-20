using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using TechTalk.SpecFlow;

namespace SFA.DAS.ApprenticeApp.Api.UnitTests.FeatureSteps
{
    [Binding]
    [Scope(Feature = "PatchApprentice")]
    public class PatchApprenticeSteps
    {
        private readonly TestContext _context;
        private readonly FakePatchOperation[] _patch;
        private FakePatchOperation[] _patchedData;
        private readonly Guid _apprenticeId;

        public PatchApprenticeSteps(TestContext context)
        {
            _context = context;
            _apprenticeId = Guid.NewGuid();

            _patch =
            [
                new FakePatchOperation
                {
                    Op = "replace",
                    Path = "/firstname",
                    Value = "testcase"
                }
            ];
        }

        [When(@"an apprentice patch request to update the email address is received")]
        public async Task WhenAnApprenticePatchRequestToUpdateTheEmailAddressIsReceived()
        {
            _context.ApprenticeAccountsInnerApi.WithPatchApprentice(_apprenticeId);
            await _context.OuterApiClient.Patch($"/apprentices/{_apprenticeId}", _patch);
        }

        [Then(@"the patch request should be passed to the inner API")]
        public void ThenThePatchRequestShouldBePassedToTheInnerAPI()
        {
            _context.ApprenticeAccountsInnerApi.LogEntries.Should().NotBeEmpty();

            var logEntry = _context.ApprenticeAccountsInnerApi.LogEntries
                .Should()
                .Contain(x =>
                    x.RequestMessage.Method == "PATCH" &&
                    x.RequestMessage.Path == $"/apprentices/{_apprenticeId}")
                .Which;

            _patchedData = JsonConvert.DeserializeObject<FakePatchOperation[]>(logEntry.RequestMessage.Body);

            _patchedData.Should().NotBeNull();
            _patchedData.Should().HaveCount(1);
        }

        [Then(@"it contains all the information")]
        public void ThenItContainsAllTheInformation()
        {
            _patchedData.Should().NotBeNull();
            _patchedData.Should().HaveCount(_patch.Length);

            _patchedData[0].Value.Should().Be(_patch[0].Value);
            _patchedData[0].Path.Should().Be(_patch[0].Path);
            _patchedData[0].Op.Should().Be(_patch[0].Op);
        }
    }

    public class FakePatchOperation
    {
        [JsonProperty("value")]
        public object Value { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("op")]
        public string Op { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }
    }
}