using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace SFA.DAS.VacanciesManage.Services;

public interface IHtmlValidatorService
{
    ValidationResult ValidateHtml(string html);
}

public class HtmlValidatorService : IHtmlValidatorService
{
    private static readonly HashSet<string> AllowedTags =
    [
        "p", "br", "strong", "b", "em", "i", "u",
        "ul", "ol", "li", "a"
    ];

    private static bool ContainsHtml(string input)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(input);

        return doc.DocumentNode
            .Descendants()
            .Any(x => x.NodeType == HtmlNodeType.Element);
    }

    public ValidationResult ValidateHtml(string html)
    {
        var result = new ValidationResult();

        if (string.IsNullOrWhiteSpace(html))
            return result;

        if (!ContainsHtml(html))
            return result;

        var doc = new HtmlDocument
        {
            OptionCheckSyntax = true // default is already true, just being explicit
        };
        doc.LoadHtml(html);

        // 0. Catch malformed/broken markup (unclosed tags, mismatched tags, etc.)
        var parseErrors = doc.ParseErrors.ToList() ?? [];
        foreach (var error in parseErrors)
        {
            result.Errors.Add(new ValidationFailure(
                "Description",
                $"{error.Reason}"));
        }

        // 1. Validate allowed tags
        foreach (var node in doc.DocumentNode.Descendants())
        {
            if (node.NodeType != HtmlNodeType.Element)
                continue;

            if (!AllowedTags.Contains(node.Name))
            {
                result.Errors.Add(new ValidationFailure("Description", $"Tag <{node.Name}> is not allowed."));
            }
        }

        // 2. Validate <li> structure
        foreach (var li in doc.DocumentNode.Descendants("li"))
        {
            var parent = li.ParentNode;

            if (parent.Name != "ul" && parent.Name != "ol")
            {
                result.Errors.Add(new ValidationFailure("Description", "<li> must be inside <ul> or <ol>."));
            }
        }

        // 3. Block dangerous tags explicitly
        if (doc.DocumentNode.Descendants("script").Any())
        {
            result.Errors.Add(new ValidationFailure("Description", "<script> tags are not allowed."));
        }

        if (doc.DocumentNode.Descendants("style").Any())
        {
            result.Errors.Add(new ValidationFailure("Description", "<style> tags are not allowed."));
        }

        return result;
    }
}

public class ValidationResult
{
    public bool IsValid => Errors.Count == 0;
    public List<ValidationFailure> Errors { get; set; } = [];
}