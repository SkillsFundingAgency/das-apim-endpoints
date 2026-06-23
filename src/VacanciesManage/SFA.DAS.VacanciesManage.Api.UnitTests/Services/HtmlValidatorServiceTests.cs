using AutoFixture.NUnit4;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.VacanciesManage.Services;
using System.Linq;

namespace SFA.DAS.VacanciesManage.Api.UnitTests.Services;

[TestFixture]
internal class HtmlValidatorServiceTests
{
    [Test, MoqAutoData]
    public void ValidateHtml_WhenInputIsNull_ReturnsNoErrors([Greedy] HtmlValidatorService htmlValidatorService)
    {
        var result = htmlValidatorService.ValidateHtml(null!);

        result.Should().NotBeNull();
        result.Errors.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public void ValidateHtml_WhenInputIsWhitespace_ReturnsNoErrors([Greedy] HtmlValidatorService htmlValidatorService)
    {
        var result = htmlValidatorService.ValidateHtml("   ");

        result.Errors.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public void ValidateHtml_WhenInputContainsNoHtml_ReturnsNoErrors([Greedy] HtmlValidatorService htmlValidatorService)
    {
        var result = htmlValidatorService.ValidateHtml("plain text only");

        result.Errors.Should().BeEmpty();
    }

    [Test, MoqAutoData]
    public void ValidateHtml_WhenHtmlIsMalformed_AddsParseErrors([Greedy] HtmlValidatorService htmlValidatorService)
    {
        var result = htmlValidatorService.ValidateHtml("<div><span>missing closing tags");

        result.Errors.Should().NotBeEmpty();
        result.Errors.Select(e => e.PropertyName)
            .Should().AllBeEquivalentTo("HTML validation error");
    }

    [Test, MoqAutoData]
    public void ValidateHtml_WhenTagIsNotAllowed_AddsValidationError([Greedy] HtmlValidatorService htmlValidatorService)
    {
        var result = htmlValidatorService.ValidateHtml("<customtag>Hello</customtag>");

        result.Errors.Should().Contain(e =>
            e.ErrorMessage.Contains("Tag <customtag> is not allowed."));
    }

    [Test, MoqAutoData]
    public void ValidateHtml_WhenLiIsOutsideUlOrOl_AddsValidationError([Greedy] HtmlValidatorService htmlValidatorService)
    {
        var result = htmlValidatorService.ValidateHtml("<div><li>Item</li></div>");

        result.Errors.Should().Contain(e =>
            e.ErrorMessage.Contains("<li> must be inside <ul>."));
    }

    [Test, MoqAutoData]
    public void ValidateHtml_WhenScriptTagExists_AddsValidationError([Greedy] HtmlValidatorService htmlValidatorService)
    {
        var result = htmlValidatorService.ValidateHtml("<div><script>alert('x')</script></div>");

        result.Errors.Should().Contain(e =>
            e.ErrorMessage.Contains("<script> tags are not allowed."));
    }

    [Test, MoqAutoData]
    public void ValidateHtml_WhenStyleTagExists_AddsValidationError([Greedy] HtmlValidatorService htmlValidatorService)
    {
        var result = htmlValidatorService.ValidateHtml("<div><style>body{color:red;}</style></div>");

        result.Errors.Should().Contain(e =>
            e.ErrorMessage.Contains("<style> tags are not allowed."));
    }

    [Test, MoqAutoData]
    public void ValidateHtml_WhenHtmlIsValid_ReturnsNoErrors([Greedy] HtmlValidatorService htmlValidatorService)
    {
        var result = htmlValidatorService.ValidateHtml("<ul><li>Item 1</li><li>Item 2</li></ul>");

        result.Errors.Should().BeEmpty();
    }
}