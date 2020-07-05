using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SFA.DAS.EmployerIncentives.Api.AcceptanceTests
{
    public class CustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        //protected override void ConfigureWebHost(IWebHostBuilder builder)
        //{
        //    builder.UseEnvironment(SFA.DAS.CommitmentsV2.Domain.Constants.IntegrationTestEnvironment);
        //}
    }
}