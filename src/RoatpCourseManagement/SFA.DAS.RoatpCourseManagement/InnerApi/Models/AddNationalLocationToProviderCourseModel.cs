namespace SFA.DAS.RoatpCourseManagement.InnerApi.Models
{
    public class AddNationalLocationToProviderCourseModel 
    {
        public string UserId { get; }
        public string UserDisplayName { get; }
        public AddNationalLocationToProviderCourseModel(string userId, string userDisplayName)
        {
            UserId = userId;
            UserDisplayName = userDisplayName;
        }
    }
}
