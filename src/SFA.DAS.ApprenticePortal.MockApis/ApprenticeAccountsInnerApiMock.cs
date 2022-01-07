﻿using System;
using System.Net;
using SFA.DAS.ApprenticePortal.MockApis.Helpers;
using SFA.DAS.ApprenticePortal.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.ApprenticePortal.MockApis
{
    public class ApprenticeAccountsInnerApiMock : ApiMock
    {
        public Apprentice AnyApprentice;

        public ApprenticeAccountsInnerApiMock() : this(0) {}

        public ApprenticeAccountsInnerApiMock(int port = 0, bool ssl = false) : base(port, ssl)
        {
            Console.WriteLine($"Apprentice Accounts Fake Api Running ({BaseAddress})");
            AnyApprentice = Fake.Apprentice;
        }

        public ApprenticeAccountsInnerApiMock WithAnyApprentice()
        {
            WithApprentice(AnyApprentice);
            return this;
        }

        public ApprenticeAccountsInnerApiMock WithApprentice(Apprentice apprentice)
        {
            MockServer
                .Given(
                    Request.Create()
                        .WithPath($"/apprentices/{apprentice.ApprenticeUrlId()}")
                        .UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithBodyAsJson(apprentice)
                );
            return this;
        }

        public ApprenticeAccountsInnerApiMock WithPing()
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
    }
}