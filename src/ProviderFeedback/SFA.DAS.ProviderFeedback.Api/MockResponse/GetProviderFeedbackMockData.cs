using SFA.DAS.ProviderFeedback.Application.InnerApi.Responses;
using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackAnnual;
using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackForAcademicYear;

namespace SFA.DAS.ProviderFeedback.Api.MockResponse
{
    public static class GetProviderFeedbackMockData
    {
        public static GetProviderFeedbackForAcademicYearResult GetProviderFeedbackForAcademicYearResultMock()
        {
            return new GetProviderFeedbackForAcademicYearResult
            {
                ProviderStandard = new GetProviderStandardForAcademicYearItem
                {
                    Ukprn = 10000528,
                    EmployerFeedback = GetEmployerFeedbackForAcademicYearResponseMock(),
                    ApprenticeFeedback = GetApprenticeFeedbackForAcademicYearResponseMock()
                }
            };
        }

        public static GetProviderFeedbackAnnualResult GetProviderFeedbackAnnualResultMock()
        {
            return new GetProviderFeedbackAnnualResult
            {
                ProviderStandard = new GetProviderStandardAnnualItem
                {
                    Ukprn = 10000528,
                    EmployerFeedback = GetEmployerFeedbackAnnualResponseMock(),
                    ApprenticeFeedback = GetApprenticeFeedbackAnnualResponseMock()
                }
            };
        }

        private static GetEmployerFeedbackAnnualResponse GetEmployerFeedbackAnnualResponseMock()
        {
            var list = new List<GetEmployerFeedbackStarsAnnualSummaryDto>();

            list.Add(new GetEmployerFeedbackStarsAnnualSummaryDto
            {
                Stars = 1,
                ReviewCount = 1,
                Ukprn = 10,
                TimePeriod = "AY2122",
                ProviderAttribute = new List<GetEmployerFeedbackAnnualAttributeItem>
                {
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Testing Attribute Name",
                        Strength = 1,
                        Weakness = 0
                    },
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Working with small numbers of apprentices",
                        Strength = 1,
                        Weakness = 0
                    },
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Initial assessment of apprentices",
                        Strength = 0,
                        Weakness = 1
                    },
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Adapting to my needs",
                        Strength = 0,
                        Weakness = 1
                    },
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Training facilities",
                        Strength = 1,
                        Weakness = 0
                    }
                }

            });
            list.Add(new GetEmployerFeedbackStarsAnnualSummaryDto
            {
                Stars = 2,
                ReviewCount = 2,
                Ukprn = 20,
                TimePeriod = "AY2223",
                ProviderAttribute = new List<GetEmployerFeedbackAnnualAttributeItem>
                {
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Testing Attribute Name",
                        Strength = 1,
                        Weakness = 0
                    },
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Working with small numbers of apprentices",
                        Strength = 0,
                        Weakness = 1
                    },
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Initial assessment of apprentices",
                        Strength = 0,
                        Weakness = 1
                    },
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Adapting to my needs",
                        Strength = 1,
                        Weakness = 0
                    },
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Training facilities",
                        Strength = 1,
                        Weakness = 0
                    }
                }

            });
            list.Add(new GetEmployerFeedbackStarsAnnualSummaryDto
            {
                Stars = 3,
                ReviewCount = 3,
                Ukprn = 30,
                TimePeriod = "AY2324",
                ProviderAttribute = new List<GetEmployerFeedbackAnnualAttributeItem>
                {
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Testing Attribute Name",
                        Strength = 1,
                        Weakness = 0
                    },
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Working with small numbers of apprentices",
                        Strength = 0,
                        Weakness = 1
                    },
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Initial assessment of apprentices",
                        Strength = 0,
                        Weakness = 1
                    },
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Adapting to my needs",
                        Strength = 1,
                        Weakness = 0
                    },
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Training facilities",
                        Strength = 1,
                        Weakness = 0
                    }
                }

            });
            list.Add(new GetEmployerFeedbackStarsAnnualSummaryDto
            {
                Stars = 3,
                ReviewCount = 3,
                Ukprn = 30,
                TimePeriod = "All",
                ProviderAttribute = new List<GetEmployerFeedbackAnnualAttributeItem>
                {
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Testing Attribute Name",
                        Strength = 0,
                        Weakness = 1
                    },
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Working with small numbers of apprentices",
                        Strength = 0,
                        Weakness = 1
                    },
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Initial assessment of apprentices",
                        Strength = 0,
                        Weakness = 1
                    },
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Adapting to my needs",
                        Strength = 1,
                        Weakness = 0
                    },
                    new GetEmployerFeedbackAnnualAttributeItem
                    {
                        Name = "Training facilities",
                        Strength = 1,
                        Weakness = 0
                    }
                }

            });


            return new GetEmployerFeedbackAnnualResponse
            {
                AnnualEmployerFeedbackDetails = list

            };
        }

        private static GetApprenticeFeedbackAnnualResponse GetApprenticeFeedbackAnnualResponseMock()
        {
            var list = new List<GetApprenticeFeedbackAnnualSummary>();

            list.Add(new GetApprenticeFeedbackAnnualSummary
            {
                Stars = 1,
                ReviewCount = 1,
                Ukprn = 10,
                TimePeriod = "AY2122",
                ProviderAttribute = new List<GetApprenticeFeedbackAnnualAttributeItem>
                {
                    new GetApprenticeFeedbackAnnualAttributeItem
                    {
                        Name = "Testing Attribute Name",
                        Agree = 1,
                        Disagree = 0,
                        Category = "category X"
                    },
                   new GetApprenticeFeedbackAnnualAttributeItem
                   {
                        Name = "Working with small numbers of apprentices",
                        Agree = 0,
                        Disagree = 1,
                        Category = "category Y"
                    },
                   new GetApprenticeFeedbackAnnualAttributeItem
                   {
                        Name = "Initial assessment of apprentices",
                        Agree = 0,
                        Disagree = 1,
                        Category = "category Z"
                    },
                   new GetApprenticeFeedbackAnnualAttributeItem
                   {
                        Name = "Adapting to my needs",
                        Agree = 1,
                        Disagree = 0,
                        Category = "category A"
                    },
                   new GetApprenticeFeedbackAnnualAttributeItem
                   {
                        Name = "Training facilities",
                        Agree = 1,
                        Disagree = 0,
                        Category = "category B"
                    }
                }
            });

            list.Add(new GetApprenticeFeedbackAnnualSummary
            {
                Stars = 1,
                ReviewCount = 1,
                Ukprn = 10,
                TimePeriod = "AY2223",
                ProviderAttribute = new List<GetApprenticeFeedbackAnnualAttributeItem>
                {
                    new GetApprenticeFeedbackAnnualAttributeItem
                    {
                        Name = "Testing Attribute Name",
                        Agree = 1,
                        Disagree = 0,
                        Category = "category X"
                    },
                   new GetApprenticeFeedbackAnnualAttributeItem
                   {
                        Name = "Working with small numbers of apprentices",
                        Agree = 0,
                        Disagree = 1,
                        Category = "category Y"
                    },
                   new GetApprenticeFeedbackAnnualAttributeItem
                   {
                        Name = "Initial assessment of apprentices",
                        Agree = 0,
                        Disagree = 1,
                        Category = "category Z"
                    },
                   new GetApprenticeFeedbackAnnualAttributeItem
                   {
                        Name = "Adapting to my needs",
                        Agree = 1,
                        Disagree = 0,
                        Category = "category A"
                    },
                   new GetApprenticeFeedbackAnnualAttributeItem
                   {
                        Name = "Training facilities",
                        Agree = 1,
                        Disagree = 0,
                        Category = "category B"
                    }
                }
            });

            list.Add(new GetApprenticeFeedbackAnnualSummary
            {
                Stars = 1,
                ReviewCount = 1,
                Ukprn = 10,
                TimePeriod = "AY2324",
                ProviderAttribute = new List<GetApprenticeFeedbackAnnualAttributeItem>
                {
                    new GetApprenticeFeedbackAnnualAttributeItem
                    {
                        Name = "Testing Attribute Name",
                        Agree = 1,
                        Disagree = 0,
                        Category = "category X"
                    },
                   new GetApprenticeFeedbackAnnualAttributeItem
                   {
                        Name = "Working with small numbers of apprentices",
                        Agree = 0,
                        Disagree = 1,
                        Category = "category Y"
                    },
                   new GetApprenticeFeedbackAnnualAttributeItem
                   {
                        Name = "Initial assessment of apprentices",
                        Agree = 0,
                        Disagree = 1,
                        Category = "category Z"
                    },
                   new GetApprenticeFeedbackAnnualAttributeItem
                   {
                        Name = "Adapting to my needs",
                        Agree = 1,
                        Disagree = 0,
                        Category = "category A"
                    },
                   new GetApprenticeFeedbackAnnualAttributeItem
                   {
                        Name = "Training facilities",
                        Agree = 1,
                        Disagree = 0,
                        Category = "category B"
                    }
                }
            });
            list.Add(new GetApprenticeFeedbackAnnualSummary
            {
                Stars = 1,
                ReviewCount = 1,
                Ukprn = 10,
                TimePeriod = "All",
                ProviderAttribute = new List<GetApprenticeFeedbackAnnualAttributeItem>
                {
                    new GetApprenticeFeedbackAnnualAttributeItem
                    {
                        Name = "Testing Attribute Name",
                        Agree = 1,
                        Disagree = 0,
                        Category = "category X"
                    },
                   new GetApprenticeFeedbackAnnualAttributeItem
                   {
                        Name = "Working with small numbers of apprentices",
                        Agree = 0,
                        Disagree = 1,
                        Category = "category Y"
                    },
                   new GetApprenticeFeedbackAnnualAttributeItem
                   {
                        Name = "Initial assessment of apprentices",
                        Agree = 0,
                        Disagree = 1,
                        Category = "category Z"
                    },
                   new GetApprenticeFeedbackAnnualAttributeItem
                   {
                        Name = "Adapting to my needs",
                        Agree = 1,
                        Disagree = 0,
                        Category = "category A"
                    },
                   new GetApprenticeFeedbackAnnualAttributeItem
                   {
                        Name = "Training facilities",
                        Agree = 1,
                        Disagree = 0,
                        Category = "category B"
                    }
                }
            });

            return new GetApprenticeFeedbackAnnualResponse
            {
                AnnualApprenticeFeedbackDetails = list

            };
        }

        private static GetEmployerFeedbackForAcademicYearResponse GetEmployerFeedbackForAcademicYearResponseMock()
        {
            return new GetEmployerFeedbackForAcademicYearResponse
            {
                Ukprn = 10000528,
                Stars = 3,
                ReviewCount = 11,
                ProviderAttribute = new List<GetEmployerFeedbackForAcademicYearAttributeItem>()
                {
                    new GetEmployerFeedbackForAcademicYearAttributeItem
                    {
                        Weakness = 0,
                        Strength = 1,
                        Name = "Testing Attribute Name"
                    },
                    new GetEmployerFeedbackForAcademicYearAttributeItem
                    {
                        Weakness = 1,
                        Strength = 0,
                        Name = "Working with small numbers of apprentices"
                    },
                    new GetEmployerFeedbackForAcademicYearAttributeItem
                    {
                        Weakness = 1,
                        Strength = 0,
                        Name = "Initial assessment of apprentices"
                    },
                    new GetEmployerFeedbackForAcademicYearAttributeItem
                    {
                        Weakness = 0,
                        Strength = 1,
                        Name = "Adapting to my needs"
                    },
                    new GetEmployerFeedbackForAcademicYearAttributeItem
                    {
                        Weakness = 0,
                        Strength = 1,
                        Name = "Training facilities"
                    }
                },
                TimePeriod = "AY2324"
            };
        }

        private static GetApprenticeFeedbackForAcademicYearResponse GetApprenticeFeedbackForAcademicYearResponseMock()
        {
            return new GetApprenticeFeedbackForAcademicYearResponse
            {
                Ukprn = 10000528,
                Stars = 3,
                ReviewCount = 11,
                ProviderAttribute = new List<GetApprenticeFeedbackForAcademicYearAttributeItem>()
                    {
                        new GetApprenticeFeedbackForAcademicYearAttributeItem
                        {
                            Disagree = 0,
                            Agree = 1,
                            Name = "Testing Attribute Name"
                        },
                        new GetApprenticeFeedbackForAcademicYearAttributeItem
                        {
                               Disagree = 0,
                            Agree = 1,
                            Name = "Working with small numbers of apprentices"
                        },
                        new GetApprenticeFeedbackForAcademicYearAttributeItem
                        {
                            Disagree = 0,
                            Agree = 1,
                            Name = "Initial assessment of apprentices"
                        },
                        new GetApprenticeFeedbackForAcademicYearAttributeItem
                        {
                            Disagree = 0,
                            Agree = 1,
                            Name = "Adapting to my needs"
                        },
                        new GetApprenticeFeedbackForAcademicYearAttributeItem
                        {
                            Disagree = 0,
                            Agree = 1,
                            Name = "Training facilities"
                        }
                    },
                TimePeriod = "AY2324"
            };
        }
    }
}
