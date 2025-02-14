namespace SFA.DAS.Aodp.Application.Queries.FormBuilder.Routes
{
    public class GetAvailableQuestionsForRoutingQueryResponse
    {
        public List<Question> Questions { get; set; }

        public class Question
        {
            public Guid Id { get; set; }
            public string Title { get; set; }
            public int Order { get; set; }

        }
    }
}

