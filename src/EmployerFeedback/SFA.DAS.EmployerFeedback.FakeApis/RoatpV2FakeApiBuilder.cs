using System.Net;
using SFA.DAS.EmployerFeedback.FakeApis.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.EmployerFeedback.FakeApis
{
    public class RoatpV2FakeApiBuilder : FakeApiBuilder<RoatpV2FakeApiBuilder>
    {
        public const int DefaultPort = 5111;
        public const string DefaultName = "RoatpV2API";

        protected RoatpV2FakeApiBuilder(string name, int port) : base(name, port) { }

        public static RoatpV2FakeApiBuilder Create(string name = DefaultName, int port = DefaultPort)
        {
            return new RoatpV2FakeApiBuilder(name, port);
        }

        public override RoatpV2FakeApiBuilder WithPing()
        {
            AddPingPath();
            return this;
        }

        public RoatpV2FakeApiBuilder With(GetRoatpProvidersRequest request, GetProvidersResponse response)
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