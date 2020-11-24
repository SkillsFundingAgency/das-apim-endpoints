namespace SFA.DAS.EpaoRegister.Api.Models
{
    public class EpaoListItem
    {
        public string Id { get; set; }
        public uint Ukprn { get; set; }
        public string Name { get; set; }

        public static implicit operator EpaoListItem(InnerApi.Responses.GetEpaosListItem source)
        {
            return new EpaoListItem
            {
                Id = source.Id,
                Ukprn = source.Ukprn,
                Name = source.Name
            };
        }
    }
}