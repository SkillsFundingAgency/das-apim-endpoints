namespace SFA.DAS.Aodp.InnerApi.AodpApi.Responses
{
    public class GetAllFormsResponse
    {
        public List<Form> Forms { get; set; }

    }

    public class Form
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
        public bool Published { get; set; }
        public string Key { get; set; }
        public string ApplicationTrackingTemplate { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }
}
