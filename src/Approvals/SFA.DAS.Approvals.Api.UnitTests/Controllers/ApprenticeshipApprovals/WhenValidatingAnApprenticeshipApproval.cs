using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.ApprenticeshipApprovals.Commands.ProcessApprenticeshipApproval;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.ApprenticeshipApprovals;

public class WhenValidatingAnApprenticeshipApproval
{
    private static readonly Guid ApprovalRequestId = Guid.Parse("a59c89ab-9651-4ca4-9a69-9e201963af3b");
    private static readonly string ApprovalUrl = $"/employers/123/apprenticeships/456/approvals/{ApprovalRequestId}";

    [TestCaseSource(nameof(InvalidUserInfoCases))]
    public async Task Then_Returns_BadRequest_Without_Sending_A_Command(
        string field, string value, bool applyChanges)
    {
        using var api = new ApprovalApi();
        var userInfo = new Dictionary<string, string>
        {
            [nameof(UserInfo.UserId)] = "external-user_42",
            [nameof(UserInfo.UserDisplayName)] = "Anne-Marie O'Neill",
            [nameof(UserInfo.UserEmail)] = "anne-marie.oneill+approvals@example.co.uk"
        };
        userInfo[field] = value;

        using var response = await api.Client.PostAsJsonAsync(ApprovalUrl, new { applyChanges, userInfo });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var errors = body.RootElement.GetProperty("errors").GetProperty($"UserInfo.{field}");
        errors.GetArrayLength().Should().BeGreaterThan(0);
        foreach (var error in errors.EnumerateArray())
        {
            error.GetString().Should().NotBeNullOrWhiteSpace();
        }
        api.Mediator.Invocations.Should().BeEmpty();
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task Then_Rejects_The_Pen_Test_Payload_In_Raw_Json(bool unicodeEscaped)
    {
        using var api = new ApprovalApi();
        var displayName = unicodeEscaped
            ? @"TestUser\u003cscript\u003ealert(1)\u003c/script\u003e"
            : "TestUser<script>alert(1)</script>";
        var json = $$"""
            {
                "applyChanges": true,
                "userInfo": {
                    "userId": "00000000-0000-0000-0000-000000000001",
                    "userDisplayName": "{{displayName}}",
                    "userEmail": "approvals.testuser@example.com"
                }
            }
            """;
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await api.Client.PostAsync(ApprovalUrl, content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        body.RootElement.GetProperty("errors").GetProperty("UserInfo.UserDisplayName")
            .GetArrayLength().Should().BeGreaterThan(0);
        api.Mediator.Invocations.Should().BeEmpty();
    }

    [Test]
    public async Task Then_Forwards_Valid_Values_Without_Changing_Them(
        [Values("Anne-Marie O'Neill", "Zoë Dvořák", "李 小龍", "نورة أحمد", "Jose\u0301 García",
            "Dr. Anne-Marie O’Neill (HR), Team / A&B", "Anne\u00a0Marie", "Anne\u2011Marie",
            "می\u200cنا احمد", "क्\u200dषितिज शर्मा")] string displayName,
        [Values(true, false)] bool applyChanges)
    {
        using var api = new ApprovalApi();
        var userInfo = new UserInfo
        {
            UserId = "external-user_42",
            UserDisplayName = displayName,
            UserEmail = "anne-marie.oneill+approvals@example.co.uk"
        };

        using var response = await api.Client.PostAsJsonAsync(ApprovalUrl, new ProcessApprenticeshipApprovalRequest
        {
            ApplyChanges = applyChanges,
            UserInfo = userInfo
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        VerifyForwardedCommand(api, applyChanges, userInfo);
    }

    [Test]
    public async Task Then_Forwards_Valid_Email_Syntax_Without_Changing_It(
        [Values("o'neill@example.com", "\"Jane Smith\"@example.com", "user+approvals@example.co.uk")] string email,
        [Values(true, false)] bool applyChanges)
    {
        using var api = new ApprovalApi();
        var userInfo = new UserInfo
        {
            UserId = "external-user_42",
            UserDisplayName = "Anne-Marie O'Neill",
            UserEmail = email
        };

        using var response = await api.Client.PostAsJsonAsync(ApprovalUrl, new ProcessApprenticeshipApprovalRequest
        {
            ApplyChanges = applyChanges,
            UserInfo = userInfo
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        VerifyForwardedCommand(api, applyChanges, userInfo);
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task Then_Accepts_The_UserId_And_DisplayName_Length_Limits(bool applyChanges)
    {
        using var api = new ApprovalApi();
        var userInfo = new UserInfo
        {
            UserId = new string('a', 100),
            UserDisplayName = new string('A', 255),
            UserEmail = "user@example.com"
        };

        using var response = await api.Client.PostAsJsonAsync(ApprovalUrl, new ProcessApprenticeshipApprovalRequest
        {
            ApplyChanges = applyChanges,
            UserInfo = userInfo
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        VerifyForwardedCommand(api, applyChanges, userInfo);
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task Then_Accepts_The_Email_Length_Limit(bool applyChanges)
    {
        using var api = new ApprovalApi();
        var email = new string('a', 64) + "@" + new string('b', 63) + "." +
                    new string('c', 63) + "." + new string('d', 58) + ".com";
        email.Length.Should().Be(255);
        var userInfo = new UserInfo
        {
            UserId = "external-user_42",
            UserDisplayName = "Anne-Marie O'Neill",
            UserEmail = email
        };

        using var response = await api.Client.PostAsJsonAsync(ApprovalUrl, new ProcessApprenticeshipApprovalRequest
        {
            ApplyChanges = applyChanges,
            UserInfo = userInfo
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        VerifyForwardedCommand(api, applyChanges, userInfo);
    }

    [TestCaseSource(nameof(OptionalUserInfoCases))]
    public async Task Then_Preserves_Optional_Values(
        string userInfoJson, UserInfo expectedUserInfo, bool applyChanges)
    {
        using var api = new ApprovalApi();
        var json = "{\"applyChanges\":" + (applyChanges ? "true" : "false") + userInfoJson + "}";
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = await api.Client.PostAsync(ApprovalUrl, content);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        VerifyForwardedCommand(api, applyChanges, expectedUserInfo);
    }

    private static void VerifyForwardedCommand(ApprovalApi api, bool applyChanges, UserInfo expectedUserInfo)
    {
        api.Mediator.Verify(mediator => mediator.Send(
            It.IsAny<ProcessApprenticeshipApprovalCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        api.Mediator.VerifyNoOtherCalls();
        api.ReceivedCommand.Should().BeEquivalentTo(new ProcessApprenticeshipApprovalCommand
        {
            ApprenticeshipId = 456,
            ApprovalRequestId = ApprovalRequestId,
            ApplyChanges = applyChanges,
            UserInfo = expectedUserInfo
        });
    }

    private static IEnumerable<TestCaseData> InvalidUserInfoCases()
    {
        var cases = new (string Field, string Value, string Description)[]
        {
            (nameof(UserInfo.UserDisplayName), "<script>alert('XSS')</script>", "Script_In_DisplayName"),
            (nameof(UserInfo.UserDisplayName), "<b>Jane Smith</b>", "Markup_In_DisplayName"),
            (nameof(UserInfo.UserDisplayName), "&lt;script&gt;alert(1)&lt;/script&gt;", "Encoded_Markup_In_DisplayName"),
            (nameof(UserInfo.UserDisplayName), "Jane\r\nSmith", "Newlines_In_DisplayName"),
            (nameof(UserInfo.UserDisplayName), "Jane\0Smith", "Null_Control_In_DisplayName"),
            (nameof(UserInfo.UserDisplayName), new string('A', 256), "Overlong_DisplayName"),
            (nameof(UserInfo.UserId), "<script>alert(1)</script>", "Markup_In_UserId"),
            (nameof(UserInfo.UserId), "external-user\r\n42", "Newlines_In_UserId"),
            (nameof(UserInfo.UserId), "external-user\t42", "Tab_In_UserId"),
            (nameof(UserInfo.UserId), new string('a', 101), "Overlong_UserId"),
            (nameof(UserInfo.UserEmail), "<script>alert(1)</script>@example.com", "Markup_In_Email"),
            (nameof(UserInfo.UserEmail), "user@example.com\r\nBcc:other@example.com", "Newlines_In_Email"),
            (nameof(UserInfo.UserEmail), "test\u2028user@example.com", "Unicode_Line_Separator_In_Email"),
            (nameof(UserInfo.UserEmail), "test\u2029user@example.com", "Unicode_Paragraph_Separator_In_Email"),
            (nameof(UserInfo.UserEmail), "not-an-email", "Malformed_Email"),
            (nameof(UserInfo.UserEmail), "user@@example.com", "Multiple_At_Signs_In_Email"),
            (nameof(UserInfo.UserEmail), "user name@example.com", "Whitespace_In_Email"),
            (nameof(UserInfo.UserEmail), new string('a', 244) + "@example.com", "Overlong_Email")
        };

        foreach (var (field, value, description) in cases)
        {
            foreach (var applyChanges in new[] { true, false })
            {
                yield return new TestCaseData(field, value, applyChanges)
                    .SetName($"Then_Rejects_{description}_When_ApplyChanges_Is_{applyChanges}");
            }
        }
    }

    private static IEnumerable<TestCaseData> OptionalUserInfoCases()
    {
        var cases = new (string Json, UserInfo Expected, string Description)[]
        {
            ("", null, "Omitted_UserInfo"),
            (",\"userInfo\":null", null, "Null_UserInfo"),
            (",\"userInfo\":{}", new UserInfo(), "Omitted_Members"),
            (",\"userInfo\":{\"userId\":null,\"userDisplayName\":null,\"userEmail\":null}", new UserInfo(), "Null_Members"),
            (",\"userInfo\":{\"userId\":\"\",\"userDisplayName\":\"\",\"userEmail\":\"\"}",
                new UserInfo { UserId = "", UserDisplayName = "", UserEmail = "" }, "Empty_Members"),
            (",\"userInfo\":{\"userId\":\"external-user_42\",\"userDisplayName\":\"Anne-Marie O'Neill\"}",
                new UserInfo { UserId = "external-user_42", UserDisplayName = "Anne-Marie O'Neill" }, "Omitted_Email"),
            (",\"userInfo\":{\"userId\":\"external-user_42\",\"userDisplayName\":\"Anne-Marie O'Neill\",\"userEmail\":\"\"}",
                new UserInfo { UserId = "external-user_42", UserDisplayName = "Anne-Marie O'Neill", UserEmail = "" }, "Empty_Email")
        };

        foreach (var (json, expected, description) in cases)
        {
            foreach (var applyChanges in new[] { true, false })
            {
                yield return new TestCaseData(json, expected, applyChanges)
                    .SetName($"Then_Preserves_{description}_When_ApplyChanges_Is_{applyChanges}");
            }
        }
    }

    private sealed class ApprovalApi : IDisposable
    {
        private readonly IHost _host;

        public ApprovalApi()
        {
            Mediator.Setup(mediator => mediator.Send(
                    It.IsAny<ProcessApprenticeshipApprovalCommand>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<Unit>, CancellationToken>((command, _) => ReceivedCommand = (ProcessApprenticeshipApprovalCommand)command)
                .ReturnsAsync(Unit.Value);

            _host = new HostBuilder()
                .ConfigureWebHost(webHost => webHost
                    .UseTestServer()
                    .ConfigureServices(services =>
                    {
                        services.AddLogging();
                        services.AddSingleton(Mediator.Object);
                        services.AddControllers().AddApplicationPart(typeof(ApprenticeshipApprovalsController).Assembly);
                    })
                    .Configure(app =>
                    {
                        app.UseRouting();
                        app.UseEndpoints(endpoints => endpoints.MapControllers());
                    }))
                .Start();
            Client = _host.GetTestClient();
        }

        public Mock<IMediator> Mediator { get; } = new();
        public ProcessApprenticeshipApprovalCommand ReceivedCommand { get; private set; }
        public HttpClient Client { get; }

        public void Dispose()
        {
            Client.Dispose();
            _host.Dispose();
        }
    }
}
