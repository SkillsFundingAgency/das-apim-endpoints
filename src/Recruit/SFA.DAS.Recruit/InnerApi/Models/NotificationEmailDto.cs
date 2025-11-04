using System;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.InnerApi.Models;

public class NotificationEmailDto
{
    public required Guid TemplateId { get; set; }
    public required string RecipientAddress { get; set; }
    public required Dictionary<string, string> Tokens { get; set; } = [];
}