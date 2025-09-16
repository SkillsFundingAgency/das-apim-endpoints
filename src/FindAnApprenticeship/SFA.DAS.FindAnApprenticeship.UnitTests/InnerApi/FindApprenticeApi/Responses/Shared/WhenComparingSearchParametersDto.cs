using SFA.DAS.FindAnApprenticeship.InnerApi.FindApprenticeApi.Responses.Shared;
using SFA.DAS.SharedOuterApi.Domain;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.FindApprenticeApi.Responses.Shared;

public class WhenComparingSearchParametersDto
{
    private static readonly object?[] EqualTestCases =
    [
        new object[] { new SearchParametersDto("foo", null, null, false, false, null, null, null, null, null), new SearchParametersDto("foo", null, null, false, false, null, null, null, null, null) },
        new object[] { new SearchParametersDto(null, [1, 2], null, false, false, null, null, null, null, null), new SearchParametersDto(null, [1, 2], null, false, false, null, null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, 1, false, false, null, null, null, null, null), new SearchParametersDto(null, null, 1, false, false, null, null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, false, null, null, null, null, null), new SearchParametersDto(null, null, null, false, false, null, null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, true, true, null, null, null, null, null), new SearchParametersDto(null, null, null, true, true, null, null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, false, [1, 2], null, null, null, null), new SearchParametersDto(null, null, null, false, false, [1, 2], null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, false, null, "London", null, null, null), new SearchParametersDto(null, null, null, false, false, null, "London", null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, false, null, null, "1.0", null, null), new SearchParametersDto(null, null, null, false, false, null, null, "1.0", null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, false, null, null, null, "1.0", null), new SearchParametersDto(null, null, null, false, false, null, null, null, "1.0", null) },
        new object[] { new SearchParametersDto(null, null, null, false, false, null, null, null, null, [ApprenticeshipTypes.Foundation]), new SearchParametersDto(null, null, null, false, false, null, null, null, null, [ApprenticeshipTypes.Foundation]) },
        new object[] { new SearchParametersDto(null, [], null, false, false, null, null, null, null, null), new SearchParametersDto(null, null, null, false, false, null, null, null, null, null) },
        new object[] { new SearchParametersDto(null, [], null, false, false, null, null, null, null, null), new SearchParametersDto(null, [], null, false, false, null, null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, false, null, null, null, null, null), new SearchParametersDto(null, [], null, false, false, null, null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, false, [], null, null, null, null), new SearchParametersDto(null, null, null, false, false, null, null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, false, [], null, null, null, null), new SearchParametersDto(null, null, null, false, false, [], null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, false, null, null, null, null, null), new SearchParametersDto(null, null, null, false, false, [], null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, false, null, null, null, null, []), new SearchParametersDto(null, null, null, false, false, null, null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, false, null, null, null, null, []), new SearchParametersDto(null, null, null, false, false, null, null, null, null, []) },
        new object[] { new SearchParametersDto(null, null, null, false, false, null, null, null, null, null), new SearchParametersDto(null, null, null, false, false, null, null, null, null, []) },
    ];
    
    [TestCaseSource(nameof(EqualTestCases))]
    public void Then_If_Values_Match_Then_Dtos_Are_Equal(SearchParametersDto left, SearchParametersDto right)
    {
        // act
        var result = left.Equals(right);
        
        // assert
        result.Should().BeTrue();
    }
    
    [TestCaseSource(nameof(EqualTestCases))]
    public void Then_Hashcodes_Should_Match(SearchParametersDto left, SearchParametersDto right)
    {
        // act
        var leftHash = left.GetHashCode();
        var rightHash = right.GetHashCode();
        
        // assert
        leftHash.Should().Be(rightHash);
    }
    
    private static readonly object?[] NotEqualTestCases =
    [
        new object[] { new SearchParametersDto("foo", null, null, false, false, null, null, null, null, null), new SearchParametersDto("foo2", null, null, false, false, null, null, null, null, null) },
        new object[] { new SearchParametersDto(null, [1, 2], null, false, false, null, null, null, null, null), new SearchParametersDto(null, [1, 3], null, false, false, null, null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, 1, false, false, null, null, null, null, null), new SearchParametersDto(null, null, 2, false, false, null, null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, false, null, null, null, null, null), new SearchParametersDto(null, null, null, true,true, null, null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, false, [1, 2], null, null, null, null), new SearchParametersDto(null, null, null, false, false, [1, 3], null, null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, false, null, "London", null, null, null), new SearchParametersDto(null, null, null, false, false, null, "Glasgow", null, null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, false, null, null, "1.0", null, null), new SearchParametersDto(null, null, null, false, false, null, null, "1.1", null, null) },
        new object[] { new SearchParametersDto(null, null, null, false, false, null, null, null, "1.0 ", null), new SearchParametersDto(null, null, null, false, false, null, null, null, "1.1", null) },
        new object[] { new SearchParametersDto(null, null, null, false, false, null, null, null, null, [ApprenticeshipTypes.Foundation]), new SearchParametersDto(null, null, null, false, false, null, null, null, null, [ApprenticeshipTypes.Standard]) },
        new object[] { new SearchParametersDto(null, null, null, false, false, null, null, null, null, []), new SearchParametersDto(null, null, null, false, false, null, null, null, null, [ApprenticeshipTypes.Standard]) },
        new object[] { new SearchParametersDto(null, null, null, false, false, null, null, null, null, [ApprenticeshipTypes.Standard]), new SearchParametersDto(null, null, null, false, false, null, null, null, null, [ApprenticeshipTypes.Standard, ApprenticeshipTypes.Foundation]) },
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