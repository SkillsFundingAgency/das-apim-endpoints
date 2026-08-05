using System.Collections.Generic;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Models;

public record ProviderEmailModel(string TemplateId, Dictionary<string, string> Tokens);

