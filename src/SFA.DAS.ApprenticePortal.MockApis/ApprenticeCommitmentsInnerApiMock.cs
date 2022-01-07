﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using SFA.DAS.ApprenticePortal.MockApis.Helpers;
using SFA.DAS.ApprenticePortal.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace SFA.DAS.ApprenticePortal.MockApis
{
    public class ApprenticeCommitmentsInnerApiMock : ApiMock
    {
        private readonly GetApprenticeApprenticeshipsResult _existingApprenticeships;


        public ApprenticeCommitmentsInnerApiMock() : this(0) {}

        public ApprenticeCommitmentsInnerApiMock(int port = 0, bool ssl = false) : base(port, ssl)
        {
            Console.WriteLine($"Apprentice Accounts Fake Api Running ({BaseAddress})");
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