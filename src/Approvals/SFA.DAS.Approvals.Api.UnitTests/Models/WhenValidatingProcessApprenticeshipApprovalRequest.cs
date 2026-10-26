using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Api.UnitTests.Models;

public class WhenValidatingProcessApprenticeshipApprovalRequest
{
    [TestCase("UserId")]
    [TestCase("UserDisplayName")]
    [TestCase("UserEmail")]
    public void Then_Control_And_Format_Characters_Are_Rejected_In_Each_Field(string field)
    {
        foreach (var character in new[] { '\0', '\t', '\r', '\n', '\u007f', '\u0085', '\u200b', '\u202e' })
        {
            var request = CreateRequest();
            var value = field == nameof(UserInfo.UserEmail)
                ? $"test{character}user@example.com"
                : $"Test{character}User";
            typeof(UserInfo).GetProperty(field)!.SetValue(request.UserInfo, value);

            var errors = Validate(request);

            errors.SelectMany(error => error.MemberNames).Should().Contain($"UserInfo.{field}");
        }
    }

    [Test]
    public void Then_Unicode_Line_And_Paragraph_Separators_Are_Rejected(
        [Values("UserId", "UserDisplayName", "UserEmail")] string field,
        [Values('\u2028', '\u2029')] char separator)
    {
        var request = CreateRequest();
        var value = field == nameof(UserInfo.UserEmail)
            ? $"test{separator}user@example.com"
            : $"Test{separator}User";
        typeof(UserInfo).GetProperty(field)!.SetValue(request.UserInfo, value);

        Validate(request).SelectMany(error => error.MemberNames).Should().Contain($"UserInfo.{field}");
    }

    [TestCase("UserId")]
    [TestCase("UserDisplayName")]
    [TestCase("UserEmail")]
    public void Then_Invalid_Unicode_Is_Rejected_Without_Throwing(string field)
    {
        var request = CreateRequest();
        var value = field == nameof(UserInfo.UserEmail) ? "test\ud800user@example.com" : "Test\ud800User";
        typeof(UserInfo).GetProperty(field)!.SetValue(request.UserInfo, value);

        var errors = Validate(request);

        errors.SelectMany(error => error.MemberNames).Should().Contain($"UserInfo.{field}");
    }

    [TestCase("Jose\u0301 O’Connor")]
    [TestCase("François L·li")]
    [TestCase("अर्जुन कुमार")]
    [TestCase("می\u200cنا احمد")]
    [TestCase("क्\u200dषितिज शर्मा")]
    [TestCase("李 明")]
    [TestCase("Jane\u00a0Smith")]
    [TestCase("Jean\u2011Luc")]
    [TestCase("\U00010400 Smith")]
    [TestCase("Smith, Jane (HR)")]
    public void Then_Legitimate_Unicode_And_Name_Punctuation_Are_Preserved(string name)
    {
        var request = CreateRequest();
        request.UserInfo.UserDisplayName = name;

        Validate(request).Should().BeEmpty();
        request.UserInfo.UserDisplayName.Should().Be(name);
    }

    [Test]
    public void Then_Normalization_Cannot_Bypass_The_Original_Field_Length_Limit()
    {
        var request = CreateRequest();
        request.UserInfo.UserDisplayName = string.Concat(Enumerable.Repeat("e\u0301", 128));

        Validate(request).SelectMany(error => error.MemberNames).Should().Contain("UserInfo.UserDisplayName");
    }

    [Test]
    public void Then_All_Invalid_Fields_Are_Reported_Without_Echoing_Their_Content()
    {
        var request = CreateRequest();
        const string payload = "<script>alert(1)</script>";
        request.UserInfo = new UserInfo { UserId = payload, UserDisplayName = payload, UserEmail = payload };

        var errors = Validate(request);

        errors.SelectMany(error => error.MemberNames).Should().BeEquivalentTo(
            "UserInfo.UserId", "UserInfo.UserDisplayName", "UserInfo.UserEmail");
        errors.Should().OnlyContain(error => !error.ErrorMessage.Contains(payload));
    }

    private static ProcessApprenticeshipApprovalRequest CreateRequest() => new()
    {
        ApplyChanges = true,
        UserInfo = new UserInfo
        {
            UserId = "00000000-0000-0000-0000-000000000001",
            UserDisplayName = "Test User",
            UserEmail = "test.user@example.com"
        }
    };

    private static List<ValidationResult> Validate(ProcessApprenticeshipApprovalRequest request)
    {
        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(request, new ValidationContext(request), errors, true);
        return errors;
    }
}
