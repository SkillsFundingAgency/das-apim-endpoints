using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses.Shared;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.FindApprenticeApi.Responses.Shared;

public class WhenComparingSearchParametersDto
{
    private static readonly object?[] EqualTestCases =
    [
        new object[] { new SearchParametersDto("foo", null, null, false, null, null, null, null), new SearchParametersDto("foo", null, null, false, null, null, null, null) },
        new object[] { new SearchParametersDto(null, [1, 2], null, false, null, null, null, null), new SearchParametersDto(null, [1, 2], null, false, null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, 1, false, null, null, null, null), new SearchParametersDto(null, null, 1, false, null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, null, null, null, null), new SearchParametersDto(null, null, null, false, null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, true, null, null, null, null), new SearchParametersDto(null, null, null, true, null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, [1, 2], null, null, null), new SearchParametersDto(null, null, null, false, [1, 2], null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, null, "London", null, null), new SearchParametersDto(null, null, null, false, null, "London", null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, null, null, "1.0", null), new SearchParametersDto(null, null, null, false, null, null, "1.0", null) },
        new object[] { new SearchParametersDto(null, null, null, false, null, null, null, "1.0"), new SearchParametersDto(null, null, null, false, null, null, null, "1.0") },
    ];
    
    [TestCaseSource(nameof(EqualTestCases))]
    public void Then_If_Values_Match_Then_Dtos_Are_Equal(SearchParametersDto left, SearchParametersDto right)
    {
        // act
        var result = left.Equals(right);
        
        // assert
        result.Should().BeTrue();
    }
    
    private static readonly object?[] NotEqualTestCases =
    [
        new object[] { new SearchParametersDto("foo", null, null, false, null, null, null, null), new SearchParametersDto("foo2", null, null, false, null, null, null, null) },
        new object[] { new SearchParametersDto(null, [1, 2], null, false, null, null, null, null), new SearchParametersDto(null, [1, 3], null, false, null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, 1, false, null, null, null, null), new SearchParametersDto(null, null, 2, false, null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, null, null, null, null), new SearchParametersDto(null, null, null, true, null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, [1, 2], null, null, null), new SearchParametersDto(null, null, null, false, [1, 3], null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, null, "London", null, null), new SearchParametersDto(null, null, null, false, null, "Glasgow", null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, null, null, "1.0", null), new SearchParametersDto(null, null, null, false, null, null, "1.1", null) },
        new object[] { new SearchParametersDto(null, null, null, false, null, null, null, "1.0 "), new SearchParametersDto(null, null, null, false, null, null, null, "1.1") },
    ];
    
    [TestCaseSource(nameof(NotEqualTestCases))]
    public void Then_If_Values_Do_Not_Match_Then_Dtos_Are_Not_Equal(SearchParametersDto left, SearchParametersDto right)
    {
        // act
        var result = left.Equals(right);
        
        // assert
        result.Should().BeFalse();
    }
}