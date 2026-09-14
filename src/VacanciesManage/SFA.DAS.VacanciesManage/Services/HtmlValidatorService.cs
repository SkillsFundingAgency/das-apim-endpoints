using System;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SFA.DAS.VacanciesManage.Services;

public interface IHtmlValidatorService
{
    ValidationResult ValidateHtml(string html);
}


public class HtmlValidatorService : IHtmlValidatorService
{
    private static readonly HashSet<string> AllowedTags = ["p", "br", "ul", "li"];
    
    private const string Description = "HTML validation error";

    private static readonly HashSet<string> VoidTags = ["br"];

    private static readonly Regex TagToken = new(@"<\s*(/?)\s*([a-zA-Z][a-zA-Z0-9]*)[^>]*?(/?)\s*>", RegexOptions.None, TimeSpan.FromMilliseconds(3));

    public ValidationResult ValidateHtml(string html)
    {
        var result = new ValidationResult();
        if (string.IsNullOrWhiteSpace(html))
            return result;

        // 1. Source-level well-formedness (catches what the parser would silently repair)
        ValidateSourceBalance(html, result);

        // 2. Content-model rules on the parsed DOM (allowed tags, <ul> contents, <li>/script/style)
        var parser = new HtmlParser(new HtmlParserOptions { IsStrictMode = false });
        var body = parser.ParseDocument(html).Body;
        if (body is not null && body.ChildNodes.Any(n => n is IElement))
            ValidateNode(body, result);

        return result;
    }

    private static void ValidateSourceBalance(string html, ValidationResult result)
    {
        var stack = new Stack<string>();

        foreach (Match m in TagToken.Matches(html))
        {
            var closing = m.Groups[1].Value == "/";
            var name = m.Groups[2].Value.ToLowerInvariant();
            var selfClosed = m.Groups[3].Value == "/";

            if (VoidTags.Contains(name) || selfClosed)
                continue; // <br>, <br/> etc. need no closing tag

            if (!closing)
            {
                stack.Push(name);
            }
            else if (stack.Count == 0)
            {
                result.Errors.Add(new ValidationFailure(Description,
                    $"Malformed HTML: stray </{name}> with no matching open tag."));
            }
            else if (stack.Peek() != name)
            {
                result.Errors.Add(new ValidationFailure(Description,
                    $"Malformed HTML: </{name}> does not match the open <{stack.Peek()}> — likely an unclosed tag."));
                if (stack.Contains(name))                 // recover, keep scanning
                    while (stack.Count > 0 && stack.Pop() != name) { }
            }
            else
            {
                stack.Pop();
            }
        }

        foreach (var open in stack)
            result.Errors.Add(new ValidationFailure(Description,
                $"Malformed HTML: <{open}> is never closed."));
    }

    private static void ValidateNode(INode node, ValidationResult result)
    {
        foreach (var child in node.ChildNodes)
        {
            if (child is not IElement element)
                continue;

            var name = element.LocalName;

            // Dedicated messages for dangerous tags; fall back to the allow-list otherwise.
            switch (name)
            {
                case "script":
                    result.Errors.Add(new ValidationFailure(Description, "<script> tags are not allowed."));
                    break;
                case "style":
                    result.Errors.Add(new ValidationFailure(Description, "<style> tags are not allowed."));
                    break;
                default:
                    if (!AllowedTags.Contains(name))
                        result.Errors.Add(new ValidationFailure(Description, $"Tag <{name}> is not allowed."));
                    break;
            }

            switch (name)
            {
                case "ul":
                {
                    foreach (var c in element.ChildNodes)
                    {
                        if (c is IElement e && e.LocalName != "li")
                            result.Errors.Add(new ValidationFailure(Description,
                                $"<ul> may only contain <li>, found <{e.LocalName}>."));
                        else if (c.NodeType == NodeType.Text && !string.IsNullOrWhiteSpace(c.TextContent))
                            result.Errors.Add(new ValidationFailure(Description,
                                "Text is not allowed directly inside <ul>; wrap it in <li>."));
                    }
                    break;
                }
                case "li":
                {
                    if (element.ParentElement?.LocalName != "ul")
                        result.Errors.Add(new ValidationFailure(Description, "<li> must be inside <ul>."));
                    break;
                }
            }

            ValidateNode(element, result);
        }
    }
}

public class ValidationResult
{
    public bool IsValid => Errors.Count == 0;
    public List<ValidationFailure> Errors { get; set; } = [];
}