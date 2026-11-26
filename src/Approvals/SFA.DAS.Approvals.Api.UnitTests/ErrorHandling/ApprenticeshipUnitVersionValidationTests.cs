using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using SFA.DAS.Approvals.ErrorHandling;
using SFA.DAS.Approvals.Exceptions;
using SFA.DAS.Approvals.Validation;

namespace SFA.DAS.Approvals.Api.UnitTests.ErrorHandling;

[TestFixture]
public class ApprenticeshipUnitVersionValidationTests
{
    [Test]
    public async Task VersionValidationReturnsBadRequestWithActualStartDateDomainError()
    {
        var builder = WebApplication.CreateBuilder();
        await using var app = builder.Build();
        var pipeline = (IApplicationBuilder)app;
        var exception = new ApprenticeshipUnitVersionException("No version is available for this actual start date.");
        pipeline.UseApiGlobalExceptionHandler(NullLogger.Instance);
        pipeline.Run(_ => throw exception);
        var context = new DefaultHttpContext { RequestServices = app.Services };
        using var body = new MemoryStream();
        context.Response.Body = body;

        await pipeline.Build()(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        context.Response.Headers[HttpHeaderNames.SubStatusCode].ToString().Should().Be("1");
        body.Position = 0;
        using var reader = new StreamReader(body);
        var response = JsonConvert.DeserializeObject<ErrorResponse>(await reader.ReadToEndAsync());
        response.Errors.Should().ContainSingle().Which.Field.Should().Be("ActualStartDate");
        response.Errors[0].Message.Should().Be(exception.Message);
    }
}
