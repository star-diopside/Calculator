using Calculator.Common.Function;
using Xunit;

namespace Calculator.Common.Tests.Function;

public class ReversePolishNotationTests
{
    [Fact]
    public void ConvertNull()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => ReversePolishNotation.Convert(null!));
        Assert.Equal("formula", ex.ParamName);
    }

    [Theory]
    [MemberData(nameof(ConvertNormalFormulaData))]
    public void ConvertNormalFormula(string formula, IEnumerable<string> expected)
    {
        var result = ReversePolishNotation.Convert(formula);
        Assert.Equal(expected, result);
    }

    public static IEnumerable<object[]> ConvertNormalFormulaData()
    {
        yield return ["11+234",
            (IEnumerable<string>)["11", "234", "+"]];

        yield return ["11+234*ab56",
            (IEnumerable<string>)["11", "234", "ab56", "*", "+"]];

        yield return ["11--234*ab56",
            (IEnumerable<string>)["11", "234", "-@", "ab56", "*", "-"]];

        yield return ["11*(234-ab56)*7",
            (IEnumerable<string>)["11", "234", "ab56", "-", "*", "7", "*"]];

        yield return ["11*((234-ab56)*7)",
            (IEnumerable<string>)["11", "234", "ab56", "-", "7", "*", "*"]];
    }

    [Theory]
    [MemberData(nameof(ConvertInvalidFormulaData))]
    public void ConvertInvalidFormula(string formula)
    {
        var result = ReversePolishNotation.Convert(formula);
        Assert.Throws<FormatException>(() => result.ToList());
    }

    public static IEnumerable<object[]> ConvertInvalidFormulaData()
    {
        yield return ["11+234a56"];
        yield return ["11+++234"];
        yield return ["11*(234-ab56*7"];
        yield return ["11*((234-ab56)*7))"];
        yield return ["11+234 56"];
    }
}
