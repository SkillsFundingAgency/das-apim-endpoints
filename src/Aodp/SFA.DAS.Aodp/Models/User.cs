namespace SFA.DAS.Aodp.Models
{
    public class User
    {
        public required string Id { get; set; } 
        public required string DisplayName { get; set; }
        public string? EmailAddress { get; set; }
    }
}
