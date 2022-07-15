namespace SFA.DAS.RoatpCourseManagement.InnerApi.Models
{
    public class AddNationalLocationToProviderCourseModel 
    {
        public string UserId { get; }
        public AddNationalLocationToProviderCourseModel(string userId) => UserId = userId;
    }
}
