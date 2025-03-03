using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using AutoFixture;
using SFA.DAS.Payments.Models.Responses;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace SFA.DAS.Payments.Stubs
{
    public class Program
    {
        private static WireMockServer _fakeLearnerDataApi;

        static void Main(string[] args)
        {
            try
            {
                _fakeLearnerDataApi = WireMockServer.Start(new WireMockServerSettings
                {
                    Urls = new[] { "http://*:6011" },
                    StartAdminInterface = true,
                });

                SetupGetLearnersResponse();

                Console.WriteLine(("Please RETURN to stop server"));
                Console.ReadLine();
            }
            finally
            {
                _fakeLearnerDataApi.Stop();
                _fakeLearnerDataApi.Dispose();
            }
        }

        private static void SetupGetLearnersResponse()
        {
            _fakeLearnerDataApi.Given(
                    Request.Create().WithPath($"/learners/*")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithHeader("Content-Type", "application/json")
                        .WithBody(JsonSerializer.Serialize(GetLearnerResponseObject())));
        }

        private static List<LearnerResponse> GetLearnerResponseObject()
        {
            return new Fixture().CreateMany<LearnerResponse>().ToList();
        }
    }
}
