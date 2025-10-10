using System.Net;
using SFA.DAS.EmployerFeedback.FakeApis;
using SFA.DAS.EmployerFeedback.FakeApis.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.Approvals.FakeApis
{
    public class CommitmentsV2FakeApiBuilder : FakeApiBuilder<CommitmentsV2FakeApiBuilder>
    {
        public const int DefaultPort = 5011;
        public const string DefaultName = "CommitmentsV2Api";

        protected CommitmentsV2FakeApiBuilder(string name, int port) : base(name, port) { }

        public static CommitmentsV2FakeApiBuilder Create(string name = DefaultName, int port = DefaultPort)
        {
            return new CommitmentsV2FakeApiBuilder(name, port);
        }

        public override CommitmentsV2FakeApiBuilder WithPing()
        {
            AddPingPath();
            return this;
        }

        public CommitmentsV2FakeApiBuilder With(GetAccountProvidersCourseStatusRequest request, GetAccountProvidersCourseStatusResponse response)
        {
            _server
                .Given(
                    Request.Create()
                        .WithPathAndParams(request.GetUrl)
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(response)
                );

            return this;
        }
    }
}