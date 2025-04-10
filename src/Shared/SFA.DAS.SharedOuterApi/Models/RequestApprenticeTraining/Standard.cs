namespace SFA.DAS.SharedOuterApi.Models.RequestApprenticeTraining
{
    public class Standard
    {
        public string StandardReference { get; set; }
        public string StandardTitle { get; set; }
        public int StandardLevel { get; set; }
        public string StandardSector { get; set; }

        public static explicit operator Standard(InnerApi.Responses.RequestApprenticeTraining.GetStandardResponse source)
        {
            if (source == null) return null;

            return new Standard
            {
                StandardReference = source.IfateReferenceNumber,
                StandardTitle = source.Title,
                StandardLevel = source.Level,
                StandardSector = source.Route,
            };
        }
    }
}