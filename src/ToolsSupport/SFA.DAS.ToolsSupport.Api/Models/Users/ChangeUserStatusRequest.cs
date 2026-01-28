using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.ToolsSupport.Api.Models.Users;

public class ChangeUserStatusRequest
{
    [Required]
    public required string ChangedByUserId { get; set; }

    [Required, EmailAddress]
    public required string ChangedByEmail { get; set; }
}

