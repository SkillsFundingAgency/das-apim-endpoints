namespace SFA.DAS.ApprenticeAan.Application.Entities
{
    public class ProfileModel
    {
        public long Id { get; set; }
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public int Ordering { get; set; }

        //public static implicit operator ProfileModel(Profile source) => new()
        //{
        //    Id = source.Id,
        //    Category = source.Category,
        //    Description = source.Description,
        //    Ordering = source.Ordering
        //};
    }
}
