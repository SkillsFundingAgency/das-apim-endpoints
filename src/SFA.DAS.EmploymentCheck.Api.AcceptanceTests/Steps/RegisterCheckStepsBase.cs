using AutoFixture;
using System.Net.Http;
using TechTalk.SpecFlow;

namespace SFA.DAS.EmploymentCheck.Api.AcceptanceTests.Steps
{
    public class RegisterCheckStepsBase
    {
        protected readonly Fixture Fixture;
        protected readonly TestContext Context;
        protected HttpResponseMessage? Response;
        protected string ExpectedResponseBody = "";
        protected const string Url = "/api/EmploymentCheck/RegisterCheck";

        public RegisterCheckStepsBase(TestContext context)
        {
            Fixture = new Fixture();
            Context = context;
        }

        [Given(@"an employer has applied for Apprenticeship Incentive for an apprentice")]
        public void GivenAnEmployerHasAppliedForApprenticeshipIncentiveForAnApprentice()
        {
        }

        [When(@"the employment check request has passed validation")]
        [When(@"the employment check request has failed validation")]
        public void WhenTheEmploymentCheckRequestHasPassedValidation()
        {
        }
    }
}