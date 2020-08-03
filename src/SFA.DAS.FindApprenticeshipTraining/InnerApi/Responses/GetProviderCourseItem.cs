namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses
{
    public class GetProviderCourseItem
    {
        public int Ukprn { get ; set ; }

        public string Name { get ; set ; }

        public string Email { get ; set ; }

        public string Phone { get ; set ; }

        public string Website { get ; set ; }

        public static implicit operator GetProviderCourseItem(GetProviderStandardItem source)
        {
            return new GetProviderCourseItem
            {
                Website = source.ContactUrl,
                Phone = source.Phone,
                Email = source.Email,
                Name = source.Name,
                Ukprn = source.Ukprn
            };

        }
    }
}