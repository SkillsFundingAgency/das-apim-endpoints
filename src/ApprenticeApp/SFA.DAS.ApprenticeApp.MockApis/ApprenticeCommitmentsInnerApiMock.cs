using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using SFA.DAS.ApprenticeApp.MockApis.Helpers;
using SFA.DAS.ApprenticeApp.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticeApp.MockApis
{
    [ExcludeFromCodeCoverage]
    public class ApprenticeCommitmentsInnerApiMock : ApiMock
    {
        private readonly GetApprenticeApprenticeshipsResult _existingApprenticeships;


        public ApprenticeCommitmentsInnerApiMock() : this(0) {}

        public ApprenticeCommitmentsInnerApiMock(int port, bool ssl = false) : base(port, ssl)
        {
            Console.WriteLine($"Apprentice Commitments Fake Api Running ({BaseAddress})");
            _existingApprenticeships = new GetApprenticeApprenticeshipsResult
            {
                Apprenticeships = new List<Apprenticeship>()
            };
        }

        public ApprenticeCommitmentsInnerApiMock WithPing()
        {
            MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/ping")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                );

            return this;
        }

        public ApprenticeCommitmentsInnerApiMock WithExistingApprenticeshipsForApprentice(Apprentice apprentice)
        {
            _existingApprenticeships.Apprenticeships = Fake.ApprenticeshipsForThisApprentice(apprentice).ToList();
            WithApprenticeshipsResponseForApprentice(apprentice, _existingApprenticeships);

            return this;
        }

        public ApprenticeCommitmentsInnerApiMock WithApprenticeshipsResponseForApprentice(Apprentice apprentice, GetApprenticeApprenticeshipsResult apprenticeshipsResult)
        {
            MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/apprentices/{apprentice.ApprenticeUrlId()}/apprenticeships")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode((int)HttpStatusCode.OK)
                        .WithBodyAsJson(apprenticeshipsResult)
                );

            return this;
        }
    }
}